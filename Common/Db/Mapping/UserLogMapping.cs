using Common.Db.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.Db.Mapping;

public class UserLogMapping : IEntityTypeConfiguration<UserLog>
{
    public void Configure(EntityTypeBuilder<UserLog> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Member)
            .WithMany(x => x.TargetUserLogs)
            .HasForeignKey(x => new {x.AuthorId, x.GuildId})
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Author)
            .WithMany(x => x.AuthorUserLogs)
            .HasForeignKey(x => new {x.MemberId, x.GuildId})
            .OnDelete(DeleteBehavior.Cascade);
    }
}