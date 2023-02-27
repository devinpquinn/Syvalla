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
    private SpriteRenderer sprite;
    private float damageInterval = 0.5f;

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
        sprite = GetComponent<SpriteRenderer>();
        currentHP = maxHP;
        camTarget = transform.Find("Enemy Camera Target");
    }

    public void Activate()
    {
        //start moving toward player
        state = enemyState.Moving;
        anim.SetBool("Moving", true);
    }

    public void Damage()
    {
        //damage animation
        StartCoroutine(DoDamage());
    }

    IEnumerator DoDamage()
    {
        sprite.color = Color.red;

        float timer = 0f;

        float stayRed = 0.15f;

        while(timer < stayRed)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;

        while(timer < damageInterval)
        {
            sprite.color = Color.Lerp(Color.red, Color.white, (timer / damageInterval));
            timer += Time.deltaTime;
            yield return null;
        }

        sprite.color = Color.white;
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
