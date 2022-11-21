using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DecodeScript : MonoBehaviour
{
    public TMP_Text decodeText;

    private string rawText;

    private int lastHovered = -1;

    private void Awake()
    {
        rawText = decodeText.text;
    }

    private void Update()
    {
        //find hovered word
        int hoverWord = TMP_TextUtilities.FindIntersectingWord(decodeText, Input.mousePosition, Camera.main);

        //check if there is a change in hover state:
        if(hoverWord != lastHovered)
        {
            //if starting highlight:
            if (hoverWord != -1)
            {
                HighlightWord(hoverWord);
            }

            //if ending highlight:
            else
            {
                decodeText.text = rawText;
            }

            lastHovered = hoverWord;
        }
    }

    private void HighlightWord(int index)
    {
        //add rich text color tags to word at index

        //clear tags in case we are switching from one word to another
        decodeText.text = rawText;

        //text of target word
        string targetWord = decodeText.textInfo.wordInfo[index].GetWord();

        //hexcode of highlight color
        string highlightString = ColorUtility.ToHtmlStringRGB(ColorSetter.colorSetter.InvertedColor());

        //update text with rich text tags
        decodeText.text = decodeText.text.Substring(0, decodeText.textInfo.wordInfo[index].firstCharacterIndex) + "<color=#" + highlightString + ">" + targetWord + "</color>" + decodeText.text.Substring(decodeText.textInfo.wordInfo[index].lastCharacterIndex + 1);
    }
}
