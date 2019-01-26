using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectState
{
    Pos1,
    Pos2,
    Pos3,
    None
}

public class ActivableObjects : MonoBehaviour
{


    bool _inTransition;

    [SerializeField]
    float _timeMovement;
    [SerializeField]
    float _timeRotation;

    [SerializeField]
    protected Transform _secondPosAndRot;
    [SerializeField]
    Transform _thirdPosAndRot;

    [SerializeField]
    ObjectState _successState = ObjectState.None;

    public bool IsInSuccessState
    {
        get
        {
            if (_successState == ObjectState.None)
                return true;
            return _successState == _currentState;
        }
    }

    public ObjectState GetState
    {
        get
        {
            return _successState;
        }
    }

    Vector3 _originalPosition;
    Quaternion _originalRotation;

    protected ObjectState _currentState;

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
            StartCoroutine(MoveToNextStateCo(_secondPosAndRot.position, _secondPosAndRot.rotation, ObjectState.Pos2));
        }
        else if (_currentState == ObjectState.Pos2)
        {
            if (_thirdPosAndRot != null)
                StartCoroutine(MoveToNextStateCo(_thirdPosAndRot.position, _thirdPosAndRot.rotation, ObjectState.Pos3));
            else
                StartCoroutine(MoveToNextStateCo(_originalPosition, _originalRotation, ObjectState.Pos1));
        }
        else
        {
            StartCoroutine(MoveToNextStateCo(_originalPosition, _originalRotation, ObjectState.Pos1));
        }
    }

    protected virtual IEnumerator MoveToNextStateCo(Vector3 position, Quaternion rotation, ObjectState newState)
    {
        _inTransition = true;

        LeanTween.move(gameObject, position, _timeMovement).setEase(LeanTweenType.easeInOutQuart);
        LeanTween.rotate(gameObject, rotation.eulerAngles, _timeRotation).setEase(LeanTweenType.easeInOutQuart);

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
