using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSwitcher : MonoBehaviour
{
    //new step material the player will enter
    public FootstepManager.groundMaterial newMat;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerController.Instance.state == PlayerController.playerState.Normal)
            {
                PlayerController.Instance.SwitchStep(newMat);
                this.enabled = false;
            }
        }
    }
}
