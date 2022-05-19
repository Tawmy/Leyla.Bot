using Common.Classes;
using Db.Helper;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using Main.Events;
using Main.Modules;

namespace Main;

public class Bot : Leyla
{
    protected override DiscordClient InitBot()
    {
        var client = new DiscordClient(new DiscordConfiguration
        {
            Token = Environment.GetEnvironmentVariable("TOKEN"),
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.Guilds
        });
        client.GuildDownloadCompleted += ClientOnGuildDownloadCompleted;
        client.ComponentInteractionCreated +=
            ClientOnComponentInteractionCreatedEvent.ClientOnComponentInteractionCreated;
        client.ModalSubmitted += ClientOnModalSubmittedEvent.ClientOnModalSubmitted;
        return client;
    }

    protected override void RegisterCommands()
    {
        var commands = Client.UseSlashCommands();

        commands.RegisterCommands<Configuration>();
        commands.RegisterCommands<Moderation>();
        commands.RegisterCommands<UserLogs>();
        commands.RegisterCommands<Quotes>();
        commands.RegisterCommands<Stashes>();
        commands.RegisterCommands<AniList>();

        commands.SlashCommandErrored += CommandsOnSlashCommandErroredEvent.CommandsOnSlashCommandErrored;
        commands.ContextMenuErrored += CommandsOnContextMenuErroredEvent.CommandsOnContextMenuErrored;
    }

    protected override async Task LoadConfig()
    {
        await ConfigHelper.Instance.Initialise();
    }
}