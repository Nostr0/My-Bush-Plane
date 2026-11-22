using UnityEditor;
using UnityEditor.SceneManagement;

namespace MaintGames.Bush.Editor
{
    /// <summary>Menu entries that open the testbed scenes in Assets/_Project/Scenes/Testbeds.</summary>
    public static class TestbedMenu
    {
        private const string PlaygroundScenePath = "Assets/_Project/Scenes/Testbeds/Playground.unity";

        [MenuItem("Bush/Testbeds/Open Playground")]
        private static void OpenPlayground()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(PlaygroundScenePath, OpenSceneMode.Single);
            }
        }
    }
}
