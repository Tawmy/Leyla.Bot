using Db.Enums;
using Db.Statics.BaseClasses;

namespace Db.Statics;

public sealed class ConfigOptions : StaticClass<ConfigOption>
{
    private ConfigOptions() : base(new List<ConfigOption>
    {
        new(1, 1, "Moderator Role", "todo", ConfigType.Role, null, 2),
        new(2, 1, "Moderator Channel", "todo", ConfigType.Channel, null, 3),
        new(3, 2, "Log Channel", "todo", ConfigType.Channel, null, 3),
        new(4, 3, "Archive Channel", "todo", ConfigType.Channel, null, 3),
        new(5, 2, "Verification Role", "todo", ConfigType.Role, null, 2),

        new(1000, 1, "String", "default", ConfigType.String, "hi", 1000),
        new(1001, 2, "StringNull", "null", ConfigType.String, null, 1000),
        new(1002, 3, "Boolean", "default", ConfigType.Boolean, "1", 1000),
        new(1003, 4, "BooleanNull", "null", ConfigType.Boolean, null, 1000),
        new(1004, 5, "Int", "default", ConfigType.Int, "7", 1000),
        new(1005, 6, "IntNull", "null", ConfigType.Int, null, 1000),
        new(1006, 7, "Char", "default", ConfigType.Char, "h", 1000),
        new(1007, 8, "CharNull", "null", ConfigType.Char, null, 1000),
        new(1008, 9, "Role", "null", ConfigType.Role, null, 1000),
        new(1009, 10, "Channel", "null", ConfigType.Channel, null, 1000)
    })
    {
    }

    #region Singleton

    private static readonly Lazy<ConfigOptions> Lazy = new(() => new ConfigOptions());
    public static ConfigOptions Instance => Lazy.Value;

    #endregion
}

public class ConfigOption : StaticField
{
    public readonly int ConfigOptionCategoryId;
    public readonly string? DefaultValue;
    public readonly string Description;
    public readonly ConfigType Type;

    public ConfigOption(int i, int s, string n, string d, ConfigType t, string? df, int ci) : base(i, s, n)
    {
        Description = d;
        Type = t;
        DefaultValue = df;
        ConfigOptionCategoryId = ci;
    }

    public ConfigOptionCategory ConfigOptionCategory => ConfigOptionCategories.Instance.Get(ConfigOptionCategoryId);
}