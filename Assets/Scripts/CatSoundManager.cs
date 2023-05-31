using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSoundManager : MonoBehaviour
{
    private AudioSource src;

    //audio clips
    public List<AudioClip> purrClips;
    public List<AudioClip> hissClips;

    private int lastPurr = -1;
    private int lastHiss = -1;

    private void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    public void Purr()
    {
        int key = Random.Range(0, purrClips.Count);
        while(key == lastPurr)
        {
            key = Random.Range(0, purrClips.Count);
        }
        lastPurr = key;
        src.PlayOneShot(purrClips[key]);
    }

    public void Hiss()
    {
        int key = Random.Range(0, hissClips.Count);
        while (key == lastHiss)
        {
            key = Random.Range(0, hissClips.Count);
        }
        lastHiss = key;
        src.PlayOneShot(hissClips[key]);
    }
}
