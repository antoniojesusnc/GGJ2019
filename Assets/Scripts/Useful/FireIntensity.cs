using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireIntensity : MonoBehaviour
{
    [SerializeField]
    Vector2 _minMaxIntensity;

    [SerializeField]
    Vector2 _minMaxTime;

    Light _ligth;
    bool _goingUp;

    void Start()
    {
        _ligth = GetComponent<Light>();

        _goingUp = true;
        StartCoroutine(ChangeIntensityCo(_minMaxIntensity.x, _minMaxIntensity.y));
    }

    private IEnumerator ChangeIntensityCo(float initialValue, float finalValue)
    {
        float timeStamp = 0;
        float totalTime = UnityEngine.Random.Range(_minMaxTime.x, _minMaxTime.y);

        while (timeStamp < totalTime)
        {
            yield return 0;
            timeStamp += Time.deltaTime;
            _ligth.intensity = Mathf.Lerp(initialValue, finalValue, timeStamp / totalTime);
        }

        NextMovement();
    }

    private void NextMovement()
    {
        _goingUp = !_goingUp;
        if(_goingUp)
            StartCoroutine(ChangeIntensityCo(_minMaxIntensity.x, _minMaxIntensity.y));
        else
            StartCoroutine(ChangeIntensityCo(_minMaxIntensity.y, _minMaxIntensity.x));
    }

}
