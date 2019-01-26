using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClick : MonoBehaviour
{
    const string ActiveObjectsLayerName = "ActiveObjects";
    const string WallsLayerName = "Walls";

    const int OutlineNotSelected = 1;
    const int OutlineSelected = 2;

    Camera _camera;
    Ray _ray;

    [SerializeField]
    List<Transform> _walls;

    [SerializeField]
    float _timeToShowWalls;
    [SerializeField]
    float _timeToHideWalls;

    [SerializeField]
    bool _checkClicks;

    ActivableObjects _lastSelected;

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
        if (_checkClicks)
        {
            if (LevelManager.Instance.IsGamePaused)
                return;

            CheckActivableObjects();
        }

        DetectWalls();
    }

    private void DetectWalls()
    {
        _walls.Sort(SortByCameraDistance);

        if (!LeanTween.isTweening(_walls[0].gameObject))
            LeanTween.alpha(_walls[0].gameObject, 0, _timeToHideWalls);
        if (!LeanTween.isTweening(_walls[1].gameObject))
            LeanTween.alpha(_walls[1].gameObject, 0, _timeToHideWalls);

        if (!LeanTween.isTweening(_walls[2].gameObject))
            LeanTween.alpha(_walls[2].gameObject, 1, _timeToShowWalls);
        if (!LeanTween.isTweening(_walls[3].gameObject))
            LeanTween.alpha(_walls[3].gameObject, 1, _timeToShowWalls);
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

    private void CheckActivableObjects()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(_ray, out hitInfo, float.MaxValue, 1 << LayerMask.NameToLayer(ActiveObjectsLayerName)))
        {
            var newActivableObject = hitInfo.collider.GetComponent<ActivableObjects>();
            if (_lastSelected == null)
            {
                newActivableObject.SetOutline(OutlineNotSelected);
            }
            else if (_lastSelected.GetInstanceID() == newActivableObject.GetInstanceID())
            {
                _lastSelected.SetOutline(OutlineSelected);
            }
            else
            {
                _lastSelected.SetOutline(OutlineNotSelected);
                newActivableObject.SetOutline(OutlineSelected);
            }

            if (Input.GetMouseButtonDown(0))
                newActivableObject.TouchObject();

            _lastSelected = newActivableObject;
        }
        else
        {
            if (_lastSelected != null)
            {
                _lastSelected.SetOutline(OutlineNotSelected);
            }
        }
    }
}
