using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class ColorSetter : MonoBehaviour
{
    public List<GameObject> primary;

    public List<GameObject> complementary;

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
        foreach(GameObject x in primary)
        {
            if(x.GetComponent<Light2D>() != null)
            {
                x.GetComponent<Light2D>().color = new Color(newColor.r, newColor.g, newColor.b);
            }
            else if (x.GetComponent<TextMeshProUGUI>() != null)
            {
                x.GetComponent<TextMeshProUGUI>().color = new Color(newColor.r, newColor.g, newColor.b, x.GetComponent<TextMeshProUGUI>().color.a);
            }
            else if (x.GetComponent<SpriteRenderer>() != null)
            {
                x.GetComponent<SpriteRenderer>().color = new Color(newColor.r, newColor.g, newColor.b, x.GetComponent<SpriteRenderer>().color.a);
            }
        }

        foreach(GameObject y in complementary)
        {
            Color inverted = InvertColor(newColor);
            if (y.GetComponent<Light2D>() != null)
            {
                y.GetComponent<Light2D>().color = inverted;
            }
            else if (y.GetComponent<TextMeshProUGUI>() != null)
            {
                y.GetComponent<TextMeshProUGUI>().color = new Color(inverted.r, inverted.g, inverted.b, y.GetComponent<TextMeshProUGUI>().color.a);
            }
            else if (y.GetComponent<SpriteRenderer>() != null)
            {
                y.GetComponent<SpriteRenderer>().color = new Color(inverted.r, inverted.g, inverted.b, y.GetComponent<SpriteRenderer>().color.a);
            }
        }
    }

    public Color InvertColor(Color myColor)
    {
        float h, s, v;
        Color.RGBToHSV(new Color(1 - myColor.r, 1 - myColor.g, 1 - myColor.b), out h, out s, out v);
        return Color.HSVToRGB(h, s, 1f);
    }

    private void OnValidate()
    {
        UpdateColor(myColor);
    }

}
