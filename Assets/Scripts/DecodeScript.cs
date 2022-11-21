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

        //check for scroll
        if (hoverWord != -1)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            
            if(scroll > 0)
            {
                //scrub letters positive
                ScrubWord(hoverWord);
            }
            else if (scroll < 0)
            {
                //scrub letters negative
                ScrubWord(hoverWord, false);
            }
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

    private void ScrubWord(int index, bool pos = true)
    {
        string oldWord = decodeText.textInfo.wordInfo[index].GetWord();
        string newWord = "";

        if (pos)
        {
            for (int i = 0; i < oldWord.Length; i++)
            {
                char subj = oldWord[i];
                if (subj.Equals('Z'))
                {
                    subj = 'A';
                }
                else
                {
                    subj = (char)(((int)subj) + 1);
                }
                newWord += subj;
            }
        }
        else
        {
            for (int i = 0; i < oldWord.Length; i++)
            {
                char subj = oldWord[i];
                if (subj.Equals('A'))
                {
                    subj = 'Z';
                }
                else
                {
                    subj = (char)(((int)subj) - 1);
                }
                newWord += subj;
            }
        }

        //update rawText
        rawText = rawText.Substring(0, decodeText.textInfo.wordInfo[index].firstCharacterIndex) + newWord + rawText.Substring(decodeText.textInfo.wordInfo[index].lastCharacterIndex + 1);

        //update decodeText and re-highlight
        string highlightString = ColorUtility.ToHtmlStringRGB(ColorSetter.colorSetter.InvertedColor());
        decodeText.text = rawText.Substring(0, decodeText.textInfo.wordInfo[index].firstCharacterIndex) + "<color=#" + highlightString + ">" + newWord + "</color>" + rawText.Substring(decodeText.textInfo.wordInfo[index].lastCharacterIndex + 1);
    }
}
