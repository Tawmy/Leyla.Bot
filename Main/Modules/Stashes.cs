using Common.Classes;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Main.Commands.Stashes;

namespace Main.Modules;

[SlashCommandGroup("Stash", "Description TODO")]
[SlashRequireGuild]
internal sealed class Stashes : ApplicationCommandLogModule
{
    [SlashCommand("list", "List all entries in stash.")]
    [SlashRequireBotPermissions(Permissions.SendMessages | Permissions.EmbedLinks)]
    public async Task SlashListAsync(InteractionContext ctx,
        [Option("Name", "Name of the stash list entries of")]
        string? title = null)
    {
        // TODO make name optional, show menu with list of stashes if not provided
        await new List(ctx, title).RunAsync();
    }

    [SlashCommand("pick", "Pick a random entry.")]
    [SlashRequireBotPermissions(Permissions.SendMessages)]
    public async Task SlashPickAsync(InteractionContext ctx,
        [Option("Name", "Name of the stash pick from. Picks from all stashes if not provided.")]
        string? title = null)
    {
        await new Pick(ctx, title).RunAsync();
    }

    [SlashCommand("show", "Shows a specific stash entry.")]
    [SlashRequireBotPermissions(Permissions.SendMessages)]
    public async Task SlashShowEntryAsync(InteractionContext ctx,
        [Option("Name", "Name of the stash to show entry from.")]
        string title,
        [Option("n", "Number of the entry to show. You can find this using /stash list.")]
        long n)
    {
        await new ShowEntry(ctx, title, n).RunAsync();
    }
}

[SlashCommandGroup("StashM", "Description TODO")]
[SlashRequireGuild]
internal sealed class StashesM : ApplicationCommandLogModule
{
    [ContextMenu(ApplicationCommandType.MessageContextMenu, "Add to Stash")]
    public async Task MenuAddToAsync(ContextMenuContext ctx)
    {
        await new AddTo(ctx).RunAsync();
    }

    [SlashCommand("create", "Create a new stash.")]
    public async Task SlashCreateAsync(InteractionContext ctx,
        [Option("Name", "Name of the stash to create")]
        string title)
    {
        await new Create(ctx, title).RunAsync();
    }

    [SlashCommand("remove", "Removes entry from a stash.")]
    public async Task SlashRemoveFromAsync(InteractionContext ctx,
        [Option("Name", "Name of the stash to delete entry from.")]
        string title,
        [Option("value",
            "This can either be the content itself or its index. You can find the index using /stash list.")]
        string value)
    {
        await new RemoveFrom(ctx, title, value).RunAsync();
    }

    [SlashCommand("delete", "Deletes a stash. This is irreversible!")]
    public async Task SlashDeleteAsync(InteractionContext ctx,
        [Option("Name", "Name of the stash to delete. This is irreversible!")]
        string title)
    {
        await new Delete(ctx, title).RunAsync();
    }
}