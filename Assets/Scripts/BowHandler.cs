using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BowHandler : MonoBehaviour
{
    private Animator anim;

    //audio
    public AudioSource src;
    public AudioClip drawClip;
    public List<AudioClip> releaseClips;

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
        PlayerController.Instance.SetAnimBool("Drawing", true);

        letterButton.StartHighlight();

        //start playing draw audio
        src.PlayOneShot(drawClip);
    }

    public void SetDamageMult(float mult)
    {
        damageMult = mult;
    }

    //start release animation, end highlight
    public void StartRelease()
    {
        anim.Play("BowRelease");
        PlayerController.Instance.SetAnimBool("Drawing", false);

        //trigger player line renderer
        PlayerController.Instance.ArrowTrail(damageMult);

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

        //get appropriate release sound for damage mult
        src.Stop();
    }

    //bow finished firing, animate out letter
    public void EndRelease()
    {
        letterButton.EndLetter();
    }
}
