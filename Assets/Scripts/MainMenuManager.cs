using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public Texture2D pointer;

    private void Start()
    {
        Cursor.SetCursor(pointer, new Vector2(0, 0), CursorMode.Auto);
    }
}
