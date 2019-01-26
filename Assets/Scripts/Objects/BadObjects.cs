using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadObjects : MonoBehaviour
{
    Outline _outline;

    void Start()
    {
        _outline = GetComponent<Outline>();
    }

    public void SetOutline(bool enableOutline)
    {
        _outline.enabled = enableOutline;
    }
}
