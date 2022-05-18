using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;
using GraphQL.Client.Http;
using Main.Handler;

namespace Main.Events;

public static class CommandsOnSlashCommandErroredEvent
{
    public static async Task CommandsOnSlashCommandErrored(SlashCommandsExtension sender, SlashCommandErrorEventArgs e)
    {
        switch (e.Exception)
        {
            case SlashExecutionChecksFailedException ex1:
                await new SlashExecutionChecksFailedExceptionHandler(e, ex1).HandleException();
                break;
            case GraphQLHttpRequestException ex2:
                await new GraphQlHttpRequestExceptionHandler(e, ex2).HandleException();
                break;
            default:
                return;
        }
    }
}