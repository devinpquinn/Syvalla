using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DecodeScript : MonoBehaviour
{
    public TMP_Text decodeText;

    [HideInInspector]
    public string rawText;

    private int lastHovered = -1;

    //blocks mouse interaction
    private bool locked = true;

    private void OnEnable()
    {
        StartCoroutine(DoScramble());
    }

    IEnumerator DoScramble()
    {
        yield return new WaitForEndOfFrame();
        InitialScramble();
    }

    private void Update()
    {
        if (!locked)
        {
            //find hovered word
            int hoverWord = TMP_TextUtilities.FindIntersectingWord(decodeText, Input.mousePosition, Camera.main);

            //check if there is a change in hover state:
            if (hoverWord != lastHovered)
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

                if (scroll > 0)
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
    }

    private void HighlightWord(int index)
    {
        //add rich text color tags to word at index

        //clear tags in case we are switching from one word to another
        decodeText.text = rawText;

        //text of target word
        string targetWord = decodeText.textInfo.wordInfo[index].GetWord();

        //update text with rich text tags
        decodeText.text = decodeText.text.Substring(0, decodeText.textInfo.wordInfo[index].firstCharacterIndex) + "<color=white>" + targetWord + "</color>" + decodeText.text.Substring(decodeText.textInfo.wordInfo[index].lastCharacterIndex + 1);
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

        decodeText.text = rawText.Substring(0, decodeText.textInfo.wordInfo[index].firstCharacterIndex) + "<color=white>" + newWord + "</color>" + rawText.Substring(decodeText.textInfo.wordInfo[index].lastCharacterIndex + 1);
    }

    private string ScrambleWord(string word)
    {
        string newWord = "";

        for (int i = 0; i < word.Length; i++)
        {
            char subj = word[i];
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

        return newWord;
    }

    private void InitialScramble()
    {
        rawText = decodeText.text;

        for(int i = 0; i < decodeText.textInfo.wordCount; i++)
        {
            string targetWord = decodeText.textInfo.wordInfo[i].GetWord();
            int scrambles = Random.Range(1, 26);

            while(scrambles > 0)
            {
                targetWord = ScrambleWord(targetWord);
                scrambles--;
            }

            rawText = rawText.Substring(0, decodeText.textInfo.wordInfo[i].firstCharacterIndex) + targetWord + rawText.Substring(decodeText.textInfo.wordInfo[i].lastCharacterIndex + 1);
        }

        decodeText.text = rawText;
    }

    public void Lock()
    {
        locked = true;
    }

    public void Unlock()
    {
        locked = false;
    }

    //called from animation event
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
