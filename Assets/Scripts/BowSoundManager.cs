using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowSoundManager : MonoBehaviour
{
    private AudioSource src;

    //audio clips
    public AudioClip unstowClip;
    public AudioClip stowClip;

    private void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    public void Stow(bool stow)
    {
        if (stow)
        {
            src.PlayOneShot(stowClip);
        }
        else
        {
            src.PlayOneShot(unstowClip);
        }
    }
}
