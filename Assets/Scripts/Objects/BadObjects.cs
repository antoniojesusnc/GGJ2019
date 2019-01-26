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

    public void SetOutline(bool enableOutline)
    {
        _outline.enabled = enableOutline;
    }
}
