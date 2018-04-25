using Discord;
using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Domain.Payload.Embed;
using InjhinuityDiscordClient.Domain.Payload.Embed.Interfaces;
using InjhinuityDiscordClient.Enums;
using InjhinuityDiscordClient.Services.Interfaces;
using System.Collections.Generic;

namespace InjhinuityDiscordClient.Services
{
    public class EmbedPayloadFactory : IEmbedPayloadFactory
    {
        private readonly IBotConfig _botConfig;

        public EmbedPayloadFactory(IBotConfig botConfig)
        {
            _botConfig = botConfig;
        }

        public IEmbedPayload CreateEmbedPayload(EmbedStruct embedStruct, EmbedPayloadType embedType, string action, IUser author = null, 
                                                IList<string> descParams = null, IList<string> titleParams = null, IList<string> footerParams = null, 
                                                IEnumerable<(string, string, bool)> fields = null) =>
            new EmbedPayload
            {
                Action = action,
                Author = author,
                DescParams = descParams,
                EmbedPayloadType = embedType,
                EmbedStruct = embedStruct,
                FooterParams = footerParams,
                Fields = fields,
                TitleParams = titleParams
            };

        //TODO: Replace these by embed structures or something along those lines
        //public IEmbedPayload CreateErrorEmbedPayload(string titleCode, string description, IUser author = null,
        //                                             string thumbnailURL = "", IEnumerable<(string, string, bool)> fields = null) =>
        //    CreateEmbedPayload($"Error - Code: {titleCode}", description, _botConfig.ErrorColor, "Error", author, thumbnailURL, fields);

        //public IEmbedPayload CreateInfoEmbedPayload(string description, IUser author = null,
        //                                            string thumbnailURL = "", IEnumerable<(string, string, bool)> fields = null) =>
        //    CreateEmbedPayload("Info", description, _botConfig.InfoColor, "Info", author, thumbnailURL, fields);

        //public IEmbedPayload CreateLogEmbedPayload(string title, string description, string footerText = "", IUser author = null,
        //                                           string thumbnailURL = "", IEnumerable<(string, string, bool)> fields = null) =>
        //    CreateEmbedPayload(title, description, _botConfig.InfoColor, footerText, author, thumbnailURL, fields);
    }
}
