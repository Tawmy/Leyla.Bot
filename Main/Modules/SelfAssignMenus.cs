using Common.Classes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Main.Commands.SelfAssignMenus;

namespace Main.Modules;

[SlashCommandGroup("Menu", "Description TODO")]
[SlashRequireGuild]
internal sealed class SelfAssignMenus : ApplicationCommandLogModule
{
    [SlashCommand("create", "Create a new self assign menu.")]
    public async Task SlashCreateAsync(InteractionContext ctx,
        [Option("Title", "Title of the self assign menu to create")]
        string title,
        [Option("Description", "Optional description of the menu")]
        string? description = null)
    {
        await new Create(ctx, title, description).RunAsync();
    }

    [SlashCommand("list", "List all self assign menus.")]
    public async Task SlashListAsync(InteractionContext ctx)
    {
        await new List(ctx).RunAsync();
    }

    [SlashCommand("rename", "Renames a self assign menu.")]
    public async Task SlashRenameAsync(InteractionContext ctx,
        [Option("Title", "Title of the self assign menu to rename")]
        string title)
    {
        await new Rename(ctx, title).RunAsync();
    }

    [SlashCommand("manage", "Manages a self assign menu.")]
    public async Task SlashManageAsync(InteractionContext ctx,
        [Option("Title", "Title of the self assign menu to manage")]
        string title)
    {
        await new Manage(ctx, title).RunAsync();
    }

    [SlashCommand("delete", "Deletes a self assign menu. This is irreversible!")]
    public async Task SlashDeleteAsync(InteractionContext ctx,
        [Option("Title", "Title of the self assign menu to delete. This is irreversible!")]
        string title)
    {
        await new Delete(ctx, title).RunAsync();
    }

    [SlashCommand("post", "Posts a self assign menu in given channel.")]
    public async Task SlashPostAsync(InteractionContext ctx,
        [Option("Title", "Title of the self assign menu to post.")]
        string title,
        [Option("Channel", "Channel to post self assign menu in.")]
        DiscordChannel channel)
    {
        await new Post(ctx, channel, title).RunAsync();
    }
}