using Discord;
using Discord.Commands;
using Discord.WebSocket;
using InjhinuityDiscordClient.Domain.Discord;
using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Handlers;
using InjhinuityDiscordClient.Services;
using InjhinuityDiscordClient.Services.Interfaces;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace InjhinuityDiscordClient
{
    public class InjhinuityInstance
    {
        private Logger _log;
        private IBotStartupService _startupService;
        private string _botConfigPath = "./Json/botConfig.json";

        public IBotCredentials BotCredentials { get; }
        public IBotServiceProvider Services { get; private set; }

        public DiscordSocketClient Client { get; }
        public CommandService CommandService { get; }
        public IBotConfig BotConfig { get; }
        public IOwnerLogger OwnerLogger { get; private set; }

        public bool Ready { get; private set; }

        public InjhinuityInstance()
        {
            //Logging configuration for the local bot instance
            var logConfig = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget()
            {
                Layout = @"${date:format=HH\:mm\:ss} ${logger} | ${message}"
            };

            logConfig.AddTarget("Console", consoleTarget);
            logConfig.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));

            LogManager.Configuration = logConfig;
            _log = LogManager.GetCurrentClassLogger();

            //Creates the bot's loggin credentials for Discord
            BotCredentials = new BotCredentials();

            var fileReader = new JsonFileReader();
            var json = fileReader.ReadFile(_botConfigPath);
            BotConfig = JsonConvert.DeserializeObject<BotConfig>(json);

            //Creates the client and hooks it up to our events for startup and logging
            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                MessageCacheSize = 1000,
                LogLevel = LogSeverity.Warning,
                ConnectionTimeout = int.MaxValue,
                AlwaysDownloadUsers = false,
            });

            Client.Ready += OnClientReady;
            Client.Log += ClientLog;

            //The command service handles the input the bot receives from text channels
            CommandService = new CommandService(new CommandServiceConfig()
            {
                CaseSensitiveCommands = false
            });
        }

        /// <summary>
        /// Starts the bot as an async process and blocks it
        /// </summary>
        /// <param name="args">Optional arguments for the bot, but aren't used here</param>
        /// <returns></returns>
        public async Task RunAndBlockAsync(params string[] args)
        {
            await RunAsync(args).ConfigureAwait(false);
            await Task.Delay(-1).ConfigureAwait(false);
        }

        /// <summary>
        /// Starts up the bot by connecting it to Discord. Also takes care hooking up our
        /// events that handles input and various commands. Finally adds all our command
        /// modules to our container.
        /// </summary>
        /// <param name="args">Optional arguments for the bot, but aren't used here</param>
        /// <returns></returns>
        public async Task RunAsync(params string[] args)
        {
            _log.Info("Starting Injhinuity [v soon tm]" /*+ StatService.BotVersion*/);

            var sw = Stopwatch.StartNew();

            await LoginAsync(BotCredentials.Token).ConfigureAwait(false);

            _log.Info("Loading dependencies...");
            AddDependencies();

            sw.Stop();
            _log.Info($"Took {sw.Elapsed.TotalSeconds:F2} seconds to connect.");

            var commandHandler = Services.GetService<CommandHandler>();
            var commandService = Services.GetService<CommandService>();

            // start handling messages received in commandhandler
            await commandHandler.StartHandling();
            await commandService.AddModulesAsync(GetType().GetTypeInfo().Assembly);
        }

        /// <summary>
        /// Adds every dependency our bot/services have to our DI container. The service provider
        /// will take care of initialising every service with it's own dependencies as they are added
        /// to the container. We also add some of them by hand, since they don't implement IService.
        /// </summary>
        private void AddDependencies()
        {
            Services = new ServiceProvider.ServiceProviderBuilder()
                .AddManual(BotCredentials)
                .AddManual(BotConfig)
                .AddManual(Client)
                .AddManual(CommandService)
                .AddManual(_log)
                .AddManual(this)
                .LoadFrom(Assembly.GetEntryAssembly())
                .Build();

            var commandHandler = Services.GetService<CommandHandler>();
            commandHandler.AddServices(Services);
        }

        /// <summary>
        /// Logs the bot into discord as a bot instance with it's token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task LoginAsync(string token)
        {
            _log.Info("Logging in...");

            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();

            Console.WriteLine("In carnage, I bloom, like a flower in the dawn.");
        }

        /// <summary>
        /// Method hooked to an event for logging purposes
        /// </summary>
        /// <param name="arg">Message from the event that occured</param>
        /// <returns></returns>
        private Task ClientLog(LogMessage arg)
        {
            _log.Error(arg.Message);
            var ex = arg.Exception;
            if (ex != null)
            {
                _log.Error(ex.Message);
                if (OwnerLogger != null)
                    OwnerLogger.LogException(ex);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Hooked to an event which dictates wether the bot is ready to execute or not
        /// </summary>
        /// <returns></returns>
        private async Task OnClientReady()
        {
            Ready = true;
            OwnerLogger = Services.GetService<IOwnerLogger>();
            await OwnerLogger.SetOwnerDMChannel();

            _startupService = Services.GetService<IBotStartupService>();
            Client.JoinedGuild += OnClientJoinedGuild;

            await _startupService.SynchroniseGuilds();
        }

        private async Task OnClientJoinedGuild(SocketGuild arg) =>
            await _startupService.SynchroniseGuilds();
    }
}
