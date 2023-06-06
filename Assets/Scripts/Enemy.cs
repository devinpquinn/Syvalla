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
    private Color baseColor;

    //audio stuff
    private EnemyFootstepManager efm;

    //health stuff
    public float maxHP;
    [HideInInspector]
    public float currentHP;

    //camera stuff
    [HideInInspector]
    public Transform camTarget;

    //fx stuff
    public GameObject bloodDrip;
    public GameObject bloodSplash;

    private void Awake()
    {
        state = enemyState.Idle;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        currentHP = maxHP;
        camTarget = transform.Find("Enemy Camera Target");
        baseColor = GetComponent<SpriteRenderer>().color;
        efm = Transform.FindObjectOfType<EnemyFootstepManager>();
    }

    public void Activate()
    {
        //start moving toward player
        state = enemyState.Moving;
        anim.SetBool("Moving", true);
    }

    public void Damage(float mult)
    {
        //damage flash
        StartCoroutine(DoDamage());

        //knockback
        transform.Translate(new Vector3(0.15f * mult, 0, 0));

        //bleed
        Instantiate(bloodDrip, this.transform);
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
            sprite.color = Color.Lerp(Color.red, baseColor, (timer / damageInterval));
            timer += Time.deltaTime;
            yield return null;
        }

        sprite.color = baseColor;
    }

    public void Die()
    {
        //die
        state = enemyState.Dead;
        GetComponent<BoxCollider2D>().enabled = false;

        //death animation
        anim.SetBool("Dead", true);

        //hide blood drips
        foreach(ParticleSystem drip in transform.GetComponentsInChildren<ParticleSystem>())
        {
            drip.emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0.0f, 0f) });
        }

        //end combat
        CombatScript.combat.EndCombat();
    }

    public void BloodSplash()
    {
        //spawn blood splash
        Instantiate(bloodSplash, transform);

        //screenshake
        PlayerController.Instance.ShakeCamera(3, 0.1f);
    }

    public void Attack()
    {
        //attack
        state = enemyState.Attacking;

        //disable combat
        CombatScript.instance.CombatDisabled();

        //kill player
        PlayerController.Instance.state = PlayerController.playerState.Dead;

        //animation
        anim.SetBool("Attacking", true);
    }

    //animation event
    public void Hit()
    {
        PlayerController.Instance.Die();
    }

    //animation events
    public void StepFront()
    {
        efm.PlaySound(true);
    }

    public void StepBack()
    {
        efm.PlaySound(false);
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
