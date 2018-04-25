using System;

namespace InjhinuityDiscordClient.Enums
{
    [Flags]
    public enum EventType
    {
        // safety net if the bitfield is 0
        None = 0,

        // DiscordSocketUser events
        UserJoined = 1 << 0,        // 1
        UserLeft = 1 << 1,          // 2
        UserBanned = 1 << 2,        // 4
        UserUnbanned = 1 << 3,      // 8

        MessageReceived = 1 << 4,   // 16
        MessageDeleted = 1 << 5,    // 32
        MessageUpdated = 1 << 6,    // 64

        // on GuildMemberUpdated, confirm if GuildMember changed nickname
        NicknameUpdated = 1 << 7,   // 128

        // on UserUpdated, resolve for type of update
        UsernameUpdated = 1 << 8,   // 256
        UserAvatarUpdated = 1 << 9, // 512
    }
}
