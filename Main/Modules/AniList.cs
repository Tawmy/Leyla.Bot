using Common.Classes;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Main.Commands.AniList;

namespace Main.Modules;

[SlashCommandGroup("AniList", "Get various data from AniList: Anime TODO")]
internal sealed class AniList : ApplicationCommandLogModule
{
    [SlashCommand("anime", "Shows information for the given anime.")]
    [SlashRequireBotPermissions(Permissions.SendMessages | Permissions.EmbedLinks)]
    public async Task SlashAnimeAsync(InteractionContext ctx,
        [Option("Title", "Title of the anime to search for")]
        string title)
    {
        await new Anime(ctx, title).RunAsync();
    }

    [SlashCommand("manga", "Shows information for the given manga.")]
    [SlashRequireBotPermissions(Permissions.SendMessages | Permissions.EmbedLinks)]
    public async Task SlashMangaAsync(InteractionContext ctx,
        [Option("Title", "Title of the manga to search for")]
        string title)
    {
        await new Manga(ctx, title).RunAsync();
    }

    [SlashCommand("character", "Shows information for the given character.")]
    [SlashRequireBotPermissions(Permissions.SendMessages | Permissions.EmbedLinks)]
    public async Task SlashCharacterAsync(InteractionContext ctx,
        [Option("Name", "Name of the character to search for")]
        string name)
    {
        await new Character(ctx, name).RunAsync();
    }
}