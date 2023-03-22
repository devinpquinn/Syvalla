using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    [TextArea]
    public List<string> lines; //the text blocks that make up this event
    private int index = 0;

    public UnityEvent onStart; //is called immediately upon beginning this interaction
    public UnityEvent onEnd; //is called when the player clicks through the last dialogue block

    public List<UnityEvent> inLine; //events called on specific lines

    //disable default camera transitions
    public bool skipCamIn = false;
    public bool skipCamOut = false;

    public void Interact()
    {
        //lock player
        PlayerController.Instance.state = PlayerController.playerState.Interacting;
        PlayerController.Instance.interaction = this;

        //set header
        PlayerController.Instance.UpdateText("", true);

        //animate out pip
        if (gameObject.GetComponent<InteractPip>() != null)
        {
            gameObject.GetComponent<Animator>().SetBool("Selected", true);
        }

        //animate camera in
        if (!skipCamIn)
        {
            PlayerController.Instance.CamIn();
        }

        //start playing text
        PlayerController.Instance.UpdateText(lines[0]);

        onStart.Invoke();
    }

    public void Advance()
    {
        index++;

        //get next line of dialogue, check for events or end of interaction
        if(lines.Count > index)
        {
            //continue dialogue
            PlayerController.Instance.UpdateText(lines[index]);
        }
        else
        {
            //end dialogue

            //clear text
            PlayerController.Instance.ClearText();

            //reset state
            PlayerController.Instance.state = PlayerController.playerState.Normal;
            PlayerController.Instance.interaction = null;

            //animate camera out
            if (!skipCamOut)
            {
                PlayerController.Instance.CamOut();
            }

            //call event
            onEnd.Invoke();

            //cleanup object
            gameObject.SetActive(false);
        }
    }
}
