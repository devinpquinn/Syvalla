using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    //state stuff
    public enum playerState { Normal, Turning, Interacting, Translating, Petting, Fighting, Dead, Locked };

    public playerState state;

    //movement stuff
    private Rigidbody2D rb;
    public float moveSpeed = 2f;
    Vector2 movement = Vector2.zero;

    //animation stuff
    private Animator anim;

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
    private float camTightSize = 7f;
    private float camWideSize = 8f;

    //combat stuff
    [HideInInspector]
    public Enemy enemy;

    //translation stuff
    [HideInInspector]
    public Translation translation;
    public DecodeScript decodeInterface;
    private TextMeshProUGUI decodeText;

    //turnback stuff
    public GameObject turnbackDisplay;

    //storage and retrieval stuff
    private static PlayerController player;
    public static PlayerController Instance { get { return player; } }

    private void Awake()
    {
        //singleton
        if(player != null && player != this)
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

        anim = GetComponent<Animator>();

        scroller = bottomText.gameObject.GetComponent<TextScroller>();

        decodeText = decodeInterface.transform.Find("DecodePanel").Find("DecodeText").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        //animation variables
        anim.SetBool("Moving", false);

        if (state == playerState.Normal)
        {
            //movement vector
            if(Input.GetKey(KeyCode.D))
            {
                movement.x = 1;
                anim.SetBool("Moving", true);
            }
            else
            {
                movement.x = 0;
            }

            //interaction trigger
            if(Input.GetKeyDown(KeyCode.W))
            {
                if(interaction != null)
                {
                    interaction.Interact();
                }
                else if(translation != null)
                {
                    translation.StartTranslation();
                }
            }

            //turnback trigger
            if(Input.GetKeyDown(KeyCode.A))
            {
                turnbackDisplay.SetActive(true);
                state = playerState.Turning;
            }
        }
        else if(state == playerState.Interacting)
        {
            if((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space)) && interaction != null)
            {
                //advance interaction
                scroller.Clicked();
            }
        }
        else if(state == playerState.Translating)
        {
            if(Input.GetKeyDown(KeyCode.Q) && translation != null)
            {
                //return from translation
                translation.EndTranslation();
            }
        }
        else if(state == playerState.Turning)
        {
            if(Input.GetKeyUp(KeyCode.A))
            {
                turnbackDisplay.SetActive(false);
                state = playerState.Normal;
            }
        }
    }

    private void FixedUpdate()
    {
        if(state == playerState.Normal)
        {
            //player movement
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void UpdateText(string line, bool top = false)
    {
        //check for events
        if(line.StartsWith("{"))
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
        if(top)
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

        anim.SetBool("Translating", true);
    }

    public void EndTranslation()
    {
        state = PlayerController.playerState.Normal;
        translation = null;
        anim.SetBool("Translating", false);
    }

    public void SetupCombat()
    {
        CombatScript.instance.CombatEnabled();
        UpdateText("Hold and release the button shown below.", true);
    }

    public void SetAnimBool(string key, bool value)
    {
        anim.SetBool(key, value);
    }

    public void CamIn()
    {
        ResizeCam(camTightSize);
    }

    public void CamOut()
    {
        ResizeCam(camWideSize);
    }

    public void CamEngage(Transform enemy, float weight = 1)
    {
        //widen camera from combat framing
        CamOut();

        //add enemy to camera
        AddToCamera(enemy, weight, camWideSize);
    }

    public void CamDisengage()
    {
        //remove enemy from target group
        targetGroup.RemoveMember(targetGroup.m_Targets[1].target);
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
