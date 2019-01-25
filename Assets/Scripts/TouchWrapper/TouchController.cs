using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : SingletonGameObject<TouchController>
{
    

    public struct TouchWrapper
    {
        Vector2 position;
        Vector2 deltaPosition;
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        GetInput()
#else 
        GetMouse();
#endif
    }

    private void GetMouse()
    {
        if (Input.GetMouseButton(0))
        {

        }
    }
}
