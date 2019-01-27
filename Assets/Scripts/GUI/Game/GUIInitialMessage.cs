using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIInitialMessage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayFadeOutCo());
    }

    IEnumerator DelayFadeOutCo()
    {

        float delayTime = 2;
        float fadeTime = 1;

        yield return new WaitForSeconds(delayTime);
        LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 0, fadeTime);
    }
}
