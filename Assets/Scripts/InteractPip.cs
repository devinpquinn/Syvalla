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

            if (translation)
            {
                if (!PlayerController.Instance.decodeInterface.gameObject.activeInHierarchy)
                {
                    PlayerController.Instance.UpdateText("", true);
                }

                PlayerController.Instance.translation = null;
            }
            else
            {
                if(PlayerController.Instance.bottomText.text.Length < 1)
                {
                    PlayerController.Instance.UpdateText("", true);
                }

                PlayerController.Instance.interaction = null;
            }
        }
    }
}
