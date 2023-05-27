using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button continueButton;

    public Texture2D pointer;
    public GameObject options;

    private string startScene = "Workroom";

    private void Start()
    {
        //set cursor
        Cursor.SetCursor(pointer, new Vector2(0, 0), CursorMode.Auto);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //check for continue option
        if (PlayerPrefs.HasKey("Checkpoint"))
        {
            continueButton.interactable = true;
        }
    }

    public void NewGame()
    {
        StartCoroutine(DoNewGame());
    }

    IEnumerator DoNewGame()
    {
        Fade.FadeEffect();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(startScene);
    }

    public void ContinueGame()
    {
        StartCoroutine(DoContinueGame());
    }

    IEnumerator DoContinueGame()
    {
        Fade.FadeEffect();
        yield return new WaitForSeconds(0.5f);
        if (PlayerPrefs.HasKey("Checkpoint"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("Checkpoint"));
        }
        else
        {
            SceneManager.LoadScene(startScene);
        }
    }

    public void ShowOptions()
    {
        options.SetActive(true);
    }

    public void HideOptions()
    {
        options.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
