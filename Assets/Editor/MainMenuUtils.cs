using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MainMenuUtils
{
    [MenuItem("GGJ2019/Play Game")]
    private static void NewMenuOption()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/0.MainScene.unity");
            EditorApplication.isPlaying = true;
        }
    }
    [MenuItem("GGJ2019/Reset Best Scores")]
    private static void ResetBestScores()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Best Scores Deleted");
    }
}
