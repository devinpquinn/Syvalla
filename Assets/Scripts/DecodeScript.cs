using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DecodeScript : MonoBehaviour
{
    public TMP_Text decodeText;

    [HideInInspector]
    public string rawText; //plain text of text box
    private string trueText; //stores text of decoded message for comparison

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
                    UpdateTextHighlights(hoverWord);
                }

                //if ending highlight:
                else
                {
                    UpdateTextHighlights();
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

    private void UpdateTextHighlights(int highlightedWord = -1)
    {
        //check if each word is decoded and set color accordingly
        decodeText.text = rawText;
        List<int> correctWords = new List<int>();

        //find correct words
        for(int i = 0; i < decodeText.textInfo.wordCount; i++)
        {
            if (CheckWord(i))
            {
                correctWords.Add(i);
            }
        }

        //construct new string to paste into UI
        string construct = "";

        for (int i = 0; i < decodeText.textInfo.wordCount; i++)
        {
            //check for leading characters
            string leading = "";
            if(i > 0)
            {
                int previousWordEndIndex = decodeText.textInfo.wordInfo[i - 1].lastCharacterIndex + 1;
                leading = rawText.Substring(previousWordEndIndex, (decodeText.textInfo.wordInfo[i].firstCharacterIndex - previousWordEndIndex));
            }
            construct += leading;

            //add either plain text of word or highlight-tagged text of word
            string thisWord = decodeText.textInfo.wordInfo[i].GetWord();
            if(highlightedWord == i)
            {
                if (correctWords.Contains(i))
                {
                    //highlight red
                    thisWord = "<color=red>" + thisWord + "</color>";
                }
                else
                {
                    //highlight white
                    thisWord = "<color=white>" + thisWord + "</color>";
                }
            }
            else if(correctWords.Contains(i))
            {
                thisWord = "<color=#800000>" + thisWord + "</color>";
            }
            construct += thisWord;

            //if this is the last word, check for trailing characters
            if(i == decodeText.textInfo.wordCount - 1)
            {
                construct += rawText.Substring(decodeText.textInfo.wordInfo[i].lastCharacterIndex + 1);
            }

            decodeText.text = construct;
        }
    }

    private void HighlightWord(int index)
    {
        //add rich text color tags to word at index

        //clear tags in case we are switching from one word to another
        decodeText.text = rawText;

        //text of target word
        string targetWord = decodeText.textInfo.wordInfo[index].GetWord();

        //check if we have highlighted a decoded word by comparing substrings of rawText and trueText
        string colorTag = "white";
        if (CheckWord(index))
        {
            colorTag = "red";
        }

        //update text with rich text tags
        decodeText.text = decodeText.text.Substring(0, decodeText.textInfo.wordInfo[index].firstCharacterIndex) + "<color=" + colorTag + ">" + targetWord + "</color>" + decodeText.text.Substring(decodeText.textInfo.wordInfo[index].lastCharacterIndex + 1);
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

        //check if we have scrubbed to a decoded word by comparing substrings of rawText and trueText
        string colorTag = "white";
        if(CheckWord(index))
        {
            colorTag = "red";
        }

        decodeText.text = rawText.Substring(0, decodeText.textInfo.wordInfo[index].firstCharacterIndex) + "<color=" + colorTag + ">" + newWord + "</color>" + rawText.Substring(decodeText.textInfo.wordInfo[index].lastCharacterIndex + 1);
    }

    private bool CheckWord(int index)
    {
        if (rawText.Substring(decodeText.textInfo.wordInfo[index].firstCharacterIndex, decodeText.textInfo.wordInfo[index].characterCount) == trueText.Substring(decodeText.textInfo.wordInfo[index].firstCharacterIndex, decodeText.textInfo.wordInfo[index].characterCount))
        {
            return true;
        }
        else
        {
            return false;
        }
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
        trueText = rawText;

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
