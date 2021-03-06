namespace Common.Db.Models;

public class Config
{
    public int Id { get; set; }

    public int ConfigOptionId { get; set; } // statics

    public ulong GuildId { get; set; }
    public Guild Guild { get; set; } = null!;

    public string Value { get; set; } = null!;
}