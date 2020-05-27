using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestFourBall : MonoBehaviour
{
    public static TestFourBall instance;
    GameObject newGO;
    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            newGO = new GameObject("GameObjectEmpty");
        }
    }

    private void OnMouseDown()
    {
        
    }
}
