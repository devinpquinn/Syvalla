using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHP = 100f;
    [HideInInspector]
    public float currentHP;
    public float speed = 1f;

    [HideInInspector]
    public Transform camTarget;

    private void Awake()
    {
        camTarget = transform.Find("Enemy Camera Target");
        currentHP = maxHP;
    }

    public void Activate()
    {
        //start moving toward player
    }
}
