using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
    public static BoardController instance;


    //TEST

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start() {}

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    for (int y = 0; y < GameController.instance.columnLength; y++)
        //    {
        //        for (int x = 0; x < GameController.instance.rowLength; x++)
        //        {
        //            StopCoroutine(GameController.instance.MoveTiles2(tiles[x, y], x, y));
        //            StartCoroutine(GameController.instance.MoveTiles2(tiles[x, y], x, y));
        //        }
        //    }
        //}
    }
    public void CreateBoard(float xOffset, float yOffset)
    {
        GameController.instance.tiles = new GameObject[GameController.instance.rowLength, GameController.instance.columnLength];

        for (int y = 0; y < GameController.instance.columnLength; y++)
        {
            for (int x = 0; x < GameController.instance.rowLength; x++)
            {
                GameObject newTile = Instantiate(GameController.instance.tile, new Vector3(GameController.instance.startPosition.x + (xOffset * x), GameController.instance.startPosition.y - (yOffset * y), 0), GameController.instance.tile.transform.rotation);

                GameController.instance.tiles[x, y] = newTile;
                newTile.name = "[ " + x + " , " + y + " ]";
                GameController.instance.tiles[x, y].transform.parent = transform;

                List<Sprite> possibleCharacters = new List<Sprite>();

                possibleCharacters.AddRange(GameController.instance.characters);

                if (x > 0)
                    if (GameController.instance.tiles[x - 1, y] != null)
                        possibleCharacters.Remove(GameController.instance.tiles[x - 1, y].GetComponent<SpriteRenderer>().sprite);
                if (y > 0)
                    if (GameController.instance.tiles[x, y - 1] != null)
                        possibleCharacters.Remove(GameController.instance.tiles[x, y - 1].GetComponent<SpriteRenderer>().sprite);
                Debug.LogFormat("possibleCharacter count is {0}", possibleCharacters.Count);
                Sprite newSprite = possibleCharacters[Random.Range(0, possibleCharacters.Count)];
                newTile.GetComponent<SpriteRenderer>().sprite = newSprite;
            }
        }
    }
}
    //void GetNewUpperTiles()
    //{
    //    for (int x = 0; x < rowLength; x++)
    //    {
    //        int inactiveCount = 0;
    //        bool foundInactive = false;
    //        List<GameObject> list1 = new List<GameObject>();
    //        List<GameObject> list2 = new List<GameObject>();
    //        for (int y = columnLength - 1; y >= 0; y--)
    //        {
    //            if (tiles[x, y].activeSelf)
    //            {
    //                if (!foundInactive)
    //                {
    //                    continue;
    //                }
    //                else
    //                {
    //                    list1.Add(tiles[x, y]);
    //                }
    //            }
    //            else if (!tiles[x, y].activeSelf)
    //            {
    //                foundInactive = true;
    //                inactiveCount++;
    //                list2.Add(tiles[x, y]);
    //            }
    //        }
            
    //        for (int i = 0; i < inactiveCount; i++)
    //        {
    //            list2[i].GetComponent<SpriteRenderer>().sprite = characters[Random.Range(0, characters.Count)];
    //            list2[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    //            list2[i].transform.position = new Vector2(list2[i].transform.position.x, startPosition.y + (i + 1) * offset.y);
    //        }

    //        list1.AddRange(list2);
    //        for (int i = 0; i < list1.Count; i++)
    //        {
    //            tiles[x, i] = list1[list1.Count - 1 - i];
    //            tiles[x, i].name = "New [ " + x + ", " + i + " ]";
    //        }
            
    //    }
    //}

    

    

    

    //IEnumerator MoveTiles(GameObject go)
    //{
    //    int a = -1, b = -1;
    //    //Debug.LogFormat("move tiles");
    //    for (int y = 0; y < columnLength; y++)
    //    {
    //        for (int x = 0; x < rowLength; x++)
    //        {
    //            if (tiles[x, y] == go)
    //            {
    //                a = x;
    //                b = y;
    //                //Debug.LogFormat("go = [{0}, {1}", x, y);
    //                while (Vector2.Distance(go.transform.position, new Vector2(go.transform.position.x, startPosition.y - offset.y * y)) > .1f)
    //                {
    //                    go.transform.position = Vector2.MoveTowards(go.transform.position, new Vector2(go.transform.position.x, startPosition.y - offset.y * y), .25f);
    //                    if (go.transform.position.y <= startPosition.y + offset.y)
    //                        go.SetActive(true);
    //                    yield return new WaitForSeconds(.05f);
    //                }
    //                go.transform.position = new Vector2(go.transform.position.x, startPosition.y - offset.y * y);
    //                Debug.LogFormat("toa do den noi {0}: [{1}, {2}]", go.name, x, y);
    //            }
    //        }
    //    }
    //    // Continue Here
    //    if (TileController.instance2.FindMatchFromSwappingTiles(go, a, b).Contains(go))
    //    {
    //        StopCoroutine(AllTilesFadeOut(TileController.instance2.FindMatchFromSwappingTiles(go, a, b)));
    //        StartCoroutine(AllTilesFadeOut(TileController.instance2.FindMatchFromSwappingTiles(go, a, b)));
    //    }
    //    //FindingPassiveMatches(go);
    //}

    //List<GameObject> MatchedGroup()
    //{
    //    List<GameObject> matchedGroup = new List<GameObject>();
        
    //    for (int x = 0; x < rowLength; x++)
    //    {
    //        for (int y = columnLength - 1; y >= 0; y--)
    //        {

    //        }
    //    }
    //    return matchedGroup;
    //}


    //IEnumerator MoveTilesForList(List<GameObject> listGO)
    //{
    //    foreach (var go in listGO)
    //    {
    //        for (int x = 0; x < rowLength; x++)
    //        {
    //            for (int y = columnLength - 1; y >= 0; y--)
    //            {
    //                if (tiles[x, y] == go)
    //                {
    //                    while (Vector2.Distance(go.transform.position, new Vector2(go.transform.position.x, startPosition.y - offset.y * y)) > .1f)
    //                    {
    //                        go.transform.position = Vector2.MoveTowards(go.transform.position, new Vector2(go.transform.position.x, startPosition.y - offset.y * y), .25f);
    //                        if (go.transform.position.y <= startPosition.y + offset.y)
    //                            go.SetActive(true);
    //                        yield return new WaitForSeconds(.05f);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    // IEnumerator GetTheTilesDown2(List<GameObject> listGO, int distance)
    // {
    //     for (int i = 0; i < 4 * distance; i++)
    //     {
    //         foreach (var go in listGO)
    //         {
    //             Vector2 currentPosition = go.transform.position;
    //             currentPosition.y -= offset.y / 4;
    //             go.transform.position = currentPosition;
    //             if (!go.activeSelf && go.transform.position.y <= (startPosition.y + offset.y))
    //                 go.SetActive(true);

    //         }
    //         yield return new WaitForSeconds(.05f);
    //     }
    //     //yield return new WaitForEndOfFrame();
    //     //StartCoroutine(FindingPassiveMatches(listGameObject));
        
    // }

    // IEnumerator GetTheTilesColumnsDownAndSetArrayIndex(List<GameObject> listGameObject, int distance)
    // {
    //     //Debug.LogFormat("Start Coroutine GetTheTilesColumnsDownAndSetArrayIndex");
    //     for (int i = 0; i < 4 * distance; i++)
    //     {
    //         foreach (var go in listGameObject)
    //         {
    //             Vector2 currentPosition = go.transform.position;
    //             currentPosition.y -= offset.y / 4;
    //             go.transform.position = currentPosition;
    //             if (!go.activeSelf && go.transform.position.y <= (startPosition.y + offset.y))
    //                 go.SetActive(true);

    //         }
    //         yield return new WaitForSeconds(.05f);
    //     }
    //     //yield return new WaitForEndOfFrame();
    //     //StartCoroutine(FindingPassiveMatches(listGameObject));
    //     FindingPassiveMatches(listGameObject);
    // }