using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //state stuff
    public enum playerState { Normal, Locked };

    public playerState state;

    //movement stuff
    private Rigidbody2D rb;
    public float moveSpeed = 3f;
    Vector2 movement = Vector2.zero;

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
    }

    private void FixedUpdate()
    {
        if (state == playerState.Normal)
        {
            //player movement
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
