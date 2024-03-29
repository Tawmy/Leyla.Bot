using Common.Classes;
using Common.Db.Models;
using Common.Extensions;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;

namespace Main.Commands.Quotes;

internal sealed class Random : SlashCommand
{
    public Random(InteractionContext ctx) : base(ctx)
    {
    }

    public override async Task RunAsync()
    {
        var quote = await GetRandomQuoteAsync(Ctx.Guild.Id);
        if (quote == null)
        {
            await Ctx.CreateResponseAsync(new DiscordInteractionResponseBuilder().AddErrorEmbed("No quotes found."));
        }

        var member = await Ctx.GetMemberAsync(quote!.UserId);
        if (member == null)
        {
            await Ctx.CreateResponseAsync(new DiscordInteractionResponseBuilder().AddErrorEmbed("Member not found."));
        }

        var embed = GetQuoteEmbed(member!.DisplayName, quote);
        await Ctx.CreateResponseAsync(new DiscordInteractionResponseBuilder().AddEmbed(embed));
    }

    #region Instance methods

    private async Task<Quote?> GetRandomQuoteAsync(ulong guildId)
    {
        return await DbCtx.Quotes.Where(x =>
                x.Member.GuildId == guildId)
            .OrderBy(x => Guid.NewGuid())
            .FirstOrDefaultAsync();
    }

    #endregion

    #region Static methods

    private static DiscordEmbed GetQuoteEmbed(string displayName, Quote quote)
    {
        var embed = new DiscordEmbedBuilder();
        embed.WithDescription(
            $"**\"{quote.Text}\"**{Environment.NewLine}- {displayName}, {quote.Date.Year}");
        embed.WithColor(DiscordColor.Blurple);
        return embed.Build();
    }

    #endregion
}