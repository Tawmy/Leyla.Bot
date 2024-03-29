using System.Text;
using Common.Classes;
using Common.Db.Models;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;

namespace Main.Commands.Quotes;

internal sealed class List : SlashCommand
{
    private readonly DiscordMember _member;

    public List(InteractionContext ctx, DiscordMember member) : base(ctx)
    {
        _member = member;
    }

    public override async Task RunAsync()
    {
        var quotes = await GetQuotesForMemberAsync(Ctx.Guild.Id, _member.Id);

        var b = new StringBuilder();
        for (var i = 0; i < quotes.Count; i++)
        {
            b.Append($"**{i + 1}.** {quotes[i].Text}{Environment.NewLine}");
        }

        var quotesStr = quotes.Count > 0
            ? b.ToString()
            : "No quotes";

        var embed = new DiscordEmbedBuilder();
        embed.WithTitle($"Quotes for {_member.DisplayName}");
        embed.WithDescription(quotesStr);
        embed.WithColor(DiscordColor.Blurple);

        await Ctx.CreateResponseAsync(new DiscordInteractionResponseBuilder().AddEmbed(embed.Build()).AsEphemeral());
    }

    #region Instance methods

    private async Task<List<Quote>> GetQuotesForMemberAsync(ulong guildId, ulong userId)
    {
        return await DbCtx.Quotes.Where(x =>
                x.GuildId == guildId &&
                x.UserId == userId)
            .ToListAsync();
    }

    #endregion
}