using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AreaCardHandler : MonoBehaviour
{
    public AudioMixerGroup trueMasterMixer;

    public void StartCard()
    {
        //set audio volume to 0 and fade in
        StartCoroutine(EaseAudio());

        //lock player
        PlayerController.Instance.state = PlayerController.playerState.Locked;
    }

    IEnumerator EaseAudio()
    {
        float timer = 0;
        float timerTarget = 0.5f;
        while(timer < timerTarget)
        {
            trueMasterMixer.audioMixer.SetFloat("volumeTrueMaster", Mathf.Log10(Mathf.Lerp(0, 1, timer / timerTarget)) * 20);
            timer += Time.deltaTime;
            yield return null;
        }

        trueMasterMixer.audioMixer.SetFloat("volumeTrueMaster", Mathf.Log10(1) * 20);
        yield return null;
    }

    public void EndCard()
    {
        //unlock player
        PlayerController.Instance.state = PlayerController.playerState.Normal;
    }
}
