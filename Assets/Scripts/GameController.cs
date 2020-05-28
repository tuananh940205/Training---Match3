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
    
    
    Sprite _sprite;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = GetComponent<GameController>();
    }

    void Start ()
    {
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

        BoardController.instance.GetNewUpperTiles2();
    }

    void OnScoreChanged()
    {
        score += 50;
        scoreText.text = "Score: " + score.ToString();
    }

    void FindingPassiveMatches(List<GameObject> listGO, GameObject[,] tilesArray)
    {
        List<GameObject> passivelyClearTileList = new List<GameObject>();
        foreach(var go in listGO)
        {
            for(int x = 0; x < tilesArray.GetLength(0); x++)
            {
                for (int y = tilesArray.GetLength(1) - 1; y >= 0; y--)
                {
                    if (tilesArray[x, y] == go)
                    {
                        if (!passivelyClearTileList.Contains(tilesArray[x, y]))
                        {
                            List<GameObject> abc = FindMatchesPassively(tilesArray[x, y], x, y, tilesArray);
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

    List<GameObject> FindMatchesPassively(GameObject go, int IndexX, int IndexY, GameObject[,] tilesArray)
    {
        List<GameObject> checkingMatchListVertical = new List<GameObject>();
        List<GameObject> checkingMatchListHorizontal = new List<GameObject>();
        List<GameObject> totalList = new List<GameObject>();
        // Scanning up
        for (int i = 1; i <= IndexY; i++)
        {
            if (tilesArray[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == tilesArray[IndexX, IndexY - i].GetComponent<SpriteRenderer>().sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", tiles[IndexX, IndexY - i].name, IndexX, IndexY - 1);
                checkingMatchListVertical.Add(tilesArray[IndexX, IndexY - i]);
            }
            else
                break;
        }
        //scanning down
        for (int i = 1; i < tilesArray.GetLength(1) - IndexY; i++)
        {
            if (tilesArray[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == tilesArray[IndexX, IndexY + i].GetComponent<SpriteRenderer>().sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", tiles[IndexX, IndexY + i].name, IndexX, IndexY + 1);
                checkingMatchListVertical.Add(tilesArray[IndexX, IndexY + i]);
            }
            else
                break;
        }
        if (checkingMatchListVertical.Count >= 2)
            totalList.AddRange(checkingMatchListVertical);
        // SCANNING LEFT
        for (int i = 1; i <= IndexX; i++)
        {
            if (tilesArray[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == tilesArray[IndexX - i, IndexY].GetComponent<SpriteRenderer>().sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", tiles[IndexX - i, IndexY].name, IndexX - i, IndexY);
                checkingMatchListHorizontal.Add(tilesArray[IndexX - i, IndexY]);
            }
            else
                break;
        }
        // SCANNING RIGHT
        for (int i = 1; i < tilesArray.GetLength(0) - IndexX; i++)
        {
            if (tilesArray[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == tilesArray[IndexX + i, IndexY].GetComponent<SpriteRenderer>().sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", BoardController.instance.tiles[IndexX + i, IndexY].name, IndexX + i, IndexY);
                checkingMatchListHorizontal.Add(tilesArray[IndexX + i, IndexY]);
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
        score = 0;
        scoreText.text = "Score: " + score.ToString();
    }

    public bool GetTheAdjacentTile(float radiant, int xIndex, int yIndex, GameObject go)
    {
        bool findTheAdjacent;
        if (radiant > -45 && radiant < 45)
        {
            //Debug.LogFormat("Right");
            if (xIndex < BoardController.instance.rowLength - 1)
            {
                if (BoardController.instance.tiles[xIndex + 1, yIndex] != null)
                    go = BoardController.instance.tiles[xIndex + 1, yIndex];
            }
        }
        else if (radiant > 45 && radiant < 135)
        {
            //Debug.LogFormat("Up");
            if (yIndex > 0)
            {
                if (BoardController.instance.tiles[xIndex, yIndex - 1] != null)
                    go = BoardController.instance.tiles[xIndex, yIndex - 1];
            }
        }
        else if (radiant > 135 || radiant < -135)
        {
            //Debug.LogFormat("Left");
            if (xIndex > 0)
            {
                if (BoardController.instance.tiles[xIndex - 1, yIndex] != null)
                    go = BoardController.instance.tiles[xIndex - 1, yIndex];
            }
        }
        else if (radiant > -135 && radiant < -45)
        {
            if (yIndex < BoardController.instance.columnLength - 1)
            {
                if (BoardController.instance.tiles[xIndex, yIndex + 1] != null)
                    go = BoardController.instance.tiles[xIndex, yIndex + 1];
            }
        }
        if (go != null)
            findTheAdjacent = true;
        else
            findTheAdjacent = false;

        return findTheAdjacent;
    }

    public IEnumerator OnTileSwapping(GameObject go1, GameObject go2, GameObject[,] tilesArray)
    {
        // Debug.LogFormat("Swapping");
        int temp1 = -1, temp2 = -1, temp3 = -1, temp4 = -1;
        for (int y = 0; y < tilesArray.GetLength(1); y++)
        {
            
            for (int x = 0; x < tilesArray.GetLength(0); x++)
            {
                if (tilesArray[x, y] == go1)
                {
                    temp1 = x;
                    temp2 = y;
                }
                if (tilesArray[x, y] == go2)
                {
                    temp3 = x;
                    temp4 = y;
                }
                if (temp1 > -1 && temp2 > -1 && temp3 > -1 && temp4 > -1)
                {
                    tilesArray[temp1, temp2] = go2;
                    tilesArray[temp1, temp2].name = "Swap [ " + temp1 + ", " + temp2 + " ]";
                    tilesArray[temp3, temp4] = go1;
                    tilesArray[temp3, temp4].name = "Swap [ " + temp3 + ", " + temp4 + " ]";
                    break;
                }
            }
        }
        Vector2 targetPosition1 = go2.transform.position;
        Vector2 targetPosition2 = go1.transform.position;

        for (int i = 0; i < 5; i++)
        {
            Vector2 currentPosition1 = go1.transform.position;
            Vector2 currentPosition2 = go2.transform.position;
            currentPosition1 += new Vector2((targetPosition1.x - go1.transform.position.x) / 5, (targetPosition1.y - go1.transform.position.y) / 5);
            currentPosition2 += new Vector2((targetPosition2.x - go2.transform.position.x) / 5, (targetPosition2.y - go2.transform.position.y) / 5);

            go1.transform.position = currentPosition1;
            go2.transform.position = currentPosition2;
            yield return new WaitForSeconds(.02f);
        }
        go1.transform.position = targetPosition1;
        go2.transform.position = targetPosition2;
        // if swap success failed

        List<GameObject> listGameObject = FindingTheMatches(go1, go2, BoardController.instance.tiles);

        if (listGameObject.Contains(go1) || listGameObject.Contains(go2))
            StartCoroutine(AllTilesFadeOut(listGameObject));
        else
            StartCoroutine(TileSwapBackOnMatchFailure(go1, go2));
    }

    IEnumerator TileSwapBackOnMatchFailure(GameObject go1, GameObject go2)
    {
        Debug.LogFormat("Swap failed");
        int temp1 = -1, temp2 = -1, temp3 = -1, temp4 = -1;
        for (int y = 0; y < BoardController.instance.columnLength; y++)
        {
            for (int x = 0; x < BoardController.instance.rowLength; x++)
            {
                if (temp1 > -1 && temp2 > -1 && temp3 > -1 && temp4 > -1)
                {
                    BoardController.instance.tiles[temp1, temp2] = go2;
                    BoardController.instance.tiles[temp1, temp2].name = "SwapBack [ " + temp1 + ", " + temp2 + " ]";
                    BoardController.instance.tiles[temp3, temp4] = go1;
                    BoardController.instance.tiles[temp3, temp4].name = "SwapBack [ " + temp3 + ", " + temp4 + " ]";

                    break;
                }
                if (BoardController.instance.tiles[x, y] == go1)
                {
                    temp1 = x;
                    temp2 = y;
                }
                if (BoardController.instance.tiles[x, y] == go2)
                {
                    temp3 = x;
                    temp4 = y;
                }
            }
        }
        Vector2 targetPosition1 = go2.transform.position;
        Vector2 targetPosition2 = go1.transform.position;

        for (int i = 0; i < 5; i++)
        {
            Vector2 currentPosition1 = go1.transform.position;
            Vector2 currentPosition2 = go2.transform.position;
            currentPosition1 += new Vector2((targetPosition1.x - go1.transform.position.x) / 5, (targetPosition1.y - go1.transform.position.y) / 5);
            currentPosition2 += new Vector2((targetPosition2.x - go2.transform.position.x) / 5, (targetPosition2.y - go2.transform.position.y) / 5);

            go1.transform.position = currentPosition1;
            go2.transform.position = currentPosition2;
            yield return new WaitForSeconds(.02f);
        }
        go1.transform.position = targetPosition1;
        go2.transform.position = targetPosition2;
        go1 = null;
        go2 = null;
    }

    public List<GameObject> FindingTheMatches(GameObject go1, GameObject go2, GameObject[,] tilesArray)
    {
        List<GameObject> clearMatchList = new List<GameObject>();

        int x1 = -1, y1 = -1, x2 = -1, y2 = -1;
        for(int y = 0; y < BoardController.instance.columnLength; y++)
        {
            for (int x = 0; x < BoardController.instance.rowLength; x++)
            {
                if (tilesArray[x, y] == go1)
                {
                    x1 = x;
                    y1 = y;
                }
                if (tilesArray[x, y] == go2)
                {
                    x2 = x;
                    y2 = y;
                }
            }
        }

        List<GameObject> list1 = FindMatchFromSwappingTiles(go1, x1, y1, BoardController.instance.tiles);
        List<GameObject> list2 = FindMatchFromSwappingTiles(go2, x2, y2, BoardController.instance.tiles);

        clearMatchList.AddRange(list1);
        clearMatchList.AddRange(list2);
        
        return clearMatchList;
    }

    public List<GameObject> FindMatchFromSwappingTiles(GameObject go, int IndexX, int IndexY, GameObject[,] tilesArray)
    {
        List<GameObject> checkingMatchListVertical = new List<GameObject>();
        List<GameObject> checkingMatchListHorizontal = new List<GameObject>();
        List<GameObject> totalList = new List<GameObject>();
        // Scanning up
        for (int i = 1; i <= IndexY; i++)
        {
            if (tilesArray[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == tilesArray[IndexX, IndexY - i].GetComponent<SpriteRenderer>().sprite)
            {
                checkingMatchListVertical.Add(tilesArray[IndexX, IndexY - i]);
            }
            else
                break;
        }
        //scanning down
        for (int i = 1; i < tilesArray.GetLength(1) - IndexY; i++)
        {
            if (tilesArray[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == tilesArray[IndexX, IndexY + i].GetComponent<SpriteRenderer>().sprite)
            {
                checkingMatchListVertical.Add(tilesArray[IndexX, IndexY + i]);
            }
            else
                break;
        }
        if (checkingMatchListVertical.Count >= 2)
            totalList.AddRange(checkingMatchListVertical);
        // SCANNING LEFT
        for (int i = 1; i <= IndexX; i++)
        {
            if (tilesArray[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == tilesArray[IndexX - i, IndexY].GetComponent<SpriteRenderer>().sprite)
            {
                checkingMatchListHorizontal.Add(tilesArray[IndexX - i, IndexY]);
            }
            else
                break;
        }
        // SCANNING RIGHT
        for (int i = 1; i < tilesArray.GetLength(0) - IndexX; i++)
        {
            if (tilesArray[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == tilesArray[IndexX + i, IndexY].GetComponent<SpriteRenderer>().sprite)
            {
                checkingMatchListHorizontal.Add(tilesArray[IndexX + i, IndexY]);
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

    

}