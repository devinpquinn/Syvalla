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
    private bool gettingPet = false;
    private float minOffset = 1.5f;
    private float maxOffset = 2f;
    private float petOffset = 1f;

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

    public void SetPettingState(bool petting)
    {
        gettingPet = petting;
    }

    private void FixedUpdate()
    {
        if (state == catState.Moving)
        {
            //check distance to see if we should stop moving
            if (PlayerController.Instance.transform.position.x - transform.position.x > (gettingPet ? petOffset : minOffset))
            {
                //movement
                rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                //we've caught up
                state = catState.Idle;
                anim.SetBool("Moving", false);

                //set player proximity notice
                PlayerController.Instance.SetAnimBool("CatNearby", true);
            }

        }
        else if (state == catState.Idle)
        {
            //check distance to see if we should start moving
            if (PlayerController.Instance.transform.position.x - transform.position.x > (gettingPet ? petOffset : maxOffset))
            {
                //start moving
                moveSpeed = PlayerController.Instance.moveSpeed;
                state = catState.Moving;
                anim.SetBool("Moving", true);

                //set player proximity notice
                PlayerController.Instance.SetAnimBool("CatNearby", false);
            }
        }
    }

    public void SetDanger(bool dangerous)
    {
        anim.SetBool("Danger", dangerous);
    }
}