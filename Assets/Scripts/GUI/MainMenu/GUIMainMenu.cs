using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIMainMenu : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _scenes;

    void Start()
    {
        
    }

    public void LoadLevel(int levelNumber)
    {
        if (!GameManager.Instance.IsLevelUnLock(levelNumber)){
            // Level lock
            return;
        }

        AudioManager.Instance.PlaySound(AudioManager.ClipButtonSound, Vector3.zero, false);

        GameManager.Instance.CurrentLevel = levelNumber;
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelNumber);
    }

    public void PreviewScene(int scene)
    {
        for (int i = _scenes.Count - 1; i >= 0; --i)
        {
            _scenes[i].SetActive(false);
        }
        _scenes[scene - 1].SetActive(true);
    }
}
