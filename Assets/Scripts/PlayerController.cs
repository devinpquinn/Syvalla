using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //state stuff
    public enum playerState { Normal, Locked };

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
        if (Input.GetKey(KeyCode.W))
        {
            if(interaction != null)
            {
                interaction.Interact();
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
        if (top)
        {
            topText.text = line;
        }
        else
        {
            bottomText.text = line;
        }
    }
}
