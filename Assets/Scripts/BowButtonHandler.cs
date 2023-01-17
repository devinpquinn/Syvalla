using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BowButtonHandler : MonoBehaviour
{
    private Animator anim;

    private TextMeshProUGUI letter;
    private Color normalColor;
    private Color highlightColor = Color.white;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        letter = GetComponent<TextMeshProUGUI>();
        normalColor = letter.color;
    }

    public void StartHighlight()
    {
        letter.color = highlightColor;
    }

    public void EndHighlight()
    {
        letter.color = normalColor;
    }

    //pick and set a new letter, allow the player to start drawing
    public void StartLetter()
    {
        anim.Play("BowButtonIn");
        CombatScript.combat.state = CombatScript.CombatState.Ready;

        //pick letter
    }

    //animate out letter
    public void EndLetter()
    {
        anim.Play("BowButtonOut");
    }
}
