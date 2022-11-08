using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //state stuff
    public enum playerState { Normal, Interacting };

    public playerState state;

    //movement stuff
    private Rigidbody2D rb;
    [HideInInspector]
    public float moveSpeed = 2f;
    Vector2 movement = Vector2.zero;

    //interaction stuff
    [HideInInspector]
    public Interaction interaction;
    public TextMeshProUGUI topText;
    public TextMeshProUGUI bottomText;

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
            }
        }
        else if (state == playerState.Interacting)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) && interaction != null)
            {
                //advance interaction
                interaction.Advance();
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
            bottomText.text = line;
        }
    }

    public void ClearText()
    {
        topText.text = "";
        bottomText.text = "";
    }
}
