using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp = 100f;
    public float speed = 1f;

    [HideInInspector]
    public Transform camTarget;

    private void Awake()
    {
        camTarget = transform.Find("Enemy Camera Target");
    }

    public void Activate()
    {

    }
}
