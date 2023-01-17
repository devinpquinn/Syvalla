using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowHandler : MonoBehaviour
{
    private Animator anim;

    public BowButtonHandler letterButton;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void StartDraw()
    {
        anim.Play("BowDraw");
    }

    public void StartRelease()
    {
        anim.Play("BowRelease");
        letterButton.EndLetter();
    }

    //bow finished firing
    public void EndRelease()
    {
        letterButton.StartLetter();
    }
}
