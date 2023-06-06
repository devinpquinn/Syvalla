using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFootstepManager : MonoBehaviour
{
    //sound selections
    public List<AudioClip> frontClips = new List<AudioClip>();
    public List<AudioClip> backClips = new List<AudioClip>();

    //index of last sound played to avoid repeats; clear on new sound selection
    private int lastIndexFront = -1;
    private int lastIndexBack = -1;

    //speaker
    private AudioSource src;

    private void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    public void PlaySound(bool front = true)
    {
        if (front)
        {
            int soundIndex = Random.Range(0, frontClips.Count);
            while (soundIndex == lastIndexFront)
            {
                soundIndex = Random.Range(0, frontClips.Count);
            }
            src.PlayOneShot(frontClips[soundIndex]);
            lastIndexFront = soundIndex;
        }
        else
        {
            int soundIndex = Random.Range(0, backClips.Count);
            while (soundIndex == lastIndexBack)
            {
                soundIndex = Random.Range(0, backClips.Count);
            }
            src.PlayOneShot(backClips[soundIndex]);
            lastIndexBack = soundIndex;
        }
    }

}
