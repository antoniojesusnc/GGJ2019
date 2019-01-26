using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIStartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Caret))
        {
            GoToMainScene();
        }
    }
    public void OnClick()
    {
        GoToMainScene();
    }

    private void GoToMainScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
