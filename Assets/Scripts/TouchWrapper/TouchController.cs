using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : SingletonGameObject<TouchController>
{
    void Start()
    {
        
    }

    void Update()
    {

    }

    public bool GetTouch()
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            return Input.touchCount > 0;
#else
        return Input.GetMouseButtonDown(0);
#endif
    }

    internal Vector3 GetTouchPosition()
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        if (Input.touchCount > 0)
            return Input.GetTouch(0).position;
        else
            return Vector3.zero * -1000;
#else
        return Input.mousePosition;
#endif
    }
}
