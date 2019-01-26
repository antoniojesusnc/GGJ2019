﻿using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableObjects : MonoBehaviour
{

    public enum ObjectState
    {
        Pos1,
        InTransition,
        Pos2
    }

    [SerializeField]
    float _timeMovement;
    [SerializeField]
    float _timeRotation;

    [SerializeField]
    Transform _secondPosAndRot;

    Vector3 _originalPosition;
    Quaternion _originalRotation;

    ObjectState _currentState;

    Outline _outline;

    void Start()
    {
        _outline = GetComponent<Outline>();

        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
    }

    // Update is called once per frame
    public void TouchObject()
    {
        if (_currentState == ObjectState.InTransition)
            return;

        if (_currentState == ObjectState.Pos1)
            StartCoroutine(MoveToCo(_secondPosAndRot.position, _secondPosAndRot.rotation));
        else
            StartCoroutine(MoveToCo(_originalPosition, _originalRotation));
    }

    private IEnumerator MoveToCo(Vector3 position, Quaternion rotation)
    {
        var initialState = _currentState;
        _currentState = ObjectState.InTransition;

        LeanTween.move(gameObject, position, _timeMovement).setEase(LeanTweenType.easeInOutQuint);
        LeanTween.rotate(gameObject, rotation.eulerAngles, _timeRotation).setEase(LeanTweenType.easeInOutQuint);

        yield return new WaitForSeconds(Math.Max(_timeMovement, _timeRotation));

        _currentState = initialState == ObjectState.Pos1 ? ObjectState.Pos2 : ObjectState.Pos1;
    }

    public List<Vector3> GetPointsToCheck()
    {
        List<Vector3> points = new List<Vector3>();

        Transform child;
        for (int i = transform.childCount - 1; i >= 0; --i)
        {
            child = transform.GetChild(i);
            if (child.GetComponent<SphereCollider>() != null)
                points.Add(child.position);

        }

        return points;

        /*
        temp.Points.Add(bound.center + new Vector3(bound.extents.x, bound.extents.y, bound.extents.z));
        temp.Points.Add(bound.center + new Vector3(bound.extents.x, -bound.extents.y, bound.extents.z));
        temp.Points.Add(bound.center + new Vector3(bound.extents.x, bound.extents.y, -bound.extents.z));
        temp.Points.Add(bound.center + new Vector3(bound.extents.x, -bound.extents.y, -bound.extents.z));

        temp.Points.Add(bound.center + new Vector3(-bound.extents.x, bound.extents.y, bound.extents.z));
        temp.Points.Add(bound.center + new Vector3(-bound.extents.x, -bound.extents.y, bound.extents.z));
        temp.Points.Add(bound.center + new Vector3(-bound.extents.x, bound.extents.y, -bound.extents.z));
        temp.Points.Add(bound.center + new Vector3(-bound.extents.x, -bound.extents.y, -bound.extents.z));
        */
    }

    public void SetOutline(int color)
    {
        if (_outline != null)
            _outline.color = color;
        else
            Debug.Log("Uou have to add the outline");
    }
}
