using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioMixerGroup masterMixer;
    private AudioSource src;

    //storage and retrieval stuff
    private static MusicManager musicManager;
    public static MusicManager Instance { get { return musicManager; } }

    private void Awake()
    {
        //aggressive singleton
        if(musicManager != null && musicManager != this)
        {
            Destroy(musicManager);
        }
        musicManager = this;

        src = GetComponent<AudioSource>();
    }

    public static void EaseMusic(float duration, bool fadeIn = true)
    {
        if (fadeIn)
        {
            Instance.StartCoroutine(Instance.EaseMusicIn(duration));
        }
        else
        {
            Instance.StartCoroutine(Instance.EaseMusicOut(duration));
        }
    }

    IEnumerator EaseMusicOut(float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            masterMixer.audioMixer.SetFloat("volumeTrueMusic", Mathf.Log10(Mathf.Lerp(1, 0.0001f, timer / duration)) * 20);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        masterMixer.audioMixer.SetFloat("volumeTrueMusic", Mathf.Log10(0.0001f) * 20);
        yield return null;
    }

    IEnumerator EaseMusicIn(float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            masterMixer.audioMixer.SetFloat("volumeTrueMusic", Mathf.Log10(Mathf.Lerp(0.0001f, 1, timer / duration)) * 20);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        masterMixer.audioMixer.SetFloat("volumeTrueMusic", Mathf.Log10(1) * 20);
        yield return null;
    }
}
