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

    //random key from selection that draws bow
    [HideInInspector]
    public string currentLetter = "";

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
        CombatScript.instance.state = CombatScript.CombatState.Ready;

        //pick letter
        currentLetter = letter.text;
        while(currentLetter == letter.text)
        {
            int letterKey = Random.Range(0, 6);
            switch (letterKey)
            {
                case 0:
                    currentLetter = "W";
                    break;
                case 1:
                    currentLetter = "A";
                    break;
                case 2:
                    currentLetter = "S";
                    break;
                case 3:
                    currentLetter = "D";
                    break;
                case 4:
                    currentLetter = "E";
                    break;
                case 5:
                    currentLetter = "Q";
                    break;
            }
        }
        letter.text = currentLetter;
    }

    //animate out letter
    public void EndLetter()
    {
        anim.Play("BowButtonOut");
    }
}
