using UnityEditor;
using UnityEditor.SceneManagement;

namespace MaintGames.Bush.Editor
{
    /// <summary>
    /// Optional menu toggle: Play Mode always starts from Bootstrap.unity, whichever scene is open.
    /// Off by default so testbed scenes can still be played directly.
    /// </summary>
    [InitializeOnLoad]
    public static class BootstrapPlayModeStart
    {
        private const string BootstrapScenePath = "Assets/_Project/Scenes/Bootstrap.unity";
        private const string MenuPath = "Bush/Play From Bootstrap Scene";
        private const string PreferenceKey = "MaintGames.Bush.PlayFromBootstrap";

        static BootstrapPlayModeStart()
        {
            // The AssetDatabase is not ready yet while the editor is still loading.
            EditorApplication.delayCall += Apply;
        }

        [MenuItem(MenuPath)]
        private static void Toggle()
        {
            EditorPrefs.SetBool(PreferenceKey, !IsEnabled());
            Apply();
        }

        [MenuItem(MenuPath, true)]
        private static bool ValidateToggle()
        {
            Menu.SetChecked(MenuPath, IsEnabled());
            return true;
        }

        private static bool IsEnabled()
        {
            return EditorPrefs.GetBool(PreferenceKey, false);
        }

        private static void Apply()
        {
            EditorSceneManager.playModeStartScene = IsEnabled()
                ? AssetDatabase.LoadAssetAtPath<SceneAsset>(BootstrapScenePath)
                : null;
        }
    }
}
