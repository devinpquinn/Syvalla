using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public Interaction interactBefore;
    public Interaction interactAfter;

    public Enemy myEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerController.Instance.state == PlayerController.playerState.Normal)
            {
                //initiate pre-combat interaction
                interactBefore.Interact();
            }
        }
    }

    public void StartCombatCamera()
    {
        //camera transition from interaction framing to combat framing
    }

    public void EndCombatCamera()
    {
        //camera transition from combat framing to interaction framing 
    }

    public void StartCombat()
    {
        //set state
        PlayerController.Instance.state = PlayerController.playerState.Fighting;

        //display combat UI

        //activate enemy
    }

    public void EndCombat()
    {
        //hide combat UI

        //reset camera

        //start post-battle interaction
    }
}
