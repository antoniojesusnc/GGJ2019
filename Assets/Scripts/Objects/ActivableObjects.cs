using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableObjects : MonoBehaviour
{

    public enum ObjectState
    {
        Pos1,
        Pos2,
        Pos3,
        None
    }

    bool _inTransition;

    [SerializeField]
    float _timeMovement;
    [SerializeField]
    float _timeRotation;

    [SerializeField]
    Transform _secondPosAndRot;
    [SerializeField]
    Transform _thirdPosAndRot;

    [SerializeField]
    ObjectState _successState;

    public bool IsInSuccessState
    {
        get
        {
            if (_successState == ObjectState.None)
                return true;
            return _successState == _currentState;
        }
    }

    Vector3 _originalPosition;
    Quaternion _originalRotation;

    ObjectState _currentState;

    QuickOutline _outline;

    void Start()
    {
        _outline = GetComponent<QuickOutline>();

        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
    }

    // Update is called once per frame
    public void TouchObject()
    {
        if (_inTransition)
            return;

        if (_currentState == ObjectState.Pos1)
        {
            StartCoroutine(MoveToCo(_secondPosAndRot.position, _secondPosAndRot.rotation, ObjectState.Pos2));
        }
        else if (_currentState == ObjectState.Pos2)
        {
            if (_thirdPosAndRot != null)
                StartCoroutine(MoveToCo(_thirdPosAndRot.position, _thirdPosAndRot.rotation, ObjectState.Pos3));
            else
                StartCoroutine(MoveToCo(_originalPosition, _originalRotation, ObjectState.Pos1));
        }
        else
        {
            StartCoroutine(MoveToCo(_originalPosition, _originalRotation, ObjectState.Pos1));
        }
    }

    private IEnumerator MoveToCo(Vector3 position, Quaternion rotation, ObjectState newState)
    {
        _inTransition = true;

        LeanTween.move(gameObject, position, _timeMovement).setEase(LeanTweenType.easeInOutQuint);
        LeanTween.rotate(gameObject, rotation.eulerAngles, _timeRotation).setEase(LeanTweenType.easeInOutQuint);

        yield return new WaitForSeconds(Math.Max(_timeMovement, _timeRotation));

        _currentState = newState;
        _inTransition = false;
    }

    public void SetOutline(Color color)
    {
        if (_outline != null)
            _outline.OutlineColor = color;
        else
            Debug.Log("Uou have to add the outline");
    }
}
