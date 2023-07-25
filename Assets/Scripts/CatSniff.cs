using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSniff : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cat"))
        {
            collision.GetComponent<Animator>().SetBool("Sniff", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Cat"))
        {
            collision.GetComponent<Animator>().SetBool("Sniff", false);
        }
    }
}
