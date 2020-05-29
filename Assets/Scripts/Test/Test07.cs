using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test07 : MonoBehaviour
{
    [SerializeField] GameObject tile;
    List<GameObject> listObj = new List<GameObject>();
    int a = 0;
    void Start()
    {
        CreateFor();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveToIndex(listObj);
        }
    }

    void CreateFor()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject go = Instantiate(tile, new Vector2(Random.Range(0, 10) * .5f, Random.Range(0, 10) * .5f), tile.transform.rotation);
            go.name = "[ " + go.transform.position.x + ", " + go.transform.position.y + " ]";
            listObj.Add(go);
            a++;
        }
    }

    void MoveToIndex(List<GameObject> listgo)
    {
        foreach (var go in listgo)
        {
            StartCoroutine(ObjMove(go, 1 , 1));
        }
    }

    IEnumerator ObjMove(GameObject go, float x, float y)
    {
        while (Vector2.Distance(go.transform.position, new Vector2(x, y)) >= .05f)
        {
            go.transform.position = Vector2.MoveTowards(go.transform.position, new Vector2(x, y), .04f);
            yield return new WaitForSeconds(.02f);
        }
        if (a > 1)
        {
            a--;
        }
        else
        {
            End();
        }
    }

    void End()
    {
        Debug.LogFormat("End");
    }
}
