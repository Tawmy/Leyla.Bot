namespace Common.Db.Models;

public class Member
{
    public ulong UserId { get; set; }

    public ulong GuildId { get; set; }
    public Guild Guild { get; set; } = null!;

    public ICollection<CommandLog> CommandLogs { get; set; } = null!;
    public ICollection<Quote> Quotes { get; set; } = null!;
    public ICollection<UserLog> AuthorUserLogs { get; set; } = null!;
    public ICollection<UserLog> TargetUserLogs { get; set; } = null!;
}