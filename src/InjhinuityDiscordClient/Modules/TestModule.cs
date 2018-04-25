//using Discord.Commands;
//using InjhinuityDiscordClient.Services;
//using System.Threading.Tasks;
//using InjhinuityDiscordClient.DiscordObjects;
//using System.Collections.Generic;

//namespace InjhinuityDiscordClient.Modules
//{
//    public class TestModule : BotModuleBase
//    {
//        public TestModule(IAPIService apiService, IBotConfig botConfig, IEmbedService embedService,
//                        IOwnerLogger ownerLogger, IDiscordPayloadFactory payloadFactory, IResourceService resources)
//            : base(apiService, botConfig, embedService, ownerLogger, payloadFactory, resources)
//        {
//        }

//        [Command("test")]
//        private async Task Test()
//        {
//            var payload = _embedPayloadFactory.CreateEmbedPayload("a", "a", _botConfig.CommandColor, "", null, "", new List<(string, string)> { ("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", "AAA") });
//            var embed = _embedService.CreateFieldEmbed(payload);

//            await SendEmbedAsync(embed);
//        }

//        [Command("testall")]
//        private async Task TestAll()
//        {
//            var payloads = new List<IAPIPayload>
//            {
//                new CommandAPIPayload(new Command(Context.Guild.Id, "4444", "4444")),
//                new EventAPIPayload(new Event(Context.Guild.Id, "4444", "4444", 4444, "4444", 4444)),
//                new ReactionAPIPayload(new Reaction(Context.Guild.Id, "4444", "4444")),
//                new RoleAPIPayload(new Role(Context.Guild.Id, "4444", 4444)),
//                new UserMessageAPIPayload(new UserMessage(Context.Guild.Id, 4444, 4444, "4444")),
//            };

//            var str = "";
//            foreach (var payload in payloads)
//                str += $"{payload.ToJson()} - Post - {(await _apiService.PostAsync(payload)).StatusCode}\n\n";

//            await SendMessageAsync(str);

//            var putPayloads = new List<IAPIPayload>
//            {
//                new CommandAPIPayload(new Command(Context.Guild.Id, "4444", "6666")),
//                new EventAPIPayload(new Event(Context.Guild.Id, "4444", "6666", 6666, "6666", 6666)),
//                new ChannelAPIPayload(new Channel(Context.Guild.Id, "4444", "4444", 6666)),
//                new LogConfigAPIPayload(new LogConfig(Context.Guild.Id, 6666)),
//                new ParamAPIPayload(new GuildParam(Context.Guild.Id, "adg", "AutoDeleteGreet", "True")),
//                new ReactionAPIPayload(new Reaction(Context.Guild.Id, "4444", "6666")),
//            };

//            str = "";
//            foreach (var payload in putPayloads)
//                str += $"{payload.ToJson()} - Put - {(await _apiService.PutAsync(payload)).StatusCode}\n\n";

//            await SendMessageAsync(str);

//            var getPayloads = new List<IAPIPayload>
//            {
//                new CommandAPIPayload(new Command(Context.Guild.Id, "4444", "6666")),
//                new EventAPIPayload(new Event(Context.Guild.Id, "4444", "6666", 6666, "6666", 6666)),
//                new ChannelAPIPayload(new Channel(Context.Guild.Id, "4444", "4444", 6666)),
//                new LogConfigAPIPayload(new LogConfig(Context.Guild.Id, 6666)),
//                new ParamAPIPayload(new GuildParam(Context.Guild.Id, "adg", "AutoDeleteGreet", "True")),
//                new ReactionAPIPayload(new Reaction(Context.Guild.Id, "4444", "6666")),
//                new UserMessageAPIPayload(new UserMessage(Context.Guild.Id, 4444, 6666, "6666")),
//                new RoleAPIPayload(new Role(Context.Guild.Id, "4444", 4444)),
//                new GuildConfigAPIPayload(new GuildConfig() { GuildId = Context.Guild.Id }),
//            };

//            str = "";
//            foreach (var payload in getPayloads)
//                str += $"{payload.ToJson()} - Get - {(await _apiService.GetAsync(payload)).StatusCode}\n\n";

//            await SendMessageAsync(str);

//            var deletePayloads = new List<IAPIPayload>
//            {
//                new CommandAPIPayload(new Command(Context.Guild.Id, "4444", "6666")),
//                new EventAPIPayload(new Event(Context.Guild.Id, "4444", "6666", 6666, "6666", 6666)),
//                new ReactionAPIPayload(new Reaction(Context.Guild.Id, "4444", "6666")),
//                new RoleAPIPayload(new Role(Context.Guild.Id, "4444", 4444)),
//            };

//            str = "";
//            foreach (var payload in deletePayloads)
//                str += $"{payload.ToJson()} - Delete - {(await _apiService.DeleteAsync(payload)).StatusCode}\n\n";

//            await SendMessageAsync(str);
//            return;
//        }
//    }
//}
