using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIMessageSystem : MonoBehaviour
{
    [SerializeField]
    GUIMessage _template;

    [SerializeField]
    float messageDuration;

    GUIMessage _activeMessage;

    public void ShowMessage(string text)
    {
        if(_activeMessage != null)
            DestroyCurrent();

        _activeMessage = Instantiate<GUIMessage>(_template, transform);
        _activeMessage.Init(this);
        _activeMessage.ShowMessage(text, messageDuration);
    }

    internal void DestroyCurrent()
    {
        if (_activeMessage != null)
            Destroy(_activeMessage.gameObject);

        _activeMessage = null;
    }
}
