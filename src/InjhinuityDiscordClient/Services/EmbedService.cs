using Discord;
using InjhinuityDiscordClient.Domain.Discord.Interfaces;
using InjhinuityDiscordClient.Domain.Payload.Embed.Interfaces;
using InjhinuityDiscordClient.Extensions;
using InjhinuityDiscordClient.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InjhinuityDiscordClient.Services
{
    public class EmbedService : IEmbedService
    {
        private readonly IFileReader _fileReader;
        private readonly IResources _resources;
        private readonly IColorConverter _colorConverter;

        private readonly Dictionary<string, Dictionary<string, string>> _structures;

        public EmbedService(IFileReader fileReader, IResources resources, IColorConverter colorConverter, 
                            IBotConfig botConfig)
        {
            _fileReader = fileReader;
            _resources = resources;
            _colorConverter = colorConverter;

            var json = _fileReader.ReadFile(botConfig.EmbedStructuresPath);
            _structures = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
        }

        public EmbedBuilder CreateBaseEmbed(IEmbedPayload payload)
        {
            var titleResx = GetTitleResx(payload);
            var descResx = GetDescResx(payload);
            var footerResx = GetFooterResx(payload);

            return new EmbedBuilder()
            {
                Title = ResolveFormatting(titleResx, payload.TitleParams?.ToArray()),
                Description = ResolveFormatting(descResx, payload.DescParams?.ToArray()),
                Color = new Color(Convert.ToUInt32(_colorConverter.GetColorCodeFromPayloadType(payload.EmbedPayloadType), 16)),
                Footer = new EmbedFooterBuilder
                {
                    IconUrl = payload.Author == null ? "" : payload.Author.GetAvatarUrl(),
                    Text = payload.Author == null ? 
                        ResolveFormatting(footerResx, payload.FooterParams?.ToArray()) : 
                        $"{payload.Author.Username} | {ResolveFormatting(footerResx, payload.FooterParams?.ToArray())}"
                },
                Timestamp = DateTime.UtcNow,
            };
        }

        public EmbedBuilder CreateFieldEmbed(IEmbedPayload payload)
        {
            var baseEmbed = CreateBaseEmbed(payload);

            foreach (var pair in payload.Fields)
                if (pair.Item1 != null && pair.Item2 != null)
                    baseEmbed.AddField(pair.Item1.TrimForFieldTitle(), pair.Item2.TrimForField(), pair.Item3);

            return baseEmbed;
        }

        private string GetTitleResx(IEmbedPayload payload)
        {
            var title = _structures[payload.EmbedStruct.ToString()]["title"];
            return string.Format(title, payload.Action);
        }

        private string GetDescResx(IEmbedPayload payload)
        {
            var desc = _structures[payload.EmbedStruct.ToString()]["desc"];
            return string.Format(desc, payload.Action);
        }

        private string GetFooterResx(IEmbedPayload payload) =>
            _structures[payload.EmbedStruct.ToString()]["footer"];

        private string ResolveFormatting(string resx, string[] args) =>
            args != null && args.Length > 0 ?
                _resources.GetFormattedResource(resx, args) :
                _resources.GetResource(resx);
    }
}
