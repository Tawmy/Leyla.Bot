using Main.Enums.CharacterSheet;
using Nito.AsyncEx;
using SixLabors.Fonts;
using xivapi_cs.Enums;
using Font = Main.Enums.CharacterSheet.Font;

namespace Main.Helper;

public sealed class ResourceHelper
{
    public static AsyncLazy<ResourceHelper> Instance { get; } = new(CreateAndLoadDataAsync);

    private IFontCollection FontCollection { get; } = new FontCollection();

    private IDictionary<Image, SixLabors.ImageSharp.Image> Images { get; } =
        new Dictionary<Image, SixLabors.ImageSharp.Image>();

    private IDictionary<Job, SixLabors.ImageSharp.Image> JobIcons { get; } =
        new Dictionary<Job, SixLabors.ImageSharp.Image>();

    #region Initialisation

    private ResourceHelper()
    {
    }

    private static async Task<ResourceHelper> CreateAndLoadDataAsync()
    {
        var ret = new ResourceHelper();
        await ret.InitializeAsync();
        return ret;
    }

    private async Task InitializeAsync()
    {
        await LoadImagesAsync();
        await LoadJobIconsAsync();
        LoadFonts();
    }

    private async Task LoadImagesAsync()
    {
        Images.Add(Image.TemplateBase,
            await SixLabors.ImageSharp.Image.LoadAsync("Resources/characterTemplateBase.png"));
        Images.Add(Image.TemplateFrame,
            await SixLabors.ImageSharp.Image.LoadAsync("Resources/characterTemplateFrame.png"));
        Images.Add(Image.TemplateJobCircle,
            await SixLabors.ImageSharp.Image.LoadAsync("Resources/characterTemplateJob.png"));
        Images.Add(Image.GcMaelstrom,
            await SixLabors.ImageSharp.Image.LoadAsync("Resources/chat_messengericon_town01.png"));
        Images.Add(Image.GcTwinAdder,
            await SixLabors.ImageSharp.Image.LoadAsync("Resources/chat_messengericon_town02.png"));
        Images.Add(Image.GcImmortalFlames,
            await SixLabors.ImageSharp.Image.LoadAsync("Resources/chat_messengericon_town03.png"));
    }

    private async Task LoadJobIconsAsync()
    {
        foreach (var job in (Job[]) Enum.GetValues(typeof(Job)))
        {
            JobIcons.Add(job,
                await SixLabors.ImageSharp.Image.LoadAsync($"Resources/Jobs/{job.ToString().ToLower()}.png"));
        }
    }

    private void LoadFonts()
    {
        FontCollection.Add("Resources/OpenSans-VariableFont_wdth,wght.ttf");
        FontCollection.Add("Resources/Vollkorn-VariableFont_wght.ttf");
        FontCollection.Add("Resources/Antonio-ExtraLight.ttf");
    }

    #endregion

    #region Public methods

    public FontFamily GetFontFamily(Font font)
    {
        return font switch
        {
            Font.OpenSans => FontCollection.Get("Open Sans"),
            Font.Vollkorn => FontCollection.Get("Vollkorn"),
            Font.Antonio => FontCollection.Get("Antonio ExtraLight"),
            _ => throw new ArgumentOutOfRangeException(nameof(font), font, "Font not in collection")
        };
    }

    public SixLabors.ImageSharp.Image GetImage(Image image)
    {
        return Images[image];
    }

    public SixLabors.ImageSharp.Image GetJobIcon(Job job)
    {
        return JobIcons[job];
    }

    #endregion
}