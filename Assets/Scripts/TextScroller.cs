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

    //audio stuff
    private AudioSource textSource;

    public AudioClip letterSound;
    public AudioClip skipSound;
    private string silentChars = ",.-?!;:()[]{}<>*& ";

    private void Awake()
    {
        uiText = GetComponent<TextMeshProUGUI>();
        textSource = GetComponent<AudioSource>();
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

            //check next character
            string addedChar = rawText.Substring(index - 1, 1);
            string nextChar = rawText.Substring(index, 1);

            //check for rich text tag
            if (addedChar == "<")
            {
                if(nextChar == "/")
                {
                    index++;
                }
                index += 2;

                //refresh characters
                addedChar = rawText.Substring(index - 1, 1);
                if(index < rawText.Length)
                {
                    nextChar = rawText.Substring(index, 1);
                }
            }

            //check if added character signals a pause
            if((addedChar == "." && nextChar != ".") || addedChar == "!" || addedChar == "?")
            {
                timer += 0.25f;
            }
            else if (addedChar == ",")
            {
                timer += 0.1f;
            }
            else if (addedChar == ";" || addedChar == ":" || rawText.Substring(0, index).EndsWith("--"))
            {
                timer += 0.2f;
            }
            
            //check next character
            if(nextChar == "'" || nextChar == "\"" || nextChar == ")" || nextChar == "]")
            {
                timer = timePerChar;
            }

            uiText.text = rawText.Substring(0, index) + richTag + rawText.Substring(index);

            //letter audio
            if (!silentChars.Contains(addedChar))
            {
                //randomly vary pitch
                float basePitch = 1f;

                float margin = 0.06f;
                float amount = Random.Range(basePitch - margin, basePitch + margin);

                textSource.pitch = amount;
                textSource.PlayOneShot(letterSound);
            }
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
                //advance
                PlayerController.Instance.interaction.Advance();
            }
        }
        else
        {
            //skip to displaying full text;
            uiText.text = rawText;
            index = rawText.Length;

            //play audio
            textSource.pitch = 1;
            textSource.PlayOneShot(skipSound);
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
