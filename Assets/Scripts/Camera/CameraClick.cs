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
    List<Transform> _walls;

    void Start()
    {
        _camera = GetComponent<Camera>();

        for (int i = _walls.Count - 1; i >= 0; --i)
        {
            if (!_walls[i].gameObject.activeSelf)
                _walls[i].gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickInActivableObjects();
        }

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
        _ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(_ray, out hitInfo, float.MaxValue, 1 << LayerMask.NameToLayer(ActiveObjectsLayerName)))
        {
            hitInfo.collider.GetComponent<ActivableObjects>().TouchObject();
        }

        //Debug.DrawRay(_camera.transform.position, )
    }
}
