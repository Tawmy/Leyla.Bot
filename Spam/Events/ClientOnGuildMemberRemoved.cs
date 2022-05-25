using Common.Helper;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Spam.Events;

public static class ClientOnGuildMemberRemoved
{
    public static async Task HandleEvent(DiscordClient sender, GuildMemberRemoveEventArgs e)
    {
        await SilenceHelper.Instance.ProcessUserLeft(e.Guild, e.Member);
    }
}