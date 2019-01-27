using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonGameObject<AudioManager>
{
    public const string ClipBackground = "ClipBackground";
    public const string ClipEndGame = "ClipEndGame";
    public const string ClipActivableObj = "ClipActivableObj";
    public const string ClipButtonSound = "ClipButtonSound";

    [SerializeField]
    List<AudioClip> _audios;

    [SerializeField]
    AudioSource _template;

    List<AudioSource> _audiosPlaying;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        _audiosPlaying = new List<AudioSource>();
    }

    public void PlaySound(string name, Vector3 position, bool loopable = false, float volume = 1)
    {
        if (_audios == null)
            return;

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
        newAudioSource.volume = volume;
        newAudioSource.Play();

        _audiosPlaying.Add(newAudioSource);
    }

    public void StopSound(string name)
    {
        AudioSource audioSource;
        do
        {
            audioSource = GetAudioSource(name);
            if (audioSource != null)
                StartCoroutine(FadeOutCo(audioSource, 0));
        } while (audioSource != null);
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
        _audiosPlaying.Remove(audioSoucre);
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

    private AudioSource GetAudioSource(string name)
    {
        for (int i = _audiosPlaying.Count - 1; i >= 0; --i)
        {
            if (_audiosPlaying[i].name == name)
                return _audiosPlaying[i];
        }
        return null;
    }
}
