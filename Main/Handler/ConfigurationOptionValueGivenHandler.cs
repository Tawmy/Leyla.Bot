using Common.Classes;
using Common.Enums;
using Common.Helper;
using Common.Statics;
using DSharpPlus;
using DSharpPlus.EventArgs;

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
        var option = ConfigOptions.Instance.Get(Convert.ToInt32(_optionId));
        var value = EventArgs.Values.First(x => x.Key.Equals("value")).Value;

        switch (option.ConfigType)
        {
            case ConfigType.String:
                await ConfigHelper.Instance.Set(option, EventArgs.Interaction.Guild.Id, value);
                break;
            case ConfigType.Int:
                var valueInt = Convert.ToInt32(value);
                await ConfigHelper.Instance.Set(option, EventArgs.Interaction.Guild.Id, valueInt);
                break;
            case ConfigType.Char:
                var valueChar = Convert.ToChar(value);
                await ConfigHelper.Instance.Set(option, EventArgs.Interaction.Guild.Id, valueChar);
                break;
            case ConfigType.Decimal:
                var valueDecimal = Convert.ToDecimal(value);
                await ConfigHelper.Instance.Set(option, EventArgs.Interaction.Guild.Id, valueDecimal);
                break;
            case ConfigType.Boolean:
            case ConfigType.Role:
            case ConfigType.Channel:
            case ConfigType.Enum:
            default:
                throw new ArgumentOutOfRangeException();
        }

        await EventArgs.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
    }
}