using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWithWallObject : MonoBehaviour
{

    public Renderer Wall
    {
        get
        {
            return _wall;
        }
    }

    [SerializeField]
    Renderer _wall;

    Renderer _renderer;
}
