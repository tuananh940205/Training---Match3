using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test06 : MonoBehaviour
{
    Tween tween;
    Vector3[] paths = new Vector3[] { new Vector2(-7, 13), new Vector2(23, 15), Vector3.down, Vector3.right };
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            tween = transform.DOPath(paths, 1).SetEase(Ease.InOutQuad);
        }
    }
}
