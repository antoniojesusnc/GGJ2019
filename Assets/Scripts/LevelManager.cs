using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonGameObject<LevelManager>
{
    public bool IsGamePaused { get; set; }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckVictory();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    public void CheckVictory()
    {
        bool allObjectsHidden = CheckIfAllObjectsHidden();

        if (allObjectsHidden)
        {
            IsGamePaused = true;
            GUIGameManager.Instance.Victory();
        }
    }

    private bool CheckIfAllObjectsHidden()
    {
        var badObjects = GameObject.FindObjectsOfType<BadObjects>();
        for (int i = badObjects.Length - 1; i >= 0; --i)
        {
            //badObjects[i].GetComponent<Renderer>().bounds.
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        bool hitAnyObject = false;
        var badObjects = GameObject.FindObjectsOfType<BadObjects>();
        for (int i = badObjects.Length - 1;!hitAnyObject && i >= 0; --i)
        {
            var bound = badObjects[i].GetComponent<MeshCollider>().bounds;
            Gizmos.DrawWireCube(bound.center, bound.size);

            List<Vector3> points = new List<Vector3>();
            points.Add(bound.center + new Vector3(bound.extents.x, bound.extents.y, bound.extents.z));
            points.Add(bound.center + new Vector3(bound.extents.x, -bound.extents.y, bound.extents.z));
            points.Add(bound.center + new Vector3(bound.extents.x, bound.extents.y, -bound.extents.z));
            points.Add(bound.center + new Vector3(bound.extents.x, -bound.extents.y, -bound.extents.z));

            points.Add(bound.center + new Vector3(-bound.extents.x, bound.extents.y, bound.extents.z));
            points.Add(bound.center + new Vector3(-bound.extents.x, -bound.extents.y, bound.extents.z));
            points.Add(bound.center + new Vector3(-bound.extents.x, bound.extents.y, -bound.extents.z));
            points.Add(bound.center + new Vector3(-bound.extents.x, -bound.extents.y, -bound.extents.z));

            Vector3 cameraPosition = Camera.main.transform.position;
            for (int j = points.Count - 1;!hitAnyObject && j >= 0; --j)
            {
                Gizmos.DrawLine(cameraPosition, points[j]);
                if (Physics.Raycast(cameraPosition, points[j], float.MaxValue, 1 <<LayerMask.NameToLayer("BadObjects")))
                    hitAnyObject = true;

            }

            Debug.Log("hitAnyObject: "+ hitAnyObject);
        }
    }
}
