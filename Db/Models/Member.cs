namespace Db.Models;

public class Member
{
    public ulong Id { get; set; }

    public ulong GuildId { get; set; }
    public Guild Guild { get; set; } = null!;

    public ICollection<Quote> Quotes { get; set; } = null!;
}