using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClick : MonoBehaviour
{
    const string ActiveObjectsLayerName = "ActiveObjects";
    const string WallsLayerName = "Walls";

    Camera _camera;
    Ray _ray;

    [SerializeField]
    Transform[] _wallDetectors;

    List<Transform> _wallDisabled;
    
[SerializeField]
    List<Transform> _walls;

    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickInActivableObjects();
        }

        if (_wallDetectors != null && _wallDetectors.Length > 0)
            DetectWalls();
    }

    private void DetectWalls()
    {
        _walls.Sort(SortByCameraDistance);
        _walls[0].gameObject.SetActive(false);
        _walls[1].gameObject.SetActive(false);
        _walls[2].gameObject.SetActive(true);
        _walls[3].gameObject.SetActive(true);

        return;


        for (int i = _wallDetectors.Length - 1; i >= 0; --i)
        {
            _ray = _camera.ScreenPointToRay(_wallDetectors[i].position - _camera.transform.position);
            RaycastHit hitInfo;
            if (Physics.Raycast(_ray, out hitInfo, float.MaxValue, LayerMask.NameToLayer(WallsLayerName)))
            {
                hitInfo.collider.gameObject.SetActive(false);
                _wallDisabled.Add(hitInfo.collider.transform);
            }
        }
    }

    private int SortByCameraDistance(Transform wall1, Transform wall2)
    {
        if (Vector3.Distance(_camera.transform.position, wall1.position) <
            Vector3.Distance(_camera.transform.position, wall2.position))
            return -1;

        if (Vector3.Distance(_camera.transform.position, wall1.position) >
            Vector3.Distance(_camera.transform.position, wall2.position))
            return 1;

        return 0;
    }

    private void ClickInActivableObjects()
    {
        _ray = _camera.ScreenPointToRay(_camera.transform.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(_ray, out hitInfo, float.MaxValue, LayerMask.NameToLayer(ActiveObjectsLayerName)))
        {
            hitInfo.collider.GetComponent<ActivableObjects>().TouchObject();
        }
    }
}
