using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public bool fadeInAtStart;

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
    }

    public static void FadeEffect(bool fadeOut = true)
    {
        if (fadeOut)
        {
            fade.anim.Play("FadeOut");
        }
        else
        {
            fade.anim.Play("FadeIn");
        }
    }
}
