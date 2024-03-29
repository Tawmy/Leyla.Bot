using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Common.Classes;
using Common.Enums;
using Common.Extensions;
using Common.GuildConfig;
using Common.Helper;
using Common.Interfaces;
using Common.Records;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Main.Enums;

namespace Main.Handler;

internal sealed class ConfigurationOptionValueGivenHandler : ModalHandler
{
    private readonly string _optionId;

    public ConfigurationOptionValueGivenHandler(DiscordClient sender, ModalSubmitEventArgs e, string optionId) :
        base(sender, e)
    {
        _optionId = optionId;
    }

    public override async Task RunAsync()
    {
        var option = GuildConfigOptions.Instance.Get(Convert.ToInt32(_optionId));
        var value = EventArgs.Values.First(x => x.Key.Equals("value")).Value;

        switch (option.ConfigType)
        {
            case ConfigType.String:
                await GuildConfigHelper.Instance.SetAsync(option, EventArgs.Interaction.Guild.Id, value);
                break;
            case ConfigType.Int:
                if (!int.TryParse(value, out var valueInt))
                {
                    await ShowErrorAsync(option);
                    return;
                }

                await GuildConfigHelper.Instance.SetAsync(option, EventArgs.Interaction.Guild.Id, valueInt);
                break;
            case ConfigType.Char:
                var valueChar = Convert.ToChar(value);
                await GuildConfigHelper.Instance.SetAsync(option, EventArgs.Interaction.Guild.Id, valueChar);
                break;
            case ConfigType.Decimal:
                if (!decimal.TryParse(value, CultureInfo.InvariantCulture, out var valueDecimal))
                {
                    await ShowErrorAsync(option);
                    return;
                }

                await GuildConfigHelper.Instance.SetAsync(option, EventArgs.Interaction.Guild.Id, valueDecimal);
                break;
            case ConfigType.Boolean:
            case ConfigType.Role:
            case ConfigType.Channel:
            case ConfigType.Enum:
            default:
                throw new ArgumentOutOfRangeException(nameof(option.ConfigType));
        }

        var embed = await CreateEmbedAsync(option);
        await EventArgs.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().AddEmbed(embed).AsEphemeral());
    }

    private async Task<DiscordEmbed> CreateEmbedAsync(ConfigOption option)
    {
        var embed = new DiscordEmbedBuilder();
        embed.WithTitle("Value edited");
        embed.WithDescription($"The value for {option.Name} has been edited.");
        embed.AddField("New value",
            await GuildConfigHelper.Instance.GetDisplayStringForCurrentValueAsync(option, EventArgs.Interaction.Guild,
                true));
        return embed.Build();
    }

    private async Task ShowErrorAsync(ConfigOption option)
    {
        var embed = new DiscordEmbedBuilder();
        embed.WithColor(DiscordColor.Red);

        embed.WithTitle("Invalid input");
        embed.WithDescription("The input was in a wrong format.");
        var typeDisplayAttr = option.ConfigType.GetAttribute<DisplayAttribute>() ??
                              throw new NullReferenceException("DisplayAttribute for ConfigType must not be null");

        embed.AddField(typeDisplayAttr.Name, typeDisplayAttr.Description);

        var button = CreateButton(option);

        await EventArgs.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().AddEmbed(embed.Build()).AddComponents(button).AsEphemeral());
    }

    private DiscordButtonComponent CreateButton(IIdentifiable option)
    {
        var customId =
            ModalHelper.GetModalName(EventArgs.Interaction.User.Id, "configOptionAction",
                new[] {ConfigurationAction.Edit.ToString(), option.Id.ToString()});
        return new DiscordButtonComponent(ButtonStyle.Primary, customId, "Reopen modal");
    }
}