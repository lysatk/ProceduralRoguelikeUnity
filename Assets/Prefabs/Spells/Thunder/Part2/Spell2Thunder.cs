﻿using UnityEngine;

public class Spell2Thunder : MonoBehaviour
{
    [SerializeField]
    float timeOut = 0.3f;
    private void Awake()
    {
        Invoke("TimeOut", timeOut);
    }

    void TimeOut()
    {
        Destroy(gameObject);
    }
}