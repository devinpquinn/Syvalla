using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextScroller : MonoBehaviour
{
    //plain line text
    private string rawText = "";
    //tmp
    private TextMeshProUGUI uiText;

    //markup, everything after this tag will be shaded out
    private string richTag = "<color=#272727>";

    //current character index in line
    private int index = 0;

    //how much time between displaying standard characters
    private float timePerChar = 0.035f;

    //decreases every update
    private float timer = 999;

    private void Awake()
    {
        uiText = PlayerController.Instance.bottomText;
    }

    public void NewLine(string line)
    {
        rawText = line;
        uiText.text = richTag + rawText;

        index = 0;
        timer = timePerChar;
    }

    //when a character is advanced, update the text in the UI
    public void AdvanceText()
    {
        index++;
        timer = timePerChar;

        if(index < rawText.Length)
        {
            //display split text
            uiText.text = rawText.Substring(0, index) + richTag + rawText.Substring(index);
        }
        else
        {
            //display full text
            uiText.text = rawText;
        }
    }

    //depending on the index, either display the full text or advance to the next line
    public void Clicked()
    {
        if(index >= rawText.Length)
        {
            //advance to next line
            if(PlayerController.Instance.interaction != null)
            {
                PlayerController.Instance.interaction.Advance();
            }
        }
        else
        {
            //skip to displaying full text;
            uiText.text = rawText;
            index = rawText.Length;
        }
    }

    private void Update()
    {
        if(index < rawText.Length)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                AdvanceText();
            }
        }
    }
}
