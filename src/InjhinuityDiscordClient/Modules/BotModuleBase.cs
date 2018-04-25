using Discord;
using Discord.Commands;
using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Enums;
using InjhinuityDiscordClient.Extensions;
using InjhinuityDiscordClient.Services.Interfaces;
using NLog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InjhinuityDiscordClient.Modules
{
    [RequireContext(ContextType.Guild)]
    public abstract class BotModuleBase : ModuleBase<SocketCommandContext>
    {
        protected Logger _log = LogManager.GetCurrentClassLogger();

        protected readonly IBotConfig _botConfig;
        protected readonly IEmbedService _embedService;
        protected readonly IEmbedPayloadFactory _embedPayloadFactory;
        protected readonly IResources _resources;

        protected string _subModuleName = "BotModuleBase";
        protected string _moduleErrorCode = "0003";

        public BotModuleBase(IBotConfig botConfig, IEmbedService embedService, IEmbedPayloadFactory embedPayloadFactory, 
                             IResources resources)
        {
            _botConfig = botConfig;
            _embedService = embedService;
            _embedPayloadFactory = embedPayloadFactory;
            _resources = resources;

            _subModuleName = GetType().Name;
        }

        protected async Task<IUserMessage> SendMessageAsync(string msg) => 
            await Context.Channel.SendMessageAsync(msg);

        protected async Task<IUserMessage> SendMessageWithMentionAsync(string msg) => 
            await Context.Channel.SendMessageAsync($"{Context.User.Mention}\n{msg}");

        protected async Task<IUserMessage> SendEmbedAsync(EmbedBuilder embedBuilder, string msg = "") => 
            await Context.Channel.SendMessageAsync(msg, false, embedBuilder.Build());

        protected async Task<IUserMessage> SendEmbedWithMentionAsync(EmbedBuilder embedBuilder, string msg = "") => 
            await Context.Channel.SendMessageAsync($"{Context.User.Mention}\n{msg}", false, embedBuilder.Build());

        protected async Task SendDeletableEmbed(EmbedBuilder embedBuilder, string msg = "")
        {
            var deletable = await SendEmbedAsync(embedBuilder, msg);

            deletable.DeleteAfterShort();
            Context.Message.DeleteAfterShort();
        }

        protected async Task HandleCommandResult(string action, bool isSuccess, IList<string> descParams = null, 
                                                 IList<string> titleParams = null, IList<string> footerParams = null)
        {
            var payload = _embedPayloadFactory.CreateEmbedPayload(GetStruct(isSuccess), GetType(isSuccess), action, Context.User,
                                                                  descParams, titleParams, footerParams);
            var embed = _embedService.CreateBaseEmbed(payload);

            await SendDeletableEmbed(embed);
        }

        protected async Task HandleModuleError()
        {
            var payload = _embedPayloadFactory.CreateEmbedPayload(EmbedStruct.ErrorCode, GetType(false), _moduleErrorCode);
            var embed = _embedService.CreateBaseEmbed(payload);

            await SendDeletableEmbed(embed);
        }

        protected EmbedStruct GetStruct(bool isSuccess) =>
            isSuccess ?
                EmbedStruct.Success :
                EmbedStruct.Failure;

        protected EmbedPayloadType GetType(bool isSuccess) =>
            isSuccess ?
                EmbedPayloadType.Info :
                EmbedPayloadType.Error;

        protected IList<string> ToList(params string[] args) =>
            new List<string>(args);
    }
}
