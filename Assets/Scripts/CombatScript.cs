using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public enum CombatState { Waiting, Ready, Drawing };

    public CombatState state;

    public static CombatScript instance;
    public static Combat combat;

    private GameObject combatInterface;

    private Enemy enemy;
    private float baseDamage = 10f;

    public BowHandler bowBrain;
    public BowButtonHandler letterBrain;
    public HealthBarHandler hpBrain;

    private void Awake()
    {
        //singleton
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;

        }

        //variable fetching
        combatInterface = transform.Find("CombatInterface").gameObject;
    }

    public void CombatEnabled()
    {
        state = CombatState.Ready;
        enemy = PlayerController.Instance.enemy;
        combatInterface.SetActive(true);
    }

    public void CombatDisabled()
    {
        state = CombatState.Waiting;

        //hide interface
        combatInterface.SetActive(false);

        //clear text
        PlayerController.Instance.UpdateText("", true);
    }

    private void Update()
    {
        if (PlayerController.Instance.paused)
        {
            return;
        }
        if(state == CombatState.Ready)
        {
            if ((Input.GetKeyDown(KeyCode.W) && letterBrain.currentLetter == "W") 
                || (Input.GetKeyDown(KeyCode.A) && letterBrain.currentLetter == "A")
                || (Input.GetKeyDown(KeyCode.S) && letterBrain.currentLetter == "S")
                || (Input.GetKeyDown(KeyCode.D) && letterBrain.currentLetter == "D")
                || (Input.GetKeyDown(KeyCode.E) && letterBrain.currentLetter == "E")
                || (Input.GetKeyDown(KeyCode.Q) && letterBrain.currentLetter == "Q"))
            {
                //start drawing bow
                bowBrain.StartDraw();
                state = CombatState.Drawing;
            }
        }
        else if (state == CombatState.Drawing)
        {
            if ((Input.GetKeyUp(KeyCode.W) && letterBrain.currentLetter == "W")
                || (Input.GetKeyUp(KeyCode.A) && letterBrain.currentLetter == "A")
                || (Input.GetKeyUp(KeyCode.S) && letterBrain.currentLetter == "S")
                || (Input.GetKeyUp(KeyCode.D) && letterBrain.currentLetter == "D")
                || (Input.GetKeyUp(KeyCode.E) && letterBrain.currentLetter == "E")
                || (Input.GetKeyUp(KeyCode.Q) && letterBrain.currentLetter == "Q"))
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

        //enemy damage animation
        enemy.Damage(mult);

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
            enemy.Die();
        }
        
    }
}
