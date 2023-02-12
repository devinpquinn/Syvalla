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
            }
        }
    }

    public void Danger(bool entering)
    {
        PlayerController.Instance.Danger(entering);
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
    }

    public void EndCombat()
    {
        //animate out combat UI
        CombatScript.instance.CombatDisabled();

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
    }
}
