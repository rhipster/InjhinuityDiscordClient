using Discord;
using InjhinuityDiscordClient.Enums;
using System.Collections.Generic;

namespace InjhinuityDiscordClient.Dto.Interfaces
{
    public interface IEmbedDto
    {
        string Action { get; set; }
        EmbedStruct EmbedStruct { get; set; }
        IList<string> TitleParams { get; set; }
        IList<string> DescParams { get; set; }
        IList<string> FooterParams { get; set; }
        IUser Author { get; set; }
        IEnumerable<(string, string, bool)> Fields { get; set; }
        EmbedPayloadType EmbedPayloadType { get; set; }
    }
}
