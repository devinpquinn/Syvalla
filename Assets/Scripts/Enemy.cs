using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //state stuff
    public enum enemyState { Idle, Moving, Attacking, Dead };

    public enemyState state;

    //movement stuff
    private Rigidbody2D rb;
    private float moveSpeed = 0.7f;
    Vector2 movement = new Vector2(-1, 0);

    //animation stuff
    private Animator anim;

    //health stuff
    public float maxHP;
    [HideInInspector]
    public float currentHP;

    //camera stuff
    [HideInInspector]
    public Transform camTarget;

    private void Awake()
    {
        state = enemyState.Idle;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHP = maxHP;
        camTarget = transform.Find("Enemy Camera Target");
    }

    public void Activate()
    {
        //start moving toward player
        state = enemyState.Moving;
        anim.SetBool("Moving", true);
    }

    public void Die()
    {
        //die
        state = enemyState.Dead;
        Debug.Log("Enemy killed!");
        GetComponent<BoxCollider2D>().enabled = false;

        //death animation


        //end combat
        CombatScript.combat.EndCombat();
    }

    public void Attack()
    {
        //attack
        state = enemyState.Attacking;

        //disable combat
        CombatScript.instance.CombatDisabled();

        //kill player
        PlayerController.Instance.state = PlayerController.playerState.Dead;
        Debug.Log("Player killed!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == enemyState.Moving && collision.CompareTag("Player"))
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        //movement
        if(state == enemyState.Moving)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
