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
    float _speed;

    Vector3 _currentRotation;


    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        //transform.RotateAround(_pivotPoint.position, Vector3.up, _speed * Time.deltaTime);
        transform.RotateAround(_pivotPoint.position, Vector3.right, 0.5f * _speed * Time.deltaTime);

        CheckAngleLimits();
    }

    void CheckAngleLimits()
    {
        _currentRotation = transform.eulerAngles;

        // rotations around X angle
        if (_currentRotation.x <= _minAngle.y)
            _currentRotation.Set(_minAngle.y, _currentRotation.y, _currentRotation.z);
        else if (_currentRotation.x >= _maxAngle.y)
            _currentRotation.Set(_maxAngle.y, _currentRotation.y, _currentRotation.z);

        // rotations around X angle
        if (_currentRotation.y <= _minAngle.x)
            _currentRotation.Set(_currentRotation.x, _minAngle.x, _currentRotation.z);
        else if (_currentRotation.y >= _maxAngle.x)
            _currentRotation.Set(_currentRotation.x, _maxAngle.x, _currentRotation.z);

        transform.eulerAngles = _currentRotation;
    }

    private void OnDrawGizmos()
    {

    }
}
