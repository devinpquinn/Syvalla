using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public enum CombatState { Waiting, Ready, Drawing };

    public CombatState state;

    public static CombatScript combat;

    private Enemy enemy;
    private float baseDamage = 10f;

    public BowHandler bowBrain;
    public BowButtonHandler letterBrain;
    public HealthBarHandler hpBrain;

    private void Awake()
    {
        //singleton
        if (combat != null && combat != this)
        {
            Destroy(this);
        }
        else
        {
            combat = this;

        }
    }

    private void OnEnable()
    {
        state = CombatState.Ready;
        enemy = PlayerController.Instance.enemy;
    }

    private void Update()
    {
        if(state == CombatState.Ready)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                //start drawing bow
                bowBrain.StartDraw();
                state = CombatState.Drawing;
            }
        }
        else if (state == CombatState.Drawing)
        {
            if (Input.GetKeyUp(KeyCode.W))
            {
                //release bow
                bowBrain.StartRelease();
                state = CombatState.Waiting;

                DealDamage(bowBrain.damageMult);
            }
        }
    }

    public void DealDamage(float mult)
    {
        //decrement enemy HP
        float damageDealt = baseDamage * mult;
        enemy.currentHP = Mathf.Max(0, enemy.currentHP - damageDealt);

        //check for death
        if (enemy.currentHP > 0)
        {
            //update health bar
            hpBrain.UpdateHP(enemy.currentHP / enemy.maxHP);
        }
        else
        {
            //kill
            hpBrain.UpdateHP(0);
        }
        
    }
}
