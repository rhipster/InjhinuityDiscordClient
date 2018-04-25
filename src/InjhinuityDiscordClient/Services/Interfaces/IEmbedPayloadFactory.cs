using Discord;
using InjhinuityDiscordClient.Domain.Payload.Embed.Interfaces;
using InjhinuityDiscordClient.Enums;
using InjhinuityDiscordClient.Services.Injection;
using System.Collections.Generic;

namespace InjhinuityDiscordClient.Services.Interfaces
{
    public interface IEmbedPayloadFactory : IService
    {
        IEmbedPayload CreateEmbedPayload(EmbedStruct embedStruct, EmbedPayloadType embedType, string action, IUser author = null,
                                         IList<string> descParams = null, IList<string> titleParams = null, IList<string> footerParams = null,
                                         IEnumerable<(string, string, bool)> fields = null);
        //IEmbedPayload CreateErrorEmbedPayload(string titleCode, string description, IUser author = null, string thumbnailURL = "",
        //                                      IEnumerable<(string, string, bool)> fields = null);
        //IEmbedPayload CreateInfoEmbedPayload(string description, IUser author = null, string thumbnailURL = "",
        //                                     IEnumerable<(string, string, bool)> fields = null);
        //IEmbedPayload CreateLogEmbedPayload(string title, string description, string footerText = "", IUser author = null,
        //                                    string thumbnailURL = "", IEnumerable<(string, string, bool)> fields = null);
    }
}
