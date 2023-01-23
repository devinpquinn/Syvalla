using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    //state stuff
    public enum playerState { Normal, Interacting, Translating, Fighting, Dead };

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
    public CinemachineTargetGroup targetGroup;

    //camera transition values
    private float camTightSize = 3f;
    private float camWideSize = 4f;

    //combat stuff
    public GameObject combatInterface;
    [HideInInspector]
    public Enemy enemy;

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

    public void SetupCombat()
    {
        combatInterface.SetActive(true);
        UpdateText("Hold and release the button shown below.", true);
    }

    public void Die()
    {
        //deactivate
        state = playerState.Dead;

        //animate out combat UI
    }

    public void CamIn()
    {
        ResizeCam(camTightSize);
    }

    public void CamOut()
    {
        ResizeCam(camWideSize);
    }

    public void CamEngage(Transform enemy, float weight = 1, float radius = 4)
    {
        //widen camera from combat framing
        CamOut();

        //add enemy to camera
        AddToCamera(enemy, weight, radius);
    }

    public void CamDisengage()
    {
        //tighten camera to interaction framing

        //remove enemy from target group

        //remember to reset damping values at end of interaction
    }

    public void AddToCamera(Transform t, float weight, float radius)
    {
        targetGroup.AddMember(t, weight, radius);
    }

    public void RemoveFromCamera(Transform t)
    {
        targetGroup.RemoveMember(t);
    }

    public void ResizeCam(float targetSize)
    {
        //resize player camera radius
        targetGroup.m_Targets[0].radius = targetSize;
    }

}
