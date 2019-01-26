using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIBestTimeLevel : MonoBehaviour
{
    [SerializeField]
    int _level;
    
    void Start()
    {
        float time = GameManager.Instance.GetBestTime(_level);

        GetComponent<TMPro.TextMeshProUGUI>().text = TimeUtils.TimeToText(time);
    }
}
