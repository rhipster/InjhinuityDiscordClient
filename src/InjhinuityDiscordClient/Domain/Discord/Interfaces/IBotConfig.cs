namespace InjhinuityDiscordClient.Domain.Discord.Interfaces
{
    public interface IBotConfig
    {
        string EmbedTimeStamp { get; }
        string PlayingStatus { get; }

        string RoundAvatarUrl { get; }
        string SecondAvatarURL { get; }
        string BotInviteLink { get; }

        string Aqua { get; }
        string Blue { get; }
        string Grey { get; }
        string Green { get; }
        string LightOrange { get; }
        string Marine { get; }
        string Olive { get; }
        string Orange { get; }
        string Purple { get; }
        string Red { get; }
        string White { get; }

        string APIActionsPath { get; }
        string ChannelIDSPath { get; }
        string EmbedStructuresPath { get; }
        string ErrorsPath { get; }
        string GuildParamsPath { get; }
        string LogEventsPath { get; }
        string ResourcesPath { get; }
        string RegionsPath { get; }

        bool Debug { get; set; }
        string PollSeparator { get; set; }
        string Prefix { get; set; }
    }
}
