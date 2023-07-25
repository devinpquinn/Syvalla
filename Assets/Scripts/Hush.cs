using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hush : MonoBehaviour
{
    public void MusicIn()
    {
        MusicManager.EaseMusic(2);
    }

    public void MusicOut()
    {
        MusicManager.EaseMusic(2, false);
    }
}
