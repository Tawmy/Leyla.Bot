using Common.Handler;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;

namespace Common.Events;

public static class CommandsOnContextMenuErroredEvent
{
    public static async Task CommandsOnContextMenuErroredAsync(SlashCommandsExtension sender,
        ContextMenuErrorEventArgs e)
    {
        if (e.Exception is ContextMenuExecutionChecksFailedException ex)
        {
            await new ContextMenuExecutionChecksFailedExceptionHandler(e, ex).HandleExceptionAsync();
        }
    }
}