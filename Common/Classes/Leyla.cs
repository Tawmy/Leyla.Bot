using Common.Events;
using Common.Helper;
using Common.Interfaces;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;

namespace Common.Classes;

public abstract class Leyla : IBot
{
    private DiscordClient? Client { get; set; }

    public async Task StartAsync()
    {
        Client = await InitBot();
        RegisterCommands();
        RegisterInteractivity();
        await Client.ConnectAsync();
    }

    public virtual async Task StopAsync()
    {
        if (Client != null)
        {
            await Client.DisconnectAsync();
        }
    }

    public void Dispose()
    {
        Client?.Dispose();
    }

    protected virtual SlashCommandsExtension RegisterCommands()
    {
        var commands = Client.UseSlashCommands();
        commands.SlashCommandErrored += CommandsOnSlashCommandErroredEvent.CommandsOnSlashCommandErrored;
        commands.ContextMenuErrored += CommandsOnContextMenuErroredEvent.CommandsOnContextMenuErrored;
        return commands;
    }

    private void RegisterInteractivity()
    {
        Client.UseInteractivity(new InteractivityConfiguration
        {
            Timeout = new TimeSpan(0, 0, 0, 30)
        });
    }

    protected async Task ClientOnGuildDownloadCompleted(DiscordClient sender, GuildDownloadCompletedEventArgs e)
    {
        if (Client != null)
        {
            await new StartupHelper(Client).SendStartupMessage();
        }
    }

    #region Abstract methods

    protected abstract Task<DiscordClient> InitBot();

    #endregion
}