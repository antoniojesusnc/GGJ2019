using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIGameManager : SingletonGameObject<GUIGameManager>
{
    [SerializeField]
    GameObject _victoryMessage;
    
    void Start()
    {
        if (_victoryMessage.activeSelf)
            _victoryMessage.SetActive(false);
    }

    public void Victory()
    {
        _victoryMessage.gameObject.SetActive(true);

    }
}
