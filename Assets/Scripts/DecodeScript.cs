using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DecodeScript : MonoBehaviour
{
    public TMP_Text decodeText;

    private int lastHovered = -1;

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
                //text of word highlighted
                string targetWord = decodeText.textInfo.wordInfo[hoverWord].GetWord();

                //highlight color hexcode
                string highlightString = ColorUtility.ToHtmlStringRGB(ColorSetter.colorSetter.InvertedColor());
            }

            //if ending highlight:
            else
            {

            }

            lastHovered = hoverWord;
        }
    }
}
