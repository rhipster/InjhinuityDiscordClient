using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Enums;
using InjhinuityDiscordClient.Services.Interfaces;

namespace InjhinuityDiscordClient.Services
{
    public class ColorConverter : IColorConverter
    {
        private readonly IBotConfig _botConfig;

        public ColorConverter(IBotConfig botConfig)
        {
            _botConfig = botConfig;
        }

        public string GetLogColorCodeFromPayloadType(EmbedPayloadType type)
        {
            switch (type)
            {
                case EmbedPayloadType.UserBanned:
                    return _botConfig.White;
                case EmbedPayloadType.UserJoined:
                    return _botConfig.Grey;
                case EmbedPayloadType.UserLeft:
                    return _botConfig.Grey;
                case EmbedPayloadType.MsgDeleted:
                    return _botConfig.Grey;
                case EmbedPayloadType.MsgUpdated:
                    return _botConfig.Grey;
                case EmbedPayloadType.UserUnbanned:
                    return _botConfig.Grey;
                case EmbedPayloadType.UserUpdated:
                    return _botConfig.Grey;
                default:
                    return _botConfig.Grey;
            }
        }

        public string GetColorCodeFromPayloadType(EmbedPayloadType type)
        {
            switch (type)
            {
                case EmbedPayloadType.Channel:
                    return _botConfig.LightOrange;
                case EmbedPayloadType.Command:
                    return _botConfig.Orange;
                case EmbedPayloadType.Error:
                    return _botConfig.Red;
                case EmbedPayloadType.Event:
                    return _botConfig.Blue;
                case EmbedPayloadType.Help:
                    return _botConfig.Olive;
                case EmbedPayloadType.Info:
                    return _botConfig.Purple;
                case EmbedPayloadType.Ok:
                    return _botConfig.Green;
                case EmbedPayloadType.Param:
                    return _botConfig.Purple;
                case EmbedPayloadType.Poll:
                    return _botConfig.Aqua;
                case EmbedPayloadType.Reaction:
                    return _botConfig.LightOrange;
                case EmbedPayloadType.Role:
                    return _botConfig.Green;
                case EmbedPayloadType.Wiki:
                    return _botConfig.Marine;
                default:
                    return _botConfig.Grey;
            }
        }
    }
}
