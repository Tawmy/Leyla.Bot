using Common.Classes;
using Common.Db;
using Common.Db.Models;
using Common.Extensions;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.EntityFrameworkCore;

namespace Main.Handler;

internal sealed class SelfAssignMenuRenameHandler : ModalHandler
{
    private readonly string _menuId;

    public SelfAssignMenuRenameHandler(DiscordClient sender, ModalSubmitEventArgs eventArgs, string menuId) : base(
        sender, eventArgs)
    {
        _menuId = menuId;
    }

    public override async Task RunAsync()
    {
        await using var context = new DatabaseContext();

        var menu = await GetSelfAssignMenuAsync(context);

        if (menu == null)
        {
            await EventArgs.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddErrorEmbed("Self assign menu not found.").AsEphemeral());
            return;
        }

        var title = EventArgs.Values["title"] ?? string.Empty;

        if (await context.SelfAssignMenus.AnyAsync(x =>
                x.GuildId == menu.GuildId && x.Title.Equals(title) && x.Id != menu.Id))
        {
            await EventArgs.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AddErrorEmbed($"A Self Assign Menu with that title ({title}) already exists.").AsEphemeral());
            return;
        }

        var description = EventArgs.Values["description"];
        await EditInDatabaseAsync(context, menu, title, description);
        await EventArgs.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
    }

    private async Task<SelfAssignMenu?> GetSelfAssignMenuAsync(DatabaseContext context)
    {
        var id = Convert.ToInt32(_menuId);
        return await context.SelfAssignMenus.FirstOrDefaultAsync(x => x.Id == id);
    }

    private static async Task EditInDatabaseAsync(DbContext context, SelfAssignMenu menu, string title,
        string? description)
    {
        menu.Title = title;
        menu.Description = description;
        context.Entry(menu).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }
}