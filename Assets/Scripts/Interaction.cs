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

    public void Interact()
    {
        //lock player
        PlayerController.Instance.state = PlayerController.playerState.Interacting;
        PlayerController.Instance.interaction = this;

        //set header
        if(lines.Count > 1)
        {
            PlayerController.Instance.UpdateText("Click to continue...", true);
        }
        else
        {
            PlayerController.Instance.UpdateText("Click to return.", true);
        }

        //animate out pip
        if(gameObject.GetComponent<InteractPip>() != null)
        {
            gameObject.GetComponent<Animator>().Play("PipSelected");
        }

        //animate camera in
        PlayerController.Instance.CamIn();

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
            if(lines.Count == index + 1)
            {
                //this is the last line
                PlayerController.Instance.UpdateText("Click to return.", true);
            }
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
            PlayerController.Instance.CamOut();

            //call event
            onEnd.Invoke();

            //cleanup object
            this.gameObject.SetActive(false);
        }
    }
}
