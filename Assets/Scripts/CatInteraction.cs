using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatInteraction : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cat"))
        {
            if (PlayerController.Instance.state == PlayerController.playerState.Normal)
            {
                GetComponent<Interaction>().Interact();
                this.enabled = false;
            }
        }
    }
}
