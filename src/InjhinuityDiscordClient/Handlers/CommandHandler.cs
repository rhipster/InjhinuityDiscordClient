using Discord;
using Discord.Commands;
using Discord.WebSocket;
using InjhinuityDiscordClient.Domain.Exceptions;
using InjhinuityDiscordClient.Enums;
using InjhinuityDiscordClient.Extensions;
using InjhinuityDiscordClient.Services;
using InjhinuityDiscordClient.Services.Injection;
using InjhinuityDiscordClient.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace InjhinuityDiscordClient.Handlers
{    
    public class CommandHandler : IService
    {
        private readonly Logger _log;

        private readonly CommandService _commandService;
        private readonly CustomCommandHandler _customCommandHandler;
        private readonly DiscordSocketClient _client;
        private readonly InjhinuityInstance _injhinuity;

        private readonly IEmbedPayloadFactory _embedPayloadFactory;
        private readonly IEmbedService _embedService;
        private readonly IOwnerLogger _ownerLogger;
        private readonly IResources _resources;

        //private readonly FoolsService _foolsService;

        private IServiceProvider _services;

        public event Func<IUserMessage, CommandInfo, Task> CommandExecuted = delegate { return Task.CompletedTask; };
        public event Func<CommandInfo, ITextChannel, string, Task> CommandErrored = delegate { return Task.CompletedTask; };

        private long _timeErrorCodeExceptionWasShown = 0;
        
        //Represents one minute in ticks
        private const long ERROR_CODE_COOLDOWN = 60 * 10000000;

        //Used to lock for exceptions to avoid having multiple exceptions being logged at once
        private readonly object errorLogLock = new object();

        public CommandHandler(CommandService commandService, DiscordSocketClient client, CustomCommandHandler customCommandHandler,
                                InjhinuityInstance injhinuity, IOwnerLogger ownerLogger,  IResources resources,
                                IEmbedPayloadFactory embedPayloadFactory, IEmbedService embedService/*, FoolsService foolsService
                                ConversationHandler conversationHandler, IPollService pollService*/)
        {
            _log = LogManager.GetCurrentClassLogger();

            _client = client;
            _commandService = commandService;
            _injhinuity = injhinuity;
            _ownerLogger = ownerLogger;
            _customCommandHandler = customCommandHandler;
            _resources = resources;
            _embedPayloadFactory = embedPayloadFactory;
            _embedService = embedService;
            //_foolsService = foolsService;
            //_conversationHandler = conversationHandler;
            //_pollService = pollService;
        }

        public void AddServices(IBotServiceProvider services) =>
            _services = services;

        public Task StartHandling()
        {
            _client.MessageReceived += (msg) => { var _ = Task.Run(async () => await MessageReceivedHandler(msg)); return Task.CompletedTask; };
            return Task.CompletedTask;
        }

        private async Task MessageReceivedHandler(SocketMessage msg)
        {
            if (msg.Author.IsBot || !_injhinuity.Ready) // Wait until bot connected and initialized
            return;

            if (!(msg is SocketUserMessage usrMsg))
                return;

            if (msg.Content.Length <= 0)
                return;

            var channel = msg.Channel as ISocketMessageChannel;
            var guild = (msg.Channel as SocketTextChannel)?.Guild;

            var context = new SocketCommandContext(_client, usrMsg);

            // Initialise error handler for exceptions to reuse in case of errors
            async void errorCodeHandler(ErrorCodeException ex)
            {
                var ticks = DateTimeOffset.Now.Ticks;

                if (_timeErrorCodeExceptionWasShown + ERROR_CODE_COOLDOWN <= ticks)
                {
                    _timeErrorCodeExceptionWasShown = ticks;

                    if (ex.ErrorCode != "0004")
                        await _ownerLogger.LogError(ex.ErrorCode, ex.Message);

                    var titleParams = new List<string> { ex.ErrorCode };
                    var footerParams = new List<string> { ex.ErrorCode };

                    var payload = _embedPayloadFactory.CreateEmbedPayload(EmbedStruct.ErrorCode, EmbedPayloadType.Error, ex.ErrorCode,
                                                                          context.Message.Author, null, titleParams, footerParams);
                    var embed = _embedService.CreateBaseEmbed(payload);

                    var message = await context.Channel.SendMessageAsync("", false, embed.Build());
                    await message.DeleteAfterLong();

                    LogException(ex);
                }
                else
                    await usrMsg.AddReactionAsync(new Emoji("❌"));
            }

            try
            {
                /*if (await _conversationHandler.TryHandlingConversationForUser(context, usrMsg.Content))
                    return;*/

                if (await _customCommandHandler.TryHandlingCustomCommand(context, usrMsg.Content))
                    return;

                /*_pollService.TryAddingNewVote(msg.Content, context);*/

                await TryRunCommand(guild, channel, usrMsg, context, errorCodeHandler);

                /*await _foolsService.CheckMessageLength(channel, (SocketUserMessage)msg, guild);*/
            }
            catch (ErrorCodeException ex) { errorCodeHandler(ex); }
            catch (HttpRequestException ex) { errorCodeHandler(new ErrorCodeException("0002", ex.Message, ex.InnerException)); }
            catch (Exception ex)
            {
                await _ownerLogger.LogException(new BotException(context, ex.TargetSite.GetType().Name, ex.Message, ex.StackTrace, ex.InnerException));
                LogException(ex);
            }
        }

        public async Task TryRunCommand(SocketGuild guild, ISocketMessageChannel channel, IUserMessage usrMsg, ICommandContext context, Action<ErrorCodeException> errorCodeHandler)
        {
            string messageContent = usrMsg.Content;
            var prefix = _injhinuity.BotConfig.Prefix;

            if (messageContent.StartsWith(prefix))
            {
                var result = await ExecuteCommandAsync(context, messageContent, prefix.Length, _services, MultiMatchHandling.Best, errorCodeHandler).ConfigureAwait(false);

                if (result.Success)
                    await CommandExecuted(usrMsg, result.Info);
                else if (result.Error != null && guild != null)
                    await CommandErrored(result.Info, channel as ITextChannel, result.Error);
            }
        }

        public async Task<(bool Success, string Error, CommandInfo Info)> ExecuteCommandAsync(ICommandContext context, string input, int argPos, IServiceProvider services, 
                                                                                              MultiMatchHandling multiMatchHandling, Action<ErrorCodeException> errorCodeHandler)
            => await ExecuteCommand(context, input.Substring(argPos), services, multiMatchHandling, errorCodeHandler);

        public async Task<(bool Success, string Error, CommandInfo Info)> ExecuteCommand(ICommandContext context, string input, IServiceProvider services, 
                                                                                         MultiMatchHandling multiMatchHandling, Action<ErrorCodeException> errorCodeHandler)
        {
            var searchResult = _commandService.Search(context, input);
            if (!searchResult.IsSuccess)
                return (false, null, null);

            var commands = searchResult.Commands;
            var preconditionResults = new Dictionary<CommandMatch, PreconditionResult>();

            foreach (var match in commands)
            {
                preconditionResults[match] = await match.Command.CheckPreconditionsAsync(context, services).ConfigureAwait(false);
            }

            var successfulPreconditions = preconditionResults
                .Where(x => x.Value.IsSuccess)
                .ToArray();

            if (successfulPreconditions.Length == 0)
            {
                // All preconditions failed, return the one from the highest priority command
                var bestCandidate = preconditionResults
                    .OrderByDescending(x => x.Key.Command.Priority)
                    .FirstOrDefault(x => !x.Value.IsSuccess);

                errorCodeHandler(new ErrorCodeException("0004"));

                return (false, bestCandidate.Value.ErrorReason, commands[0].Command);
            }

            var parseResultsDict = new Dictionary<CommandMatch, ParseResult>();
            foreach (var pair in successfulPreconditions)
            {
                var parseResult = await pair.Key.ParseAsync(context, searchResult, pair.Value, services).ConfigureAwait(false);

                if (parseResult.Error == CommandError.MultipleMatches)
                {
                    IReadOnlyList<TypeReaderValue> argList, paramList;
                    switch (multiMatchHandling)
                    {
                        case MultiMatchHandling.Best:
                            argList = parseResult.ArgValues.Select(x => x.Values.OrderByDescending(y => y.Score).First()).ToImmutableArray();
                            paramList = parseResult.ParamValues.Select(x => x.Values.OrderByDescending(y => y.Score).First()).ToImmutableArray();
                            parseResult = ParseResult.FromSuccess(argList, paramList);
                            break;
                    }
                }

                parseResultsDict[pair.Key] = parseResult;
            }
            // Calculates the 'score' of a command given a parse result
            float CalculateScore(CommandMatch match, ParseResult parseResult)
            {
                float argValuesScore = 0, paramValuesScore = 0;

                if (match.Command.Parameters.Count > 0)
                {
                    var argValuesSum = parseResult.ArgValues?.Sum(x => x.Values.OrderByDescending(y => y.Score).FirstOrDefault().Score) ?? 0;
                    var paramValuesSum = parseResult.ParamValues?.Sum(x => x.Values.OrderByDescending(y => y.Score).FirstOrDefault().Score) ?? 0;

                    argValuesScore = argValuesSum / match.Command.Parameters.Count;
                    paramValuesScore = paramValuesSum / match.Command.Parameters.Count;
                }

                var totalArgsScore = (argValuesScore + paramValuesScore) / 2;
                return match.Command.Priority + totalArgsScore * 0.99f;
            }

            // Order the parse results by their score so that we choose the most likely result to execute
            var parseResults = parseResultsDict
                .OrderByDescending(x => CalculateScore(x.Key, x.Value));

            var successfulParses = parseResults
                .Where(x => x.Value.IsSuccess)
                .ToArray();

            if (successfulParses.Length == 0)
            {
                // All parses failed, return the one from the highest priority command, using score as a tie breaker
                var bestMatch = parseResults
                    .FirstOrDefault(x => !x.Value.IsSuccess);
                return (false, bestMatch.Value.ErrorReason, commands[0].Command);
            }

            var cmd = successfulParses[0].Key.Command;

            // Bot will ignore commands which are ran more often than what specified by
            // GlobalCommandsCooldown constant (miliseconds)
            //if (!UsersOnShortCooldown.Add(context.Message.Author.Id))
            //    return (false, null, cmd);
            //return SearchResult.FromError(CommandError.Exception, "You are on a global cooldown.");

            // If we get this far, at least one parse was successful. Execute the most likely overload.
            var chosenOverload = successfulParses[0];
            var execResult = (ExecuteResult)await chosenOverload.Key.ExecuteAsync(context, chosenOverload.Value, services).ConfigureAwait(false);

            if (execResult.Exception is Exception ex)
            {
                lock (errorLogLock)
                {
                    var now = DateTime.Now;
                    File.AppendAllText($"./command_errors_{now:yyyy-MM-dd}.txt",
                        $"[{now:HH:mm-yyyy-MM-dd}]" + Environment.NewLine
                        + execResult.Exception.ToString() + Environment.NewLine
                        + "------" + Environment.NewLine);
                    _log.Warn(execResult.Exception);
                }
                await _ownerLogger.LogBotException(new BotException(context, cmd.Module.Name, ex.Message, ex.StackTrace, ex.InnerException));
            }

            return (true, null, cmd);
        }

        private void LogException(Exception e)
        {
            _log.Warn("Error in CommandHandler");
            _log.Warn(e);
            LogInnerException(e);
        }

        private void LogInnerException(Exception e)
        {
            if (e.InnerException is Exception inner)
            {
                _log.Warn("Inner Exception of the error in CommandHandler");
                _log.Warn(inner);
            }
        }
    }
}
