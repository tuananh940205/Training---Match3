using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestTwo : MonoBehaviour
{
    public static TestTwo instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        transform.DOMove(transform.position, .5f).SetEase(Ease.InOutQuad);
    }
}
