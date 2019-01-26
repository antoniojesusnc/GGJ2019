using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIMainMenu : MonoBehaviour
{
    void Start()
    {
        
    }

    public void LoadLevel(int levelNumber)
    {
        if (!GameManager.Instance.IsLevelUnLock(levelNumber)){
            // Level lock
            return;
        }

        GameManager.Instance.CurrentLevel = levelNumber;
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelNumber);
    }
}
