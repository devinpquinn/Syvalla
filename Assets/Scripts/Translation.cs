using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Translation : MonoBehaviour
{
    //the message to be decoded
    [TextArea]
    public string messageText;

    //a word or words that, if unscrambled when the player closes the interface, triggers an event
    public List<string> keywords;

    //if applicable, an event triggered when the player closes the interface
    public UnityEvent onEndNormal;

    //if applicable, an event triggered when the player closes the interface with a keyword unscrambled
    public UnityEvent onEndSpecial;

    //displayed as bottom text while decoding
    public string description;

    public void StartTranslation()
    {
        //lock player
        PlayerController.Instance.state = PlayerController.playerState.Translating;
        PlayerController.Instance.translation = this;

        //set header and footer
        PlayerController.Instance.UpdateText("Use scroll wheel to translate. Press 'Q' to return.", true);
        PlayerController.Instance.UpdateText(description);

        //animate out pip
        if (gameObject.GetComponent<InteractPip>() != null)
        {
            gameObject.GetComponent<Animator>().Play("PipSelected");
        }

        //animate camera in
        PlayerController.Instance.CamIn();

        //trigger translation interface through player script
        PlayerController.Instance.SetupTranslation(messageText);
    }

    public void EndTranslation()
    {
        //animate out interface
        PlayerController.Instance.decodeInterface.gameObject.GetComponent<Animator>().Play("TranslationDisappear");

        //clear text
        PlayerController.Instance.ClearText();

        //reset state
        PlayerController.Instance.state = PlayerController.playerState.Normal;
        PlayerController.Instance.translation = null;

        //animate camera out
        PlayerController.Instance.CamOut();

        //check for event(s)
        if(onEndSpecial != null)
        {
            string decodedText = PlayerController.Instance.decodeInterface.rawText;

            foreach(string key in keywords)
            {
                if (decodedText.Contains(key))
                {
                    //trigger special event
                    onEndSpecial.Invoke();
                    break;
                }
            }

            //if special event conditions not met, still check normal event
            if(onEndNormal != null)
            {
                //trigger normal event
                onEndNormal.Invoke();
            }
        }
        else if(onEndNormal != null)
        {
            //trigger normal event
            onEndNormal.Invoke();
        }

        //cleanup object
        gameObject.SetActive(false);
    }
}
