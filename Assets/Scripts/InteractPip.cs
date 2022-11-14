using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPip : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Animator>().enabled = true;
            PlayerController.Instance.interaction = gameObject.GetComponent<Interaction>();
            PlayerController.Instance.UpdateText("Press 'W' to inspect.", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Animator>().Play("PipExit");
            PlayerController.Instance.interaction = null;
            PlayerController.Instance.UpdateText("", true);
        }
    }
}
