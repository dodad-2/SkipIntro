using MelonLoader;
using SkipIntro;
using UnityEngine.SceneManagement;

[assembly: MelonInfo(typeof(Mod), "Skip Intro", "0.0.1", "dodad")]
[assembly: MelonGame("Bohemia Interactive", "Silica")]

namespace SkipIntro;

public class Mod : MelonMod
{
    public static Mod? instance;
    internal static QList.OptionTypes.BoolOption? SkipIntroOption;
    internal static Action<Scene, Scene>? activeSceneChangedDelegate;

    public override void OnInitializeMelon()
    {
        instance = this;

        PreferencesConfig.SetFilePath(this);

        var skipIntroCategory = MelonPreferences.CreateCategory("Skip Intro");
        skipIntroCategory.SetFilePath(PreferencesConfig.filePath);

        var skipIntroEntry = MelonPreferences.CreateEntry<bool>(skipIntroCategory.Identifier, "PLAY_INTRO", false, "Play intro");

        QList.Options.RegisterMod(Mod.instance);

        SkipIntroOption = new QList.OptionTypes.BoolOption(skipIntroEntry);

        QList.Options.AddOption(SkipIntroOption);

        activeSceneChangedDelegate = new Action<Scene, Scene>(ActiveSceneChanged);
        SceneManager.activeSceneChanged += activeSceneChangedDelegate;
    }

    private static void ActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        if (SkipIntroOption != null && !SkipIntroOption.GetValue() && SceneManager.GetActiveScene().name.Equals("Intro"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
