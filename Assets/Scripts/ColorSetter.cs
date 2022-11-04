using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class ColorSetter : MonoBehaviour
{
    public Light2D enviroLight;

    public List<TextMeshProUGUI> texts;

    public List<SpriteRenderer> inverseSprites;

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

        foreach(SpriteRenderer s in inverseSprites)
        {
            s.color = InvertColor(newColor);
        }
    }

    public Color InvertColor(Color myColor)
    {
        float h, s, v;
        Color.RGBToHSV(new Color(1 - myColor.r, 1 - myColor.g, 1 - myColor.b), out h, out s, out v);
        return Color.HSVToRGB(h, s, 0.85f);
    }

    private void OnValidate()
    {
        UpdateColor(myColor);
    }

}
