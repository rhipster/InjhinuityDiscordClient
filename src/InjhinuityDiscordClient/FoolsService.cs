//using Discord;
//using Discord.WebSocket;
//using InjhinuityDiscordClient.Services.Injection;
//using System.Threading.Tasks;

//namespace InjhinuityDiscordClient
//{
//    public class FoolsService : IService
//    {
//        private const ulong _safeChannelId = 377941113736265730;

//        public FoolsService(DiscordSocketClient client)
//        {
//            client.MessageUpdated += Client_MessageUpdated;
//        }

//        private async Task Client_MessageUpdated(Cacheable<IMessage, ulong> arg1, SocketMessage arg2, ISocketMessageChannel arg3)
//        {
//            if (arg2 != null)
//                await CheckMessageLength(arg3, (SocketUserMessage)arg2);
//        }

//        public async Task CheckMessageLength(IChannel channel, SocketUserMessage userMsg, SocketGuild guild = null)
//        {
//            if (channel.Id != _safeChannelId ||
//                (guild != null &&
//                guild.GetUser(userMsg.Author.Id).GuildPermissions.BanMembers))
//                return;

//            if (!CheckIfFourWordMessage(userMsg.Content))
//                await userMsg.DeleteAsync();
//        }

//        private bool CheckIfFourWordMessage(string content)
//        {
//            var split = content.Split(new char[] { ' ', '.', ',', '_', '-', '\'', '[' ,']', '/' });
//            return split.Length == 4;
//        }
//    }
//}
