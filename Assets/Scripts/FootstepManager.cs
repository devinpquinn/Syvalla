using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    //types of step sounds by material
    public enum groundMaterial { Dirt, Grass, Silent, Stone };

    public groundMaterial stepMat;

    //sound selections
    public List<AudioClip> DirtClips = new List<AudioClip>();
    public List<AudioClip> GrassClips = new List<AudioClip>();
    public List<AudioClip> StoneClips = new List<AudioClip>();

    //index of last sound played to avoid repeats; clear on new sound selection
    private int lastIndex = -1;

    //speaker
    private AudioSource src;

    private void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        if(stepMat != groundMaterial.Silent)
        {
            //play a random, non-repeated step sound from the appropriate list
            if(stepMat == groundMaterial.Dirt)
            {
                int soundIndex = Random.Range(0, DirtClips.Count);
                while(soundIndex == lastIndex)
                {
                    soundIndex = Random.Range(0, DirtClips.Count);
                }
                src.PlayOneShot(DirtClips[soundIndex]);
                lastIndex = soundIndex;
            }
            else if (stepMat == groundMaterial.Grass)
            {
                int soundIndex = Random.Range(0, GrassClips.Count);
                while (soundIndex == lastIndex)
                {
                    soundIndex = Random.Range(0, GrassClips.Count);
                }
                src.PlayOneShot(GrassClips[soundIndex]);
                lastIndex = soundIndex;
            }
            else if (stepMat == groundMaterial.Stone)
            {
                int soundIndex = Random.Range(0, StoneClips.Count);
                while (soundIndex == lastIndex)
                {
                    soundIndex = Random.Range(0, StoneClips.Count);
                }
                src.PlayOneShot(StoneClips[soundIndex]);
                lastIndex = soundIndex;
            }
        }
    }

    public void SwitchSound(groundMaterial newMat)
    {
        lastIndex = -1;
        stepMat = newMat;
    }
}
