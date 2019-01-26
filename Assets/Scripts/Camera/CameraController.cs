using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera _camera;

    [SerializeField]
    [Header("Pivot point for the camera")]
    [Tooltip("The pivot forward will set the camera forward")]
    Transform _pivotPoint;

    [SerializeField]
    [Header("Min Angle Allow")]
    Vector2 _minAngle;

    [SerializeField]
    [Header("Max Angle Allow")]
    Vector2 _maxAngle;

    [SerializeField]
    [Header("Camera Speed")]
    Vector2 _speed;
    [SerializeField]
    Vector2 _decrementSpeedRatio;
    [SerializeField]
    Vector2 _speedMod;


    float _distanceToPivot;
    Vector3 _currentRotation;
    Vector3 _currentPosition;

    Vector2 _currentInput;
    Vector2 _lastInput;

    void Start()
    {
        _camera = GetComponent<Camera>();

        _distanceToPivot = (_camera.transform.position - _pivotPoint.transform.position).magnitude;

        Application.targetFrameRate = 60;
    }

    void Update()
    {
        //UpdateInput();

        _currentRotation = transform.eulerAngles;
        _currentPosition = transform.position;

        UpdateMovement();

        _speed -= _speed * _decrementSpeedRatio * Time.deltaTime;

        transform.position = _currentPosition;
        transform.LookAt(_pivotPoint);
    }

    void UpdateMovement()
    {
        // up/down movement of the camera
        if (_speed.x != 0)
        {
            if (CanMoveX())
            {
                _currentPosition = transform.position;
            }
            else
            {
                SetToLimitPositonX();
            }
        }

        // up/down movement of the camera
        if (_speed.y != 0)
        {
            if (CanMoveY())
            {
                transform.RotateAround(_pivotPoint.position, Vector3.up, 0.5f * _speed.y * Time.deltaTime);
                _currentPosition = transform.position;
            }
            else
            {
                SetToLimitPositonY();
            }
        }
    }

    private bool CanMoveX()
    {
        bool isBetweenAngles = _currentRotation.x >= _minAngle.x && _currentRotation.x <= _maxAngle.x;
        bool isBotXLimitAndSpeedUp= _currentRotation.x >= _minAngle.x && _speed.x > 0;
        bool isTopXLimitAndSpeedDown = _currentRotation.x <= _maxAngle.x && _speed.x < 0;


        return isBetweenAngles ||
                    (!isBotXLimitAndSpeedUp && !isTopXLimitAndSpeedDown);
    }

    private bool CanMoveY()
    {
        bool isBetweenAngles = _currentRotation.y >= _minAngle.y && _currentRotation.y <= _maxAngle.y;
        bool isBotYLimitAndSpeedUp = _currentRotation.y >= _maxAngle.y && _speed.y > 0;
        bool isTopYLimitAndSpeedDown = _currentRotation.y <= _minAngle.y && _speed.y < 0;


        return isBetweenAngles || 
            (!isBotYLimitAndSpeedUp && !isTopYLimitAndSpeedDown);
    }

    private void SetToLimitPositonX()
    {
        float angle = _currentRotation.x;
        if (_currentRotation.x <= _minAngle.x)
        {
            angle = _minAngle.x + 0.1f;
        }
        else if (_currentRotation.x >= _maxAngle.x)
        {
            angle = _maxAngle.x - 0.1f;
        }

        Vector3 point = Quaternion.Euler(angle, 0, 0) * -_pivotPoint.forward;
        point *= _distanceToPivot;
        point += _pivotPoint.transform.position;

        _currentPosition.Set(_currentPosition.x, point.y, _currentPosition.z);
    }

    private void SetToLimitPositonY()
    {
        float angle = _currentRotation.y;
        if (_currentRotation.y <= _minAngle.y)
        {
            angle = _minAngle.y + 0.1f;
        }
        else if (_currentRotation.y >= _minAngle.y)
        {
            angle = _maxAngle.y - 0.1f;
        }

        Vector3 point = Quaternion.Euler(0, angle, 0) * -_pivotPoint.forward;
        point *= _distanceToPivot;
        point += _pivotPoint.transform.position;

        _currentPosition.Set(point.x, _currentPosition.y, _currentPosition.z);
    }

    private void UpdateInput()
    {
        if (Input.GetMouseButtonDown(0))
            StartInputPressed();

        if (Input.GetMouseButton(0))
            UpdateInputPressed();

        if (Input.GetMouseButtonUp(0))
            FinishInputPressed();
    }



    private void StartInputPressed()
    {
        _lastInput = _currentInput = _camera.ScreenToViewportPoint(Input.mousePosition) * _speedMod;
        _speed.Set(0, 0);
    }


    private void UpdateInputPressed()
    {
        _currentInput = _camera.ScreenToViewportPoint(Input.mousePosition) * _speedMod;

        //Vector2 speedVariance = new Vector2(_lastInput.x - _currentInput.x, _lastInput.y - _currentInput.y);
        Vector2 speedVariance = new Vector2(_currentInput.x - _lastInput.x, 0);
        Debug.Log(_currentInput + " -" + speedVariance + " - " + _lastInput);
        _speed += speedVariance;

        _lastInput = _currentInput;
    }

    private void FinishInputPressed()
    {

    }

   

    private void OnDrawGizmos()
    {

    }
}
