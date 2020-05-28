using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public TileController obj;
    [SerializeField] public Text scoreText;
    [SerializeField] public int score;
    Dictionary<string, Coroutine> coroutineMap = new Dictionary<string, Coroutine>();
    public List<Sprite> characters = new List<Sprite>();
    [SerializeField] public GameObject tile;
    public int rowLength;
    public int columnLength;
    public GameObject[,] tiles;
    public List<Sprite> listSwapContainer = new List<Sprite>();
    public Vector2 offset;
    public Vector2 startPosition = new Vector2(-2.61f, 3.5f);

    void Awake()
    {
        offset = tile.GetComponent<SpriteRenderer>().bounds.size;
        instance = GetComponent<GameController>();
    }

    void Start ()
    {
        BoardController.instance.CreateBoard(offset.x, offset.y);

        DetectMatchExist(FindTheMatchExist());

        ShowScore();
    }


    public IEnumerator AllTilesFadeOut(List<GameObject> tiles)
    {
        for (int i = 10; i > 0; i--)
        {
            foreach (var tile in tiles)
                tile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i * .1f);

            yield return new WaitForSeconds(.05f);
        }
        foreach (var tile in tiles)
        {
            tile.SetActive(false);
            OnScoreChanged();
        }

        //GetNewUpperTiles();
        GetNewUpperTiles2();
    }

    void OnScoreChanged()
    {
        score += 50;
        scoreText.text = "Score: " + score.ToString();
    }

    void GetNewUpperTiles2()
    
    {
        for (int y = 0; y < columnLength; y++)
        {
            for (int x = 0; x < rowLength; x++)
            {
                tiles[x, y].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }
        for (int x = 0; x < rowLength; x++)
        {
            //Debug.LogFormat("Execute GetNewUpperTiles2");
            List<GameObject> listTotal = new List<GameObject>();
            List<GameObject> nullList = new List<GameObject>();

            int nullCount = 0;
            for (int y = columnLength - 1; y >= 0; y--)
            {
                if (tiles[x, y].activeSelf)
                {
                    if (nullCount == 0)
                        continue;
                    else
                        listTotal.Add(tiles[x, y]);
                }
                else
                {
                    nullList.Add(tiles[x, y]);
                    nullCount++;
                }
            }
            for (int i = 0; i < nullList.Count; i++)
            {
                nullList[i].GetComponent<SpriteRenderer>().sprite = characters[Random.Range(0, characters.Count)];
                nullList[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                nullList[i].transform.position = new Vector2(nullList[i].transform.position.x, startPosition.y + (i + 1) * offset.y);
            }
            listTotal.AddRange(nullList);



            for (int i = 0; i < listTotal.Count; i++)
            {
                tiles[x, i] = listTotal[listTotal.Count - 1 - i];
                tiles[x, i].name = "New [ " + x + ", " + i + " ]";
            }
            for (int i = 0; i < listTotal.Count; i++)
            {
                if (coroutineMap.ContainsKey(tiles[x, i].name))
                {
                    StopCoroutine(coroutineMap[tiles[x, i].name]);
                    coroutineMap.Remove(tiles[x, i].name);
                }
                coroutineMap[tiles[x, i].name] = StartCoroutine(MoveTiles2(tiles[x, i], x, i));
            }
        }
    }

    IEnumerator MoveTiles2(GameObject go, int indexX, int indexY)
    {
        Vector2 finalPosition = new Vector2(go.transform.position.x, startPosition.y - offset.y * indexY);
        while (Vector2.Distance(go.transform.position, finalPosition) >= .1f)
        {
            go.transform.position = Vector2.MoveTowards(go.transform.position, finalPosition, 0.25f);
            if (go.transform.position.y <= startPosition.y + offset.y)
                go.SetActive(true);
            yield return new WaitForSeconds(.05f);
        }
        go.transform.position = finalPosition;


        DetectMatchExist(FindTheMatchExist());
        if (secondTile.GetComponent<TileController>.FindMatchFromSwappingTiles(go, indexX, indexY).Contains(go))
        {
            //StopAllCoroutines();
            StartCoroutine(instance.AllTilesFadeOut(TileController.secondTile.FindMatchFromSwappingTiles(go, indexX, indexY)));
        }
    }

    public void DetectMatchExist(List<GameObject> listGO)
    {
        if (listGO.Count > 0)
        {
            Debug.LogFormat("We found the Match!");
            foreach (var go in listGO)
                go.GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, 1);
        }
        else
        {
            Debug.LogFormat("Match 404 not found. Reset the board");

            for (int y = 0; y < columnLength; y++)
            {
                for (int x = 0; x < rowLength; x++)
                {
                    listSwapContainer.Add(tiles[x, y].GetComponent<SpriteRenderer>().sprite);
                }
            }

            for (int y = 0; y < columnLength; y++)
            {
                for (int x = 0; x < rowLength; x++)
                {
                    Sprite go = listSwapContainer[Random.Range(0, listSwapContainer.Count)];
                    tiles[x, y].GetComponent<SpriteRenderer>().sprite = go;
                    listSwapContainer.Remove(go);
                }
            }
        }
    }

    public List<GameObject> FindTheMatchExist()
    {
        List<GameObject> listCanBeMatch = new List<GameObject>();
        for (int y = 0; y < columnLength; y++)
        {
            for (int x = 0; x < rowLength; x++)
            {
                if (y < columnLength - 3)
                {
                    if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x, y + 2].GetComponent<SpriteRenderer>().sprite &&
                    tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x, y + 3].GetComponent<SpriteRenderer>().sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x, y + 2]);
                        listCanBeMatch.Add(tiles[x, y + 3]);
                    }

                    if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x, y + 1].GetComponent<SpriteRenderer>().sprite &&
                        tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x, y + 3].GetComponent<SpriteRenderer>().sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x, y + 1]);
                        listCanBeMatch.Add(tiles[x, y + 3]);
                    }
                }

                if (x < rowLength - 3)
                {
                    if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x + 2, y].GetComponent<SpriteRenderer>().sprite &&
                        tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x + 3, y].GetComponent<SpriteRenderer>().sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x + 2, y]);
                        listCanBeMatch.Add(tiles[x + 3, y]);
                    }

                    if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite &&
                        tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x + 3, y].GetComponent<SpriteRenderer>().sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x + 1, y]);
                        listCanBeMatch.Add(tiles[x + 3, y]);
                    }
                }

                // new write method
                if (x < rowLength - 1 && y < columnLength - 2)
                {
                    if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite &&
                            tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x, y + 2].GetComponent<SpriteRenderer>().sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x + 1, y + 1]);
                        listCanBeMatch.Add(tiles[x, y + 2]);
                    }

                    if (tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite == tiles[x, y + 1].GetComponent<SpriteRenderer>().sprite &&
                        tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite == tiles[x + 1, y + 2].GetComponent<SpriteRenderer>().sprite)
                    {
                        listCanBeMatch.Add(tiles[x + 1, y]);
                        listCanBeMatch.Add(tiles[x, y + 1]);
                        listCanBeMatch.Add(tiles[x + 1, y + 2]);
                    }

                    if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite &&
                        tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x + 1, y + 2].GetComponent<SpriteRenderer>().sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x + 1, y + 1]);
                        listCanBeMatch.Add(tiles[x + 1, y + 2]);
                    }

                    if (tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite == tiles[x, y + 1].GetComponent<SpriteRenderer>().sprite &&
                        tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite == tiles[x, y + 2].GetComponent<SpriteRenderer>().sprite)
                    {
                        listCanBeMatch.Add(tiles[x + 1, y]);
                        listCanBeMatch.Add(tiles[x, y + 1]);
                        listCanBeMatch.Add(tiles[x, y + 2]);
                    }

                    if (tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite == tiles[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite &&
                        tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite == tiles[x, y + 2].GetComponent<SpriteRenderer>().sprite)
                    {
                        listCanBeMatch.Add(tiles[x + 1, y]);
                        listCanBeMatch.Add(tiles[x + 1, y + 1]);
                        listCanBeMatch.Add(tiles[x, y + 2]);
                    }

                    if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x, y + 1].GetComponent<SpriteRenderer>().sprite &&
                        tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x + 1, y + 2].GetComponent<SpriteRenderer>().sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x, y + 1]);
                        listCanBeMatch.Add(tiles[x + 1, y + 2]);
                    }


                }

                if (x < rowLength - 2 && y < columnLength - 1)
                {
                    if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite &&
                        tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x + 2, y + 1].GetComponent<SpriteRenderer>().sprite)
                    {
                        listCanBeMatch.Add(tiles[x + 1, y + 1]);
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x + 2, y + 1]);
                    }

                    if (tiles[x, y + 1].GetComponent<SpriteRenderer>().sprite == tiles[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite &&
                        tiles[x, y + 1].GetComponent<SpriteRenderer>().sprite == tiles[x + 2, y].GetComponent<SpriteRenderer>().sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y + 1]);
                        listCanBeMatch.Add(tiles[x + 1, y + 1]);
                        listCanBeMatch.Add(tiles[x + 2, y]);
                    }



                    if (tiles[x, y + 1].GetComponent<SpriteRenderer>().sprite == tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite &&
                        tiles[x, y + 1].GetComponent<SpriteRenderer>().sprite == tiles[x + 2, y].GetComponent<SpriteRenderer>().sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y + 1]);
                        listCanBeMatch.Add(tiles[x + 1, y]);
                        listCanBeMatch.Add(tiles[x + 2, y]);
                    }

                    if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite &&
                        tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x + 2, y + 1].GetComponent<SpriteRenderer>().sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x + 1, y]);
                        listCanBeMatch.Add(tiles[x + 2, y + 1]);
                    }

                    if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x + 2, y].GetComponent<SpriteRenderer>().sprite &&
                        tiles[x, y].GetComponent<SpriteRenderer>().sprite == tiles[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x + 2, y]);
                        listCanBeMatch.Add(tiles[x + 1, y + 1]);
                    }

                    if (tiles[x, y + 1].GetComponent<SpriteRenderer>().sprite == tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite &&
                        tiles[x, y + 1].GetComponent<SpriteRenderer>().sprite == tiles[x + 2, y + 1].GetComponent<SpriteRenderer>().sprite)
                    {
                        //Debug.Log("Alooooooo");
                        listCanBeMatch.Add(tiles[x, y + 1]);
                        listCanBeMatch.Add(tiles[x + 1, y]);
                        listCanBeMatch.Add(tiles[x + 2, y + 1]);
                    }
                }
            }
        }
        return listCanBeMatch;
    }

    void FindingPassiveMatches(List<GameObject> listGO)
    {
        List<GameObject> passivelyClearTileList = new List<GameObject>();
        foreach(var go in listGO)
        {
            for(int x = 0; x < rowLength; x++)
            {
                for (int y = columnLength - 1; y >= 0; y--)
                {
                    if (tiles[x, y] == go)
                    {
                        if (!passivelyClearTileList.Contains(tiles[x, y]))
                        {
                            List<GameObject> abc = FindMatchesPassively(tiles[x, y], x, y);
                            foreach(var a in abc)
                            {
                                if (!passivelyClearTileList.Contains(a))
                                    passivelyClearTileList.Add(a);
                            }
                        }
                    }
                }
            }
        }
        if (passivelyClearTileList.Count > 0)
            StartCoroutine(GameController.instance.AllTilesFadeOut(passivelyClearTileList));
    }

    List<GameObject> FindMatchesPassively(GameObject go, int IndexX, int IndexY)
    {
        List<GameObject> checkingMatchListVertical = new List<GameObject>();
        List<GameObject> checkingMatchListHorizontal = new List<GameObject>();
        List<GameObject> totalList = new List<GameObject>();
        // Scanning up
        for (int i = 1; i <= IndexY; i++)
        {
            if (tiles[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == tiles[IndexX, IndexY - i].GetComponent<SpriteRenderer>().sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", tiles[IndexX, IndexY - i].name, IndexX, IndexY - 1);
                checkingMatchListVertical.Add(tiles[IndexX, IndexY - i]);
            }
            else
                break;
        }
        //scanning down
        for (int i = 1; i < columnLength - IndexY; i++)
        {
            if (tiles[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == tiles[IndexX, IndexY + i].GetComponent<SpriteRenderer>().sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", tiles[IndexX, IndexY + i].name, IndexX, IndexY + 1);
                checkingMatchListVertical.Add(tiles[IndexX, IndexY + i]);
            }
            else
                break;
        }
        if (checkingMatchListVertical.Count >= 2)
            totalList.AddRange(checkingMatchListVertical);
        // SCANNING LEFT
        for (int i = 1; i <= IndexX; i++)
        {
            if (tiles[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == tiles[IndexX - i, IndexY].GetComponent<SpriteRenderer>().sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", tiles[IndexX - i, IndexY].name, IndexX - i, IndexY);
                checkingMatchListHorizontal.Add(tiles[IndexX - i, IndexY]);
            }
            else
                break;
        }
        // SCANNING RIGHT
        for (int i = 1; i < rowLength - IndexX; i++)
        {
            if (tiles[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == tiles[IndexX + i, IndexY].GetComponent<SpriteRenderer>().sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", BoardController.instance.tiles[IndexX + i, IndexY].name, IndexX + i, IndexY);
                checkingMatchListHorizontal.Add(tiles[IndexX + i, IndexY]);
            }
            else
                break;
        }
        if (checkingMatchListHorizontal.Count >= 2)
            totalList.AddRange(checkingMatchListHorizontal);

        if (totalList.Count >= 2)
            totalList.Add(go);

        return totalList;
    }

    void ShowScore()
    {
        GameController.instance.score = 0;
        GameController.instance.scoreText.text = "Score: " + GameController.instance.score.ToString();
    }
    
}