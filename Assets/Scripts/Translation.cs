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
    public UnityEvent onCloseNormal;

    //if applicable, an event triggered when the player closes the interface with a keyword unscrambled
    public UnityEvent onCloseSpecial;

    //displayed as bottom text while decoding
    public string description;

    public void Display()
    {

    }

    public void Hide()
    {

    }
}
