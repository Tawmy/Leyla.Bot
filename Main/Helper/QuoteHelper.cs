using Common.Db;
using Common.Db.Models;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;

namespace Main.Helper;

public static class QuoteHelper
{
    public static DiscordEmbed GetQuoteEmbed(string displayName, Quote quote)
    {
        var embed = new DiscordEmbedBuilder();
        embed.WithDescription(
            $"**\"{quote.Text}\"**{Environment.NewLine}- {displayName}, {quote.Date.Year}");
        embed.WithColor(DiscordColor.Blurple);
        return embed.Build();
    }

    public static async Task<Quote?> GetQuote(ulong guildId, ulong memberId, int i)
    {
        await using var context = new DatabaseContext();

        return await context.Quotes.Where(x =>
                x.Member.GuildId == guildId &&
                x.MemberId == memberId)
            .Skip(i - 1)
            .FirstOrDefaultAsync();
    }
}