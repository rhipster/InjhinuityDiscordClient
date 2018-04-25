using Discord;
using InjhinuityDiscordClient.Domain.Payload.Embed.Interfaces;
using InjhinuityDiscordClient.Enums;
using System.Collections.Generic;

namespace InjhinuityDiscordClient.Domain.Payload.Embed
{
    public class EmbedPayload : IEmbedPayload
    {
        public string Action { get; set; }
        public EmbedStruct EmbedStruct { get; set; }
        public IList<string> TitleParams { get; set; }
        public IList<string> DescParams { get; set; }
        public IList<string> FooterParams { get; set; }
        public IUser Author { get; set; }
        public IEnumerable<(string, string, bool)> Fields { get; set; }
        public EmbedPayloadType EmbedPayloadType { get; set; }
    }
}
