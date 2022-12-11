using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    //state stuff
    public enum playerState { Normal, Interacting, Translating };

    public playerState state;

    //movement stuff
    private Rigidbody2D rb;
    [HideInInspector]
    public float moveSpeed = 1.5f;
    Vector2 movement = Vector2.zero;

    //interaction stuff
    [HideInInspector]
    public Interaction interaction;
    public TextMeshProUGUI topText;
    public TextMeshProUGUI bottomText;
    [HideInInspector]
    public TextScroller scroller;

    //interaction camera stuff
    private CinemachineVirtualCamera vcam;
    private float camDefaultSize = 5;
    private float camCloseSize = 4f;

    //the coroutine currently running on the camera
    private Coroutine camRoutine = null;

    //translation stuff
    [HideInInspector]
    public Translation translation;
    public DecodeScript decodeInterface;
    private TextMeshProUGUI decodeText;

    //storage and retrieval stuff
    private static PlayerController player;
    public static PlayerController Instance { get { return player; } }

    private void Awake()
    {
        //singleton
        if (player != null && player != this)
        {
            Destroy(gameObject);
        }
        else
        {
            player = this;

        }

        //variable fetching and setting
        rb = GetComponent<Rigidbody2D>();
        vcam = Camera.main.GetComponentInChildren<CinemachineVirtualCamera>();

        scroller = bottomText.gameObject.GetComponent<TextScroller>();

        decodeText = decodeInterface.transform.Find("DecodePanel").Find("DecodeText").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if(state == playerState.Normal)
        {
            //movement vector
            if (Input.GetKey(KeyCode.D))
            {
                movement.x = 1;
            }
            else
            {
                movement.x = 0;
            }

            //interaction trigger
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (interaction != null)
                {
                    interaction.Interact();
                }
                else if (translation != null)
                {
                    translation.StartTranslation();
                }
            }
        }
        else if (state == playerState.Interacting)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) && interaction != null)
            {
                //advance interaction
                scroller.Clicked();
            }
        }
        else if(state == playerState.Translating)
        {
            if (Input.GetKeyDown(KeyCode.Q) && translation != null)
            {
                //return from translation
                translation.EndTranslation();
            }
        }
    }

    private void FixedUpdate()
    {
        if (state == playerState.Normal)
        {
            //player movement
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void UpdateText(string line, bool top = false)
    {
        //check for events
        if (line.StartsWith("{"))
        {
            //read key
            int key = int.Parse(line.Substring(1, 1));

            //edit line
            line = line.Substring(3);

            //call event
            if(interaction != null)
            {
                interaction.inLine[key].Invoke();
            }
        }

        //set text
        if (top)
        {
            topText.text = line;
        }
        else
        {
            scroller.NewLine(line);
        }
    }

    public void ClearText()
    {
        topText.text = "";
        bottomText.text = "";
    }

    public void SetupTranslation(string message)
    {
        decodeText.text = message;
        decodeInterface.gameObject.SetActive(true);

        state = playerState.Translating;
    }

    public void CamIn()
    {
        //abort any ongoing camera coroutine
        if(camRoutine != null)
        {
            StopCoroutine(camRoutine);
        }

        //zoom the camera in
        camRoutine = StartCoroutine(LerpCameraSize(camCloseSize, 0.7f));
    }

    public void CamOut()
    {
        //abort any ongoing camera coroutine
        if (camRoutine != null)
        {
            StopCoroutine(camRoutine);
        }

        //zoom the camera out
        camRoutine = StartCoroutine(LerpCameraSize(camDefaultSize, 0.8f));
    }

    public IEnumerator LerpCameraSize(float targetSize, float transitionTime = 1f)
    {
        if(vcam.m_Lens.OrthographicSize != targetSize)
        {
            float startSize = vcam.m_Lens.OrthographicSize;
            float timer = 0;

            while (timer < transitionTime)
            {
                timer += Time.fixedDeltaTime;
                vcam.m_Lens.OrthographicSize = Mathf.SmoothStep(startSize, targetSize, timer / transitionTime);
                yield return new WaitForFixedUpdate();
            }

            vcam.m_Lens.OrthographicSize = targetSize;
        }
        
        yield return null;
    }
}
