using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public Texture2D pointer;
    public GameObject options;

    private void Start()
    {
        Cursor.SetCursor(pointer, new Vector2(0, 0), CursorMode.Auto);
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
