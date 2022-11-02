using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class ColorSetter : MonoBehaviour
{
    public Light2D enviroLight;

    public List<TextMeshProUGUI> texts;

    public static ColorSetter colorSetter;
    public static ColorSetter Instance { get { return colorSetter; } }

    public Color myColor;

    private void Awake()
    {
        //singleton
        if (colorSetter != null && colorSetter != this)
        {
            Destroy(gameObject);
        }
        else
        {
            colorSetter = this;
        }
    }

    public void UpdateColor(Color newColor)
    {
        enviroLight.color = new Color(newColor.r, newColor.g, newColor.b, enviroLight.color.a);

        foreach(TextMeshProUGUI t in texts)
        {
            t.color = new Color(newColor.r, newColor.g, newColor.b, t.color.a);
        }
    }

    private void OnValidate()
    {
        UpdateColor(myColor);
    }

}
