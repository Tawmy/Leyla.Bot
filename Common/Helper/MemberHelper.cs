using Common.Db;
using Common.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Helper;

public static class MemberHelper
{
    public static async Task CreateIfNotExist(ulong memberId, ulong guildId)
    {
        await GuildHelper.CreateIfNotExist(guildId);

        await using var context = new DatabaseContext();
        if (!await context.Members.AnyAsync(x => x.Id == memberId && x.GuildId == guildId))
        {
            await context.Members.AddAsync(new Member
            {
                Id = memberId,
                GuildId = guildId
            });
            await context.SaveChangesAsync();
        }
    }
}