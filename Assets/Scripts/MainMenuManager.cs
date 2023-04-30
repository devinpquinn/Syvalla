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
        Cursor.SetCursor(pointer, new Vector2(0, 0), CursorMode.Auto);

        //check for continue option
        if (PlayerPrefs.HasKey("Checkpoint"))
        {
            continueButton.interactable = true;
        }
    }

    public void NewGame()
    {
        if (PlayerPrefs.HasKey("Checkpoint"))
        {
            //todo: overwrite warning?
            SceneManager.LoadScene(PlayerPrefs.GetString("Checkpoint"));
        }
        else
        {
            SceneManager.LoadScene(startScene);
        }
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("Checkpoint"));
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
