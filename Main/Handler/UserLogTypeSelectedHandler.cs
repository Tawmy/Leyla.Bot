using Common.Classes;
using Common.Extensions;
using Db.Enums;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Main.Handler;

public class UserLogTypeSelectedHandler : InteractionHandler
{
    private readonly ulong _userId;
    
    public UserLogTypeSelectedHandler(DiscordClient sender, ComponentInteractionCreateEventArgs e, ulong userId) : base(sender, e)
    {
        _userId = userId;
    }

    public override async Task RunAsync()
    {
        var userLogType = (UserLogType) Convert.ToInt32(EventArgs.Values[0]);
        var member = await EventArgs.Interaction.GetMember(_userId);
        var displayName = member?.DisplayName ?? _userId.ToString();

        var modal = GetModal(userLogType, displayName);
        await EventArgs.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
    }

    private DiscordInteractionResponseBuilder GetModal(UserLogType type, string displayName)
    {
        var response = new DiscordInteractionResponseBuilder();
        response.WithTitle($"Add {type} log for {displayName}");
        response.WithCustomId($"addUserLog-{_userId}-{(int)type}");
        response.AddComponents(new TextInputComponent("Reason", "reason", style: TextInputStyle.Paragraph,
            min_length: 1, max_length: 2000));
        return response;
    }
}