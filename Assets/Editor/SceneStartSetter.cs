using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class StartSceneToggle
{
    private const string MenuPath = "Tools/Boot from Menu";
    private const string ScenePath = "Assets/Scenes/MainMenu.unity";
    [MenuItem(MenuPath)]
    private static void Toggle()
    {
        if (EditorSceneManager.playModeStartScene == null)
        {
            EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath);
            Debug.Log("Start Scene Override: ON");
        }
        else
        {
            EditorSceneManager.playModeStartScene = null;
            Debug.Log("Start Scene Override: OFF");
        }
        
    }
    [MenuItem(MenuPath, true)]
    private static bool ToggleValidate()
    {
        Menu.SetChecked(MenuPath, EditorSceneManager.playModeStartScene != null);
        return true;
    }
}
