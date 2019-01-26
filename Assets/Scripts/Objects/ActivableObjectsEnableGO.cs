using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableObjectsEnableGO : ActivableObjects
{
    protected override IEnumerator MoveToNextStateCo(Vector3 position, Quaternion rotation, ObjectState newState)
    {
        _secondPosAndRot.gameObject.SetActive(newState == ObjectState.Pos1);

        _currentState = newState;
        yield return 0;
    }
}
