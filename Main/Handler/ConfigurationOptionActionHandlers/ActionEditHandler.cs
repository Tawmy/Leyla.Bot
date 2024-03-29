using System.ComponentModel.DataAnnotations;
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

namespace Main.Handler.ConfigurationOptionActionHandlers;

public class ActionEditHandler : InteractionHandler
{
    private readonly string _optionId;

    public ActionEditHandler(DiscordClient sender, ComponentInteractionCreateEventArgs e, string optionId) :
        base(sender, e)
    {
        _optionId = optionId;
    }

    public override async Task RunAsync()
    {
        var optionId = Convert.ToInt32(_optionId);
        var option = GuildConfigOptions.Instance.Get(optionId);
        var optionDetailsEmbed = GetOptionDetailsEmbed(option);

        switch (option.ConfigType)
        {
            case ConfigType.String:
            case ConfigType.Int:
            case ConfigType.Char:
            case ConfigType.Decimal:
                var modalBuilder = await GetModalAsync(option);
                await EventArgs.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modalBuilder);
                break;
            case ConfigType.Boolean:
                var boolSelect = await GetBoolSelectAsync(option);
                await EventArgs.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(optionDetailsEmbed).AddComponents(boolSelect)
                        .AsEphemeral());
                break;
            case ConfigType.Role:
                var roleSelect = await GetRoleSelectAsync(option);
                var currentRole = await GuildConfigHelper.Instance.GetRoleAsync(option.Name, EventArgs.Guild);
                if (currentRole != null)
                {
                    // Workaround until Discord adds support for default values to these select types
                    optionDetailsEmbed = new DiscordEmbedBuilder(optionDetailsEmbed)
                        .AddField("Current Value", currentRole.Mention)
                        .Build();
                }

                await EventArgs.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(optionDetailsEmbed).AddComponents(roleSelect)
                        .AsEphemeral());
                break;
            case ConfigType.Channel:
                var channelSelect = await GetTextChannelSelectAsync(option);
                var currentChannel = await GuildConfigHelper.Instance.GetChannelAsync(option.Name, EventArgs.Guild);
                if (currentChannel != null)
                {
                    // Workaround until Discord adds support for default values to these select types
                    optionDetailsEmbed = new DiscordEmbedBuilder(optionDetailsEmbed)
                        .AddField("Current Value", currentChannel.Mention)
                        .Build();
                }

                await EventArgs.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(optionDetailsEmbed).AddComponents(channelSelect)
                        .AsEphemeral());
                break;
            case ConfigType.Enum:
                var enumSelect = await GetEnumSelectAsync(option);
                await EventArgs.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(optionDetailsEmbed).AddComponents(enumSelect)
                        .AsEphemeral());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(option.ConfigType));
        }
    }

    private static DiscordEmbed GetOptionDetailsEmbed(ConfigOption option)
    {
        var embed = new DiscordEmbedBuilder();
        embed.WithTitle(option.Name);
        embed.WithDescription(option.ConfigStrings.Description);
        embed.WithColor(DiscordColor.Blurple);
        return embed.Build();
    }

    private Task<DiscordRoleSelectComponent> GetRoleSelectAsync(IIdentifiable option)
    {
        // TODO support clearing config by selecting none
        var customId =
            ModalHelper.GetModalName(EventArgs.User.Id, "configOptionValueSelected", new[] {option.Id.ToString()});
        return Task.FromResult(new DiscordRoleSelectComponent(customId, "Select role",
            minOptions: 1, maxOptions: 1));
    }

    private Task<DiscordChannelSelectComponent> GetTextChannelSelectAsync(IIdentifiable option)
    {
        var customId =
            ModalHelper.GetModalName(EventArgs.User.Id, "configOptionValueSelected", new[] {option.Id.ToString()});
        return Task.FromResult(new DiscordChannelSelectComponent(customId, "Select channel",
            minOptions: 1, maxOptions: 1, channelTypes: new[] {ChannelType.Text}));
    }

    private async Task<DiscordSelectComponent> GetBoolSelectAsync(IIdentifiable option)
    {
        var currentConfig = await GuildConfigHelper.Instance.GetBoolAsync(option.Name, EventArgs.Guild.Id);
        var options = new List<DiscordSelectComponentOption>
            {new("On", "1", isDefault: currentConfig == true), new("Off", "0", isDefault: currentConfig != true)};
        var customId =
            ModalHelper.GetModalName(EventArgs.User.Id, "configOptionValueSelected", new[] {option.Id.ToString()});
        return new DiscordSelectComponent(customId, "Select value", options, minOptions: 1, maxOptions: 1);
    }

    private async Task<DiscordSelectComponent> GetEnumSelectAsync(ConfigOption option)
    {
        if (option.EnumType == null)
        {
            throw new ArgumentNullException(nameof(option), nameof(option.EnumType));
        }

        var currentConfig = await GuildConfigHelper.Instance.GetStringAsync(option.Name, EventArgs.Guild.Id);

        var options = new List<DiscordSelectComponentOption>();

        // there's gotta be a better way to do this
        foreach (var entry in Enum.GetNames(option.EnumType))
        {
            // get name and index of each enum member
            var obj = Enum.Parse(option.EnumType, entry);
            var index = (int) obj;
            var displayName = obj.GetAttribute<DisplayAttribute>()?.Name ?? obj.ToString();
            options.Add(new DiscordSelectComponentOption(displayName, index.ToString(),
                isDefault: index.ToString().Equals(currentConfig)));
        }

        var customId =
            ModalHelper.GetModalName(EventArgs.User.Id, "configOptionValueSelected", new[] {option.Id.ToString()});
        return new DiscordSelectComponent(customId, "Select value", options, minOptions: 1, maxOptions: 1);
    }

    private async Task<DiscordInteractionResponseBuilder> GetModalAsync(ConfigOption option)
    {
        var response = new DiscordInteractionResponseBuilder();
        response.WithTitle(option.Name);
        response.AddModalCustomId(EventArgs.User.Id, "configOptionValueGiven", new[] {option.Id.ToString()});

        var placeholder = option.ConfigType switch
        {
            ConfigType.String => "Enter text",
            ConfigType.Char => "Enter singular character",
            ConfigType.Int => "Enter digits",
            ConfigType.Decimal => "Enter decimal value (XY.ZA)",
            _ => "Enter value"
        };

        var maxLength = option.ConfigType switch
        {
            ConfigType.Char => 1,
            ConfigType.Int => 9, // 9 to avoid any value > maxInt troubles
            ConfigType.Decimal => 16,
            _ => 100
        };

        var currentConfig = await GuildConfigHelper.Instance.GetStringAsync(option.Name, EventArgs.Guild.Id) ??
                            string.Empty;

        response.AddComponents(new TextInputComponent("Value", "value", placeholder, currentConfig, min_length: 1,
            max_length: maxLength));
        return response;
    }
}