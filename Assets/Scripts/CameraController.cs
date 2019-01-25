using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera _camera;

    [SerializeField]
    [Header("Pivot point for the camera")]\
    [Tooltip("The pivot forward will set the camera forward")]
    Transform _pivotPoint;

    [Header("Min Angle Allow")]
    float _minAngle;

    [Header("Max Angle Allow")]
    float _maxAngle;

    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {

    }
}
