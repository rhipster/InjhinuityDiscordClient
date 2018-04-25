using Discord;
using InjhinuityDiscordClient.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace InjhinuityDiscordClient.Services
{
    public class BotCredentials : IBotCredentials
    {
        private Logger _log;

        public ulong ClientID { get; }
        public string Token { get; }
        public string ApiClientID { get; }
        public string ApiClientSecret { get; }
        public ImmutableArray<ulong> OwnerIDs { get; }

        public bool IsOwner(IUser u) => OwnerIDs.Contains(u.Id);
        public static string CredsFileName { get; } = "./Creds/credentials.json";
        public static string DBFileName { get; } = "Filename=Injhinuity.db";

        public BotCredentials()
        {
            _log = LogManager.GetCurrentClassLogger();

            //Credential file check. If it's missing something went really wrong...
            if (!File.Exists(CredsFileName))
            {
                _log.Error($"Credentials file is missing. This should totally not happen.");
                _log.Error($"The file should be found here: {CredsFileName}");
                _log.Error($"CurrentDirectory: {Directory.GetCurrentDirectory()}");
                Console.ReadKey();
                Environment.Exit(3);
            }

            try
            {
                //Fetches the credentials file and creates a configBuilder with it's content
                var configBuilder = new ConfigurationBuilder()
                    .AddJsonFile(CredsFileName);

                var data = configBuilder.Build();

                ulong.TryParse(data[nameof(ClientID)], out ulong clID);
                ClientID = clID;

                //Fetches data from the configBuilder with their respective Json names in the creds file
                Token = data[nameof(Token)];
                if (string.IsNullOrWhiteSpace(Token))
                    LogCredFileErrorAndExit(nameof(Token));

                ApiClientID = data[nameof(ApiClientID)];
                if (string.IsNullOrWhiteSpace(ApiClientID))
                    LogCredFileErrorAndExit(nameof(ApiClientID));

                ApiClientSecret = data[nameof(ApiClientSecret)];
                if (string.IsNullOrWhiteSpace(ApiClientSecret))
                    LogCredFileErrorAndExit(ApiClientSecret);

                OwnerIDs = data.GetSection("OwnerIDs").GetChildren()
                    .Select(c => ulong.Parse(c.Value)).ToImmutableArray();
            }
            catch (Exception ex)
            {
                _log.Fatal(ex, ex.Message);
                throw;
            }
        }

        private void LogCredFileErrorAndExit(string credParamName)
        {
            _log.Error($"{credParamName} is missing from credentials.json. Add it and restart the program.");
            Console.ReadKey();
            Environment.Exit(3);
        }
    }
}
