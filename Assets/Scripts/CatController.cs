using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    //state stuff
    public enum catState { Idle, Moving };

    public catState state;

    //movement stuff
    private Rigidbody2D rb;
    private float moveSpeed = 2f;
    Vector2 movement = new Vector2(1, 0);

    //animation stuff
    private Animator anim;

    //distance triggers
    [HideInInspector]
    public float minOffset = 1.5f;
    [HideInInspector]
    public float maxOffset = 2f;

    //storage and retrieval stuff
    private static CatController cat;
    public static CatController Instance { get { return cat; } }

    private void Awake()
    {
        //singleton
        if (cat != null && cat != this)
        {
            Destroy(gameObject);
        }
        else
        {
            cat = this;

        }

        //variable fetching and setting
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        state = catState.Idle;
    }


    private void FixedUpdate()
    {
        if (state == catState.Moving)
        {
            //check distance to see if we should stop moving
            if (PlayerController.Instance.transform.position.x - transform.position.x > minOffset)
            {
                //movement
                rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                //we've caught up
                state = catState.Idle;
                anim.SetBool("Moving", false);
            }

        }
        else if (state == catState.Idle)
        {
            //check distance to see if we should start moving
            if (PlayerController.Instance.transform.position.x - transform.position.x > maxOffset)
            {
                //start moving
                moveSpeed = PlayerController.Instance.moveSpeed;
                state = catState.Moving;
                anim.SetBool("Moving", true);
            }
        }
    }

    public void SetDanger(bool dangerous)
    {
        anim.SetBool("Danger", dangerous);
    }
}