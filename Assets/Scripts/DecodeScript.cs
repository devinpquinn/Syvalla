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
    [HideInInspector]
    public List<string> decodedWords = new List<string>(); //list of words that have been unscrambled at some point (for checking events)

    private int lastHovered = -1;

    //animation
    private Animator anim;

    //blocks mouse interaction
    private bool locked = true;

    //particle effect
    public GameObject blood;
    private Transform particleFloor;

    //audio
    public AudioSource src;
    public AudioClip scrollSound;
    public AudioClip correctSound;
    public AudioClip closeSound;
    public AudioClip solvedSound;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        particleFloor = decodeText.transform.parent.Find("BloodFloor");
    }

    private void OnEnable()
    {
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
                    //scrub letters negative
                    ScrubWord(hoverWord, false);
                }
                else if (scroll < 0)
                {
                    //scrub letters positive
                    ScrubWord(hoverWord);
                }
            }
        }
    }

    private void UpdateTextHighlights(int highlightedWord = -1, bool scrolled = false)
    {
        //check if each word is decoded and set color accordingly
        decodeText.text = rawText;
        List<int> correctWords = new List<int>();

        TMP_TextInfo myInfo = decodeText.GetTextInfo(rawText);

        //find correct words
        for(int i = 0; i < myInfo.wordCount; i++)
        {
            if (CheckWord(i))
            {
                correctWords.Add(i);
            }
        }

        //construct new string to paste into UI
        string construct = "";

        for (int i = 0; i < myInfo.wordCount; i++)
        {
            //check for leading characters
            string leading = "";
            if(i > 0)
            {
                int previousWordEndIndex = myInfo.wordInfo[i - 1].lastCharacterIndex + 1;
                leading = rawText.Substring(previousWordEndIndex, (myInfo.wordInfo[i].firstCharacterIndex - previousWordEndIndex));
            }
            construct += leading;

            //add either plain text of word or highlight-tagged text of word
            string thisWord = myInfo.wordInfo[i].GetWord();
            if (highlightedWord == i)
            {
                if (correctWords.Contains(i))
                {
                    //highlight red
                    thisWord = "<color=red>" + thisWord + "</color>";

                    //check if we just scrolled to a correct word
                    if (scrolled)
                    {
                        //play panel animation
                        anim.Play("TranslationPulse", -1, 0);

                        //find center of word
                        TMP_WordInfo myWord = myInfo.wordInfo[i];
                        Vector3 wordBottomLeft = myInfo.characterInfo[myWord.firstCharacterIndex].bottomLeft;
                        Vector3 wordTopRight = myInfo.characterInfo[myWord.lastCharacterIndex].topRight;
                        Vector3 wordCenter = Vector3.Lerp(wordBottomLeft, wordTopRight, 0.5f);

                        //spawn blood
                        GameObject myBlood = Instantiate(blood, decodeText.transform.parent);
                        myBlood.transform.localPosition = wordCenter;

                        //set particle system properties
                        ParticleSystem myParticles = myBlood.GetComponent<ParticleSystem>();
                        int myLetters = myWord.characterCount;

                        ParticleSystem.ShapeModule myShape = myParticles.shape;
                        myShape.scale = new Vector3((float)0.6f * myLetters, 0.75f, 1);

                        myParticles.emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0.0f, 6 * myLetters) });

                        myParticles.collision.AddPlane(particleFloor);

                        //pause for emphasis
                        StartCoroutine(LockPause(0.5f));

                        //play success scrolling sound
                        src.PlayOneShot(correctSound);

                        //add to decodedWords
                        string solvedWord = myInfo.wordInfo[i].GetWord();
                        if (!decodedWords.Contains(solvedWord))
                        {
                            decodedWords.Add(solvedWord);
                        }

                        //check if all words are decoded
                        if(decodedWords.Count == myInfo.wordCount)
                        {
                            anim.Play("TranslationSolved");
                            //src.PlayOneShot(solvedSound);
                        }
                    }
                }
                else
                {
                    //highlight white
                    thisWord = "<color=white>" + thisWord + "</color>";

                    if (scrolled)
                    {
                        //play normal scrolling sound
                        src.clip = scrollSound;
                        src.Play();
                    }
                }
            }
            else if (correctWords.Contains(i))
            {
                thisWord = "<color=#800000>" + thisWord + "</color>";
            }
            construct += thisWord;

            //if this is the last word, check for trailing characters
            if(i == myInfo.wordCount - 1)
            {
                construct += rawText.Substring(myInfo.wordInfo[i].lastCharacterIndex + 1);
            }

            decodeText.text = construct;
        }
    }

    IEnumerator LockPause(float duration)
    {
        locked = true;
        yield return new WaitForSeconds(duration);
        locked = false;
    }

    private void ScrubWord(int index, bool pos = true)
    {
        //check if already decoded
        if (CheckWord(index))
        {
            return;
        }

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

        //update text
        rawText = rawText.Substring(0, decodeText.textInfo.wordInfo[index].firstCharacterIndex) + newWord + rawText.Substring(decodeText.textInfo.wordInfo[index].lastCharacterIndex + 1);
        UpdateTextHighlights(index, true);
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
        decodedWords = new List<string>();

        TMP_TextInfo myInfo = decodeText.GetTextInfo(rawText);

        for(int i = 0; i < myInfo.wordCount; i++)
        {
            string targetWord = myInfo.wordInfo[i].GetWord();
            int scrambles = Random.Range(1, 26);

            while(scrambles > 0)
            {
                targetWord = ScrambleWord(targetWord);
                scrambles--;
            }

            rawText = rawText.Substring(0, myInfo.wordInfo[i].firstCharacterIndex) + targetWord + rawText.Substring(myInfo.wordInfo[i].lastCharacterIndex + 1);
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

        //show cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    //called from animation event
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void AnimateOut()
    {
        if (decodedWords.Count == decodeText.GetTextInfo(rawText).wordCount)
        {
            //animate out from complete
            anim.Play("TranslationClose");
        }
        else
        {
            //animate out from incomplete
            anim.Play("TranslationDisappear");
        }
    }
}
