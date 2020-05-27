using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class TestThreeTile : MonoBehaviour
{
    public static TestThreeTile instance;
    
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
        TestFourBall.instance.transform.DOMove(transform.position, 0.5f);
    }

    void ArriveOnTile()
    {
        TestFourBall.instance.transform.DOShakeScale(.5f);
    }
}
