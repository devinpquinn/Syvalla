using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BowHandler : MonoBehaviour
{
    private Animator anim;

    public BowButtonHandler letterButton;
    public TextMeshProUGUI multText;

    [HideInInspector]
    public float damageMult = 0.1f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    //start draw animation and letter highlight
    public void StartDraw()
    {
        anim.Play("BowDraw");
        letterButton.StartHighlight();
    }

    public void SetDamageMult(float mult)
    {
        damageMult = mult;
    }

    //start release animation, end highlight
    public void StartRelease()
    {
        anim.Play("BowRelease");
        letterButton.EndHighlight();

        //set damage multiplier
        if(damageMult == 1)
        {
            multText.text = "<color=#808080>x</color>1.0";
        }
        else
        {
            multText.text = "<color=#808080>x</color>" + damageMult.ToString();
        }
        
        multText.gameObject.SetActive(false);
        multText.gameObject.SetActive(true);
    }

    //bow finished firing, animate out letter
    public void EndRelease()
    {
        letterButton.EndLetter();
    }
}
