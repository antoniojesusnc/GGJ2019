using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIBestTimer : MonoBehaviour
{
    public const string BestTimeString = "BestTime: \n{0}";
    TMPro.TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TMPro.TextMeshProUGUI>();
    }
    void Update()
    {
        if (LevelManager.Instance.IsGameFinished)
            return;

        _text.text = string.Format(BestTimeString, TimeUtils.TimeToText(GameManager.Instance.GetBestTime(GameManager.Instance.CurrentLevel)));
    }
}
