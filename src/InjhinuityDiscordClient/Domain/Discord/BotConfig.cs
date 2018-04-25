using InjhinuityDiscordClient.Domain.Discord.Interfaces;

namespace InjhinuityDiscordClient.Domain.Discord
{
    public class BotConfig : IBotConfig
    {
        public string EmbedTimeStamp { get; set; }
        public string PlayingStatus { get; }

        public string RoundAvatarUrl { get; set; }
        public string SecondAvatarURL { get; set; }
        public string BotInviteLink { get; set; }

        public string Aqua { get; set; }
        public string Blue { get; set; }
        public string Grey { get; set; }
        public string Green { get; set; }
        public string LightOrange { get; set; }
        public string Marine { get; set; }
        public string Olive { get; set; }
        public string Orange { get; set; }
        public string Purple { get; set; }
        public string Red { get; set; }
        public string White { get; set; }

        public string APIActionsPath { get; set; }
        public string ChannelIDSPath { get; set; }
        public string EmbedStructuresPath { get; set; }
        public string ErrorsPath { get; set; }
        public string GuildParamsPath { get; set; }
        public string LogEventsPath { get; set; }
        public string ResourcesPath { get; set; }
        public string RegionsPath { get; set; }

        public bool Debug { get; set; }
        public string PollSeparator { get; set; }
        public string Prefix { get; set; }
    }
}
