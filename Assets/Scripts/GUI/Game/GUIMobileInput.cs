using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIMobileInput : MonoBehaviour
{
    CameraControllerPolar _camera;

    void Start()
    {
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
            gameObject.SetActive(false);
            return;
#endif
        _camera = FindObjectOfType<CameraControllerPolar>();
    }

    public void PressLeft()
    {
        _camera.Mov = new Vector2(-1, _camera.Mov.y);
    }

    public void PressRight()
    {
        _camera.Mov = new Vector2(1, _camera.Mov.y);
    }

    public void PressUp()
    {
        _camera.Mov = new Vector2(_camera.Mov.x, 1);
    }

    public void PressDown()
    {
        _camera.Mov = new Vector2(_camera.Mov.x, -1);
    }
}
