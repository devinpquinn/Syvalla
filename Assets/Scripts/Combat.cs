using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public Interaction interactBefore;
    public Interaction interactAfter;

    private Enemy myEnemy;

    private void Awake()
    {
        myEnemy = GetComponentInChildren<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerController.Instance.state == PlayerController.playerState.Normal)
            {
                //initiate pre-combat interaction
                interactBefore.Interact();

                //alert cat
                CatController.Instance.SetDanger(true);

                //fade out music
                MusicManager.EaseMusic(1, false);
            }
        }
    }

    public void Danger(bool value)
    {
        PlayerController.Instance.SetAnimBool("Danger", value);

        //player draws or stows bow
        PlayerController.Instance.bowSound.Stow(!value);
    }

    public void StartCombatCamera()
    {
        //camera transition from interaction framing to combat framing
        PlayerController.Instance.CamEngage(myEnemy.camTarget);
    }

    public void EndCombatCamera()
    {
        //camera transition from combat framing to interaction framing 
        PlayerController.Instance.CamDisengage();
    }

    public void StartCombat()
    {
        //set state
        PlayerController.Instance.state = PlayerController.playerState.Fighting;

        //activate enemy
        PlayerController.Instance.enemy = myEnemy;
        myEnemy.Activate();

        //display combat UI
        PlayerController.Instance.SetupCombat();

        //set variable in manager
        CombatScript.combat = this;

        //tell player animator they're in combat
        PlayerController.Instance.SetAnimBool("Combat", true);
    }

    public void EndCombat()
    {
        //animate out combat UI
        CombatScript.instance.CombatDisabled();

        //tell player to lower bow
        PlayerController.Instance.SetAnimBool("Combat", false);

        //start coroutine
        StartCoroutine(DoEndCombat());
    }

    IEnumerator DoEndCombat()
    {
        yield return new WaitForSeconds(2);

        //start post-combat interaction
        PlayerController.Instance.CamDisengage();
        interactAfter.Interact();

        //calm cat
        CatController.Instance.SetDanger(false);

        //ease music in
        MusicManager.EaseMusic(1);

        //stow bow
        PlayerController.Instance.SetAnimBool("Danger", false);
        PlayerController.Instance.bowSound.Stow(true);
    }
}
