using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClick : MonoBehaviour
{
    readonly Color OutlineNotSelected = new Color(255.0f/255.0f, 255.0f/255.0f, 0.0f, 1.0f);
    readonly Color OutlineSelected = Color.green;

    Camera _camera;
    Ray _ray;

    [SerializeField]
    List<GameObject> _walls;

    [SerializeField]
    float _timeToShowWalls;
    [SerializeField]
    float _timeToHideWalls;

    [SerializeField]
    bool _checkClicks = true;

    ActivableObjects _lastSelected;

    void Start()
    {
        _camera = GetComponent<Camera>();

        if (_walls == null || _walls.Count <= 0 || _walls[0] == null)
            _walls = new List<GameObject>( GameObject.FindGameObjectsWithTag("Walls"));

        for (int i = _walls.Count - 1; i >= 0; --i)
        {
            if (!_walls[i].activeSelf)
                _walls[i].SetActive(true);
        }
    }

    void Update()
    {
        if (_checkClicks)
        {
            if (LevelManager.Instance.IsGameFinished)
                return;

            CheckActivableObjects();
        }

        DetectWalls();
    }

    private void DetectWalls()
    {
        _walls.Sort(SortByCameraDistance);

        float alphaDestination = 0.15f;

        if (!LeanTween.isTweening(_walls[0]))
            LeanTween.alpha(_walls[0], alphaDestination, _timeToHideWalls);
        if (!LeanTween.isTweening(_walls[1]))
            LeanTween.alpha(_walls[1], alphaDestination, _timeToHideWalls);
        if (!LeanTween.isTweening(_walls[2]))
            LeanTween.alpha(_walls[2], 1, _timeToShowWalls);
        if (!LeanTween.isTweening(_walls[3]))
            LeanTween.alpha(_walls[3], 1, _timeToShowWalls);
    }

    private int SortByCameraDistance(GameObject wall1, GameObject wall2)
    {
        if (Vector3.Distance(_camera.transform.position, wall1.transform.position) <
            Vector3.Distance(_camera.transform.position, wall2.transform.position))
            return -1;

        if (Vector3.Distance(_camera.transform.position, wall1.transform.position) >
            Vector3.Distance(_camera.transform.position, wall2.transform.position))
            return 1;

        return 0;
    }

    private void CheckActivableObjects()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        int layerMask =
            1 << LayerMask.NameToLayer(LayerReference.ActiveObjects) |
            1 << LayerMask.NameToLayer(LayerReference.StaticObjects);

        if (Physics.Raycast(_ray, out hitInfo, float.MaxValue, layerMask))
        {
            var newActivableObject = hitInfo.collider.GetComponent<ActivableObjects>();
            if (newActivableObject == null)
                return;

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

    public void DisableClick()
    {
        _checkClicks = false;
    }
}
