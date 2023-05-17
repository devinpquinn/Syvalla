using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string TargetScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerController.Instance.state == PlayerController.playerState.Normal)
            {
                //begin scene transition
                StartCoroutine(DoDoor());
            }
        }
    }

    IEnumerator DoDoor()
    {
        Fade.FadeEffect();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(TargetScene);
    }
}
