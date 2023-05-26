using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Fade : MonoBehaviour
{
    public bool fadeInAtStart;
    public AudioMixerGroup masterMixer;

    private Animator anim;

    private static Fade fade;
    public static Fade Instance { get { return fade; } }

    private void Awake()
    {
        //aggressive singleton
        if (fade != null && fade != this)
        {
            Destroy(fade);
        }
        fade = this;

        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (fadeInAtStart)
        {
            FadeEffect(false);
        }
        else
        {
            //ensure master audio is set to 1
            masterMixer.audioMixer.SetFloat("volumeTrueMaster", 0);
        }
    }

    IEnumerator EaseAudioIn()
    {
        //if the player died while music was off, ensure music is on
        masterMixer.audioMixer.SetFloat("volumeTrueMusic", 0);

        float timer = 0;
        float timerTarget = 0.5f;
        while (timer < timerTarget)
        {
            masterMixer.audioMixer.SetFloat("volumeTrueMaster", Mathf.Log10(Mathf.Lerp(0.0001f, 1, timer / timerTarget)) * 20);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        masterMixer.audioMixer.SetFloat("volumeTrueMaster", 0);
        yield return null;
    }

    IEnumerator EaseAudioOut()
    {
        float timer = 0;
        float timerTarget = 0.5f;
        while (timer < timerTarget)
        {
            masterMixer.audioMixer.SetFloat("volumeTrueMaster", Mathf.Log10(Mathf.Lerp(1, 0, timer / timerTarget)) * 20);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        masterMixer.audioMixer.SetFloat("volumeTrueMaster", Mathf.Log10(0.0001f) * 20);
        yield return null;
    }

    public static void FadeEffect(bool fadeOut = true)
    {
        if (fadeOut)
        {
            fade.anim.Play("FadeOut");
            //lerp out audio
            Instance.StartCoroutine(Instance.EaseAudioOut());
        }
        else
        {
            fade.anim.Play("FadeIn");
            //lerp in audio
            Instance.StartCoroutine(Instance.EaseAudioIn());
        }
    }
}
