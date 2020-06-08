using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadOnLy : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
