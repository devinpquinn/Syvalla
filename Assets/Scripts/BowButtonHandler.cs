using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowButtonHandler : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    //pick and set a new letter, allow the player to start drawing
    public void StartLetter()
    {
        anim.Play("BowButtonIn");
        CombatScript.combat.state = CombatScript.CombatState.Ready;

        //pick letter
    }

    public void EndLetter()
    {
        anim.Play("BowButtonOut");
    }
}
