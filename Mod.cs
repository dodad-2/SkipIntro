using MelonLoader;
using SkipIntro;
using UnityEngine.SceneManagement;

[assembly: MelonInfo(typeof(Mod), "SkipIntro", "0.0.1", "dodad")]
[assembly: MelonGame("Bohemia Interactive", "Silica")]
[assembly: MelonOptionalDependencies("QList")]

namespace SkipIntro;

public class Mod : MelonMod
{
    public static Mod? instance;
    internal static Action<Scene, Scene>? activeSceneChangedDelegate;
    internal static MelonPreferences_Entry<bool>? skipIntroEntry;

    private bool QListPresent() => RegisteredMelons.Any(m => m.Info.Name == "QList");

    public override void OnInitializeMelon()
    {
        instance = this;

        PreferencesConfig.SetFilePath(this);

        var skipIntroCategory = MelonPreferences.CreateCategory("Skip Intro");
        skipIntroCategory.SetFilePath(PreferencesConfig.filePath);

        skipIntroEntry = MelonPreferences.CreateEntry<bool>(skipIntroCategory.Identifier, "PLAY_INTRO", false, "Play intro");

        activeSceneChangedDelegate = new Action<Scene, Scene>(ActiveSceneChanged);
        SceneManager.activeSceneChanged += activeSceneChangedDelegate;

        if (QListPresent())
        {
            QList.Options.RegisterMod(this);
            //QList.Options.RegisterMod(Mod.instance);

            //var skipIntroOption = new QList.OptionTypes.BoolOption(skipIntroEntry);

            //QList.Options.AddOption(skipIntroOption);
        }
    }

    private static void ActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        if (skipIntroEntry != null && skipIntroEntry.Value && SceneManager.GetActiveScene().name.Equals("Intro"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
