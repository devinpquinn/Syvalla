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
    private float moveSpeed = 0.5f;
    Vector2 movement = new Vector2(-1, 0);

    //health stuff
    public float maxHP = 100f;
    [HideInInspector]
    public float currentHP;

    //camera stuff
    [HideInInspector]
    public Transform camTarget;

    private void Awake()
    {
        state = enemyState.Idle;
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
        camTarget = transform.Find("Enemy Camera Target");
    }

    public void Activate()
    {
        //start moving toward player
        state = enemyState.Moving;
    }

    public void Die()
    {
        state = enemyState.Dead;
    }

    public void Attack()
    {
        state = enemyState.Attacking;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Attack();
            PlayerController.Instance.Die();
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
