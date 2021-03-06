﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIStartButton : MonoBehaviour
{
    // Start is called before the first frame update
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Caret))
        {
            GoToFirstLevel();
        }
    }
    public void OnClick()
    {
        GoToFirstLevel();
    }

    private void GoToFirstLevel()
    {

        FindObjectOfType<GUIMainMenu>().LoadLevel(1);
    }
}
