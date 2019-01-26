using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIMessage : MonoBehaviour
{
    [SerializeField]
    float _timeFade;

    GUIMessageSystem _messageSystem;
    LTSeq _sequence;

    public void Init(GUIMessageSystem messageSystem)
    {
        _messageSystem = messageSystem;
    }

    public void ShowMessage(string message, float time)
    {
        gameObject.SetActive(true);
        GetComponent<TMPro.TextMeshProUGUI>().text = message;

        StartCoroutine(AnimCo(time));
    }

    private IEnumerator AnimCo(float time)
    {
        var canvasGroup = GetComponent<CanvasGroup>();

        float _timestamp = 0;
        while (_timestamp < _timeFade)
        {
            _timestamp += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, _timestamp / _timeFade);
            yield return 0;
        }

        yield return new WaitForSeconds(time);

        _timestamp = 0;
        while (_timestamp < _timeFade)
        {
            _timestamp += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, _timestamp / _timeFade);
            yield return 0;
        }

        _messageSystem.DestroyCurrent();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
