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
        //if the player died while music was off, ensure music is on
        trueMasterMixer.audioMixer.SetFloat("volumeTrueMusic", 0);

        float timer = 0;
        float timerTarget = 1f;
        while(timer < timerTarget)
        {
            trueMasterMixer.audioMixer.SetFloat("volumeTrueMaster", Mathf.Log10(Mathf.Lerp(0.0001f, 1, timer / timerTarget)) * 20);
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
