using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonGameObject<AudioManager>
{
    public const string ClipActivableObj = "ClipActivableObj";
    public const string ClipButtonSound = "ClipButtonSound";


    [SerializeField]
    List<AudioClip> _audios;

    [SerializeField]
    AudioSource _template;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(string name, Vector3 position, bool loopable = false)
    {

        AudioClip clip = GetAudioclip(name);
        if (clip == null)
        {
            Debug.LogWarning("Audio with name: " + name + " not found");
            return;
        }

        var newAudioSource = GameObject.Instantiate<AudioSource>(_template, position, Quaternion.identity, transform);

        if (!loopable)
            StartCoroutine(FadeOutCo(newAudioSource, clip.length));

        newAudioSource.clip = clip;
        newAudioSource.loop = loopable;
        newAudioSource.Play();
    }

    private IEnumerator FadeOutCo(AudioSource audioSoucre, float length)
    {
        yield return new WaitForSeconds(length);

        float fadeOutTime = 1;
        float timeStamp = 0;

        while(timeStamp < fadeOutTime)
        {
            timeStamp += Time.deltaTime;

            audioSoucre.volume = Mathf.Lerp(1, 0, timeStamp / fadeOutTime);

            yield return 0;
        }
        Destroy(audioSoucre.gameObject);
    }

    private AudioClip GetAudioclip(string name)
    {
        for (int i = _audios.Count - 1; i >= 0; --i)
        {
            if (_audios[i].name == name)
                return _audios[i];
        }
        return null;
    }
}
