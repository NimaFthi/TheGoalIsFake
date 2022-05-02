#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtility : MonoBehaviour
{
    [MenuItem("Utils/Play")]
    public static void PlayGame()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path);
        EditorApplication.isPlaying = true;
    }
}

#endif