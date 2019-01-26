using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUICurrentTimer : MonoBehaviour
{
    public const string BestTimeString = "CurrentTime: \n{0}";

    TMPro.TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TMPro.TextMeshProUGUI>();
    }
    void Update()
    {
        if (LevelManager.Instance.IsGameFinished)
            return;

        _text.text = string.Format(BestTimeString, TimeUtils.TimeToText(LevelManager.Instance.ElapsedTime));
    }
}
