using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPip : MonoBehaviour
{
    public bool translation = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Animator>().enabled = true;

            if (translation)
            {
                PlayerController.Instance.translation = gameObject.GetComponent<Translation>();
                PlayerController.Instance.UpdateText("Press 'W' to translate.", true);
            }
            else
            {
                PlayerController.Instance.interaction = gameObject.GetComponent<Interaction>();
                PlayerController.Instance.UpdateText("Press 'W' to inspect.", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Animator>().Play("PipExit");
            PlayerController.Instance.UpdateText("", true);

            if (translation)
            {
                PlayerController.Instance.translation = null;
            }
            else
            {
                PlayerController.Instance.UpdateText("", true);
            }
        }
    }
}
