using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIMobileInput : MonoBehaviour
{
    CameraControllerPolar _camera;

    int _inputX;
    int _inputY;

    void Start()
    {
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
        _camera = FindObjectOfType<CameraControllerPolar>();
#else
        gameObject.SetActive(false);
            return;
#endif
    }

    private void Update()
    {
        _camera.Mov = new Vector2(_inputX, _inputY);
    }

    public void PressLeftEnter()
    {
        --_inputX;
    }

    public void PressLeftExit()
    {
        ++_inputX;
    }

    public void PressRightEnter()
    {
        ++_inputX;
    }

    public void PressRightExit()
    {
        --_inputX;
    }

    public void PressUpEnter()
    {
        ++_inputY ;
    }

    public void PressUpExit()
    {
        --_inputY;
    }

    public void PressDownEnter()
    {
        --_inputY ;
    }

    public void PressDownExit()
    {
        ++_inputY ;
    }
}
