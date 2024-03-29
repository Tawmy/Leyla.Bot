using Common.Classes;
using Common.Db;
using Common.Db.Models;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Main.Handler;

internal sealed class StashEntryValueGivenHandler : ModalHandler
{
    private readonly string _stashIds;

    public StashEntryValueGivenHandler(DiscordClient sender, ModalSubmitEventArgs eventArgs, string stashIds) : base(
        sender, eventArgs)
    {
        _stashIds = stashIds;
    }

    public override async Task RunAsync()
    {
        await AddToDatabaseAsync(EventArgs.Values["value"]);
        await EventArgs.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
    }

    private async Task AddToDatabaseAsync(string value)
    {
        // this is so wack i cannot even
        var stashIds = _stashIds.Split(",").Select(x => Convert.ToInt32(x)); // skipcq: CS-R1068

        await using var context = new DatabaseContext();

        foreach (var stashId in stashIds)
        {
            await context.StashEntries.AddAsync(new StashEntry
            {
                Value = value,
                StashId = stashId
            });
        }

        await context.SaveChangesAsync();
    }
}