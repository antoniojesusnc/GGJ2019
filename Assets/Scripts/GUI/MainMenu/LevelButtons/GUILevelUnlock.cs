using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUILevelUnlock : MonoBehaviour
{
    [SerializeField]
    int _level;

    
    void Start()
    {
        if (GameManager.Instance.IsLevelUnLock(_level))
            Destroy(gameObject);
    }
}
