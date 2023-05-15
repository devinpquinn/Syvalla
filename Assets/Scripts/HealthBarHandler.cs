using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
    public Image barPrimary;
    public Image barSecondary;

    public void Reset()
    {
        StopAllCoroutines();
        barPrimary.fillAmount = 1;
        barSecondary.fillAmount = 1;
    }

    public void UpdateHP(float fraction)
    {
        StopAllCoroutines();
        StartCoroutine(UpdateHPBar(barPrimary.fillAmount, fraction));
    }

    IEnumerator UpdateHPBar(float oldFraction, float newFraction)
    {
        barPrimary.fillAmount = newFraction;
        barSecondary.fillAmount = oldFraction;
        yield return new WaitForSeconds(1f);
        barSecondary.fillAmount = newFraction;
    }
}
