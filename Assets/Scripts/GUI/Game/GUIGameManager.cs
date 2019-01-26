using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIGameManager : SingletonGameObject<GUIGameManager>
{
    [SerializeField]
    GameObject _victoryDialog;

    [SerializeField]
    GUIMessageSystem _messageSystem;

    void Start()
    {
        if (_victoryDialog.activeSelf)
            _victoryDialog.SetActive(false);
    }

    public void Victory()
    {
        _victoryDialog.gameObject.SetActive(true);
    }

    public void ShowMessage(string text)
    {
        _messageSystem.ShowMessage(text);
    }
}
