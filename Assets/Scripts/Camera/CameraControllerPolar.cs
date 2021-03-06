﻿using System;
using UnityEngine;

public class CameraControllerPolar : MonoBehaviour
{
    Camera _camera;

    [SerializeField]
    [Header("Pivot point for the camera")]
    [Tooltip("The pivot forward will set the camera forward")]
    Transform _pivotPoint;

    [SerializeField]
    [Header("Up Down Angle Min Max")]
    Vector2 _upDownAngleMinMax;

    [SerializeField]
    [Header("Camera Speed")]
    float _speed;
    [SerializeField]
    [Header("Scroll Speed")]
    float _scrollSpeed;

    [Header("Fake Movement")]
    [SerializeField]
    bool _doFakeMovement;
    [SerializeField]
    Vector2 _fakeMovement;

    SphericalCoordinates _sphericalCoordinate;

    public Vector2 Mov { get; set; }
    public float _zoom { get; set; }

    void Start()
    {
        _camera = GetComponent<Camera>();

        _sphericalCoordinate = new SphericalCoordinates();
        Mov = new Vector2();
    }

    void Update()
    {
        if (!_doFakeMovement)
        {
            if (!LevelManager.Instance.IsGameFinished)
            {
                UpdateInput();
            }
        }
        else
        {
            Mov = _fakeMovement;
        }

        UpdateMoveCamera();
    }

    void UpdateInput()
    {
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
        float movX = Input.GetAxis("Horizontal");
        float movY = Input.GetAxis("Vertical");
        Mov = new Vector2(movX, movY);

        bool ScrollUp = Input.GetKey(KeyCode.Z);
        bool ScrollDown = Input.GetKey(KeyCode.X);

        _zoom = 0;
        if (ScrollUp)
            _zoom = 1;
        else if (ScrollDown)
            _zoom = -1;
#endif
    }

    void UpdateMoveCamera()
    {
        _sphericalCoordinate.SetProperties(
            transform.position, 1, 500,
            0, Mathf.PI * 2,
            Mathf.Deg2Rad * _upDownAngleMinMax.x, Mathf.Deg2Rad * _upDownAngleMinMax.y);

        //Mathf.Deg2Rad * _minAngle.x, Mathf.Deg2Rad * _maxAngle.x,

        transform.position = _sphericalCoordinate.toCartesian + _pivotPoint.position;

        if (Mov.x != 0 || Mov.y != 0)
            transform.position = _sphericalCoordinate.Rotate(Mov.x * _speed * Time.deltaTime, Mov.y * _speed * Time.deltaTime).toCartesian + _pivotPoint.position;

        // temporal zoom in, zoom out
       if (_zoom != 0)
            transform.position = _sphericalCoordinate.TranslateRadius(_zoom * Time.deltaTime * _scrollSpeed).toCartesian + _pivotPoint.position;

        //_distanceToPivot = (_camera.transform.position - _pivotPoint.transform.position).magnitude;

        transform.LookAt(_pivotPoint);
    }

    public void SetFakeMovement(Vector2 fakeSpeed, float fakeZoomSpeed)
    {
        _doFakeMovement = true;
        _fakeMovement = fakeSpeed;
        _zoom = 1;
        _scrollSpeed = fakeZoomSpeed;
        GetComponent<CameraClick>().DisableClick();
    }


}
