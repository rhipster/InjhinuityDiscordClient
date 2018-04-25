using Discord;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace InjhinuityDiscordClient.Extensions
{
    public static class Extensions
    {
        private const int FIELD_MESSAGE_LENGTH = 1024;
        private const int FIELD_MESSAGE_TRIM_LENGTH = 1020;
        private const int FIELD_TITLE_LENGTH = 256;
        private const int FIELD_TITLE_TRIM_LENGTH = 252;

        public static Task<IUserMessage> SendEmbedAsync(this IMessageChannel channel, EmbedBuilder embedBuilder) =>
            channel.SendMessageAsync("", false, embedBuilder.Build());

        public static Task<IUserMessage> SendEmbedMsgAsync(this IMessageChannel channel, string msg, EmbedBuilder embedBuilder) =>
            channel.SendMessageAsync(msg, false, embedBuilder.Build());

        public static string TrimForField(this string msg) =>
            msg.Length < FIELD_MESSAGE_LENGTH 
            ? msg 
            : $"{msg.Substring(0, FIELD_MESSAGE_TRIM_LENGTH)}...";

        public static string TrimForFieldTitle(this string msg) =>
            msg.Length < FIELD_TITLE_LENGTH 
            ? msg 
            : $"{msg.Substring(0, FIELD_TITLE_TRIM_LENGTH)}...";

        public static string FullName(this IUser user) =>
            $"{user.Username}#{user.Discriminator}";

        public static async Task<IMessage> DeleteAfter(this IUserMessage msg, int seconds)
        {
            await Task.Delay(seconds * 1000);
            try { await msg.DeleteAsync().ConfigureAwait(false); }
            catch { }
            return msg;
        }

        public static async Task<IMessage> DeleteAfterShort(this IUserMessage msg)
        {
            await Task.Delay(10000);
            try { await msg.DeleteAsync().ConfigureAwait(false); }
            catch { }
            return msg;
        }

        public static async Task<IMessage> DeleteAfterLong(this IUserMessage msg)
        {
            await Task.Delay(60000);
            try { await msg.DeleteAsync().ConfigureAwait(false); }
            catch { }
            return msg;
        }

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var json = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
