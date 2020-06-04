using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test08 : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            spriteRenderer.DOColor(new Color(0, 0, 0, 0), 1);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            spriteRenderer.DOFade(0, 1);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            
        }
    }
}