using Common.Classes;
using Common.GuildConfig;
using Common.Helper;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Main.Handler.ConfigurationOptionActionHandlers;

public class ActionResetHandler : InteractionHandler
{
    private readonly string _optionId;

    public ActionResetHandler(DiscordClient sender, ComponentInteractionCreateEventArgs e, string optionId) :
        base(sender, e)
    {
        _optionId = optionId;
    }

    public override async Task RunAsync()
    {
        var optionId = Convert.ToInt32(_optionId);
        await GuildConfigHelper.Instance.ResetAsync(optionId, EventArgs.Guild.Id);

        var embed = CreateEmbed(optionId);
        await EventArgs.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().AddEmbed(embed).AsEphemeral());
    }

    private static DiscordEmbed CreateEmbed(int optionId)
    {
        var option = GuildConfigOptions.Instance.Get(optionId);

        var embed = new DiscordEmbedBuilder();
        embed.WithTitle("Value reset");
        embed.WithDescription($"The value for {option.Name} has been reverted.");
        embed.AddField("New value", GuildConfigHelper.GetDisplayStringForDefaultValue(option, true));
        return embed.Build();
    }
}