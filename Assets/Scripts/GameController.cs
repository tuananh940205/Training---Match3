﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    [SerializeField] public Text scoreText;
    [SerializeField] public int score;
    public GameObject firstTile = null;
    public GameObject secondTile = null;
    

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = GetComponent<GameController>();
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
            tile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            OnScoreChanged();
        }
        firstTile = null;
        secondTile = null;
        BoardController.Instance.GetNewUpperTiles2();
    }
    

    void OnScoreChanged()
    {
        score += 50;
        scoreText.text = "Score: " + score.ToString();
    }

    public void ClearAllPassiveMatches(List<GameObject> listGO, TileController[,] tilesArray)
    {
        List<GameObject> passivelyClearTileList = new List<GameObject>();
        foreach(var go in listGO)
        {
            for(int x = 0; x < tilesArray.GetLength(0); x++)
            {
                for (int y = tilesArray.GetLength(1) - 1; y >= 0; y--)
                {
                    if (tilesArray[x, y].gameObject == go)
                    {
                        if (!passivelyClearTileList.Contains(tilesArray[x, y].gameObject))
                        {

                            foreach(var tile in FindMatchesPassively(tilesArray[x, y].gameObject, x, y, tilesArray))
                            {
                                if (!passivelyClearTileList.Contains(tile))
                                    passivelyClearTileList.Add(tile);
                            }
                        }
                    }
                }
            }
        }
        if (passivelyClearTileList.Count > 0)
            StartCoroutine(AllTilesFadeOut(passivelyClearTileList));
    }

    public List<GameObject> FindMatchesPassively(GameObject go, int IndexX, int IndexY, TileController[,] tilesArray)
    {
        Debug.LogFormat("Execute FindMatchPassiveLy");
        List<GameObject> checkingMatchListVertical = new List<GameObject>();
        List<GameObject> checkingMatchListHorizontal = new List<GameObject>();
        List<GameObject> totalList = new List<GameObject>();
        // Scanning up
        for (int i = 1; i <= IndexY; i++)
        {
            if (tilesArray[IndexX, IndexY].SpriteRenderer.sprite == tilesArray[IndexX, IndexY - i].SpriteRenderer.sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", tiles[IndexX, IndexY - i].name, IndexX, IndexY - 1);
                checkingMatchListVertical.Add(tilesArray[IndexX, IndexY - i].gameObject);
            }
            else
                break;
        }
        //scanning down
        for (int i = 1; i < tilesArray.GetLength(1) - IndexY; i++)
        {
            if (tilesArray[IndexX, IndexY].SpriteRenderer.sprite == tilesArray[IndexX, IndexY + i].SpriteRenderer.sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", tiles[IndexX, IndexY + i].name, IndexX, IndexY + 1);
                checkingMatchListVertical.Add(tilesArray[IndexX, IndexY + i].gameObject);
            }
            else
                break;
        }
        if (checkingMatchListVertical.Count >= 2)
            totalList.AddRange(checkingMatchListVertical);
        // SCANNING LEFT
        for (int i = 1; i <= IndexX; i++)
        {
            if (tilesArray[IndexX, IndexY].SpriteRenderer.sprite == tilesArray[IndexX - i, IndexY].SpriteRenderer.sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", tiles[IndexX - i, IndexY].name, IndexX - i, IndexY);
                checkingMatchListHorizontal.Add(tilesArray[IndexX - i, IndexY].gameObject);
            }
            else
                break;
        }
        // SCANNING RIGHT
        for (int i = 1; i < tilesArray.GetLength(0) - IndexX; i++)
        {
            if (tilesArray[IndexX, IndexY].SpriteRenderer.sprite == tilesArray[IndexX + i, IndexY].SpriteRenderer.sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", BoardController.instance.tiles[IndexX + i, IndexY].name, IndexX + i, IndexY);
                checkingMatchListHorizontal.Add(tilesArray[IndexX + i, IndexY].gameObject);
            }
            else
                break;
        }
        if (checkingMatchListHorizontal.Count >= 2)
            totalList.AddRange(checkingMatchListHorizontal);

        if (totalList.Count >= 2)
            totalList.Add(go);

        Debug.LogFormat("totalListCount = {0}", totalList.Count);

        return totalList;
    }

    void ShowScore()
    {
        score = 0;
        scoreText.text = "Score: " + score.ToString();
    }

    public bool GetTheAdjacentTile(float radiant, int xIndex, int yIndex, TileController[,] tilesArray)
    {
        bool findTheAdjacent;
        if (radiant > -45 && radiant < 45)
        {
            //Debug.LogFormat("Right");
            if (xIndex < tilesArray.GetLength(0) - 1)
            {
                if (BoardController.Instance.tiles[xIndex + 1, yIndex] != null)
                    secondTile = tilesArray[xIndex + 1, yIndex].gameObject;
            }
        }
        else if (radiant > 45 && radiant < 135)
        {
            //Debug.LogFormat("Up");
            if (yIndex > 0)
            {
                if (tilesArray[xIndex, yIndex - 1] != null)
                    secondTile = tilesArray[xIndex, yIndex - 1].gameObject;
            }
        }
        else if (radiant > 135 || radiant < -135)
        {
            //Debug.LogFormat("Left");
            if (xIndex > 0)
            {
                if (tilesArray[xIndex - 1, yIndex] != null)
                    secondTile = tilesArray[xIndex - 1, yIndex].gameObject;
            }
        }
        else if (radiant > -135 && radiant < -45)
        {
            // Debug.LogFormat("Down");
            if (yIndex < tilesArray.GetLength(1) - 1)
            {
                if (tilesArray[xIndex, yIndex + 1] != null)
                    secondTile = tilesArray[xIndex, yIndex + 1].gameObject;
            }
        }
        if (secondTile != null)
            findTheAdjacent = true;
        else
            findTheAdjacent = false;

        return findTheAdjacent;
    }

    void OnTileSwapping(TileController go1, TileController go2, TileController[,] tilesArray)
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
                    tilesArray[temp1, temp2].gameObject.name = "Swap [ " + temp1 + ", " + temp2 + " ]";
                    tilesArray[temp3, temp4] = go1;
                    tilesArray[temp3, temp4].gameObject.name = "Swap [ " + temp3 + ", " + temp4 + " ]";
                    break;
                }
            }
        }
        Vector2 targetPosition1 = go2.gameObject.transform.position;
        Vector2 targetPosition2 = go1.gameObject.transform.position;

        StartCoroutine(SwappingTiles(go1, go2, targetPosition1, targetPosition2));
    }

    IEnumerator SwappingTiles(TileController tile1, TileController tile2, Vector2 targetPosition1, Vector2 targetPosition2)
    {
        for (int i = 0; i < 5; i++)
        {
            Vector2 currentPosition1 = tile1.gameObject.transform.position;
            Vector2 currentPosition2 = tile2.gameObject.transform.position;
            currentPosition1 += new Vector2((targetPosition1.x - tile1.gameObject.transform.position.x) / 5, (targetPosition1.y - tile1.gameObject.transform.position.y) / 5);
            currentPosition2 += new Vector2((targetPosition2.x - tile2.gameObject.transform.position.x) / 5, (targetPosition2.y - tile2.gameObject.transform.position.y) / 5);

            tile1.transform.position = currentPosition1;
            tile2.transform.position = currentPosition2;
            yield return new WaitForSeconds(.02f);
        }
        tile1.transform.position = targetPosition1;
        tile2.transform.position = targetPosition2;
        // if swap success failed

        List<GameObject> listGameObject = FindingTheMatches(tile1.gameObject, tile2.gameObject, BoardController.Instance.tiles);

        if (listGameObject.Contains(tile1.gameObject) || listGameObject.Contains(tile2.gameObject))
            StartCoroutine(AllTilesFadeOut(listGameObject));
        else
            StartCoroutine(TileSwapBackOnMatchFailure(tile1, tile2, BoardController.Instance.tiles));
    }

    IEnumerator TileSwapBackOnMatchFailure(TileController tile1, TileController tile2, TileController[,] tilesArray)
    {
        // Debug.LogFormat("Swap failed");
        int temp1 = -1, temp2 = -1, temp3 = -1, temp4 = -1;
        for (int y = 0; y < tilesArray.GetLength(1); y++)
        {
            for (int x = 0; x < tilesArray.GetLength(0); x++)
            {
                if (tilesArray[x, y] == tile1)
                {
                    temp1 = x;
                    temp2 = y;
                }
                if (tilesArray[x, y] == tile2)
                {
                    temp3 = x;
                    temp4 = y;
                }
                if (temp1 > -1 && temp2 > -1 && temp3 > -1 && temp4 > -1)
                {
                    tilesArray[temp1, temp2] = tile2;
                    tilesArray[temp1, temp2].gameObject.name = "SwapBack [ " + temp1 + ", " + temp2 + " ]";
                    tilesArray[temp3, temp4] = tile1;
                    tilesArray[temp3, temp4].gameObject.name = "SwapBack [ " + temp3 + ", " + temp4 + " ]";

                    break;
                }
            }
        }
        Vector2 targetPosition1 = tile2.gameObject.transform.position;
        Vector2 targetPosition2 = tile1.gameObject.transform.position;

        for (int i = 0; i < 5; i++)
        {
            Vector2 currentPosition1 = tile1.gameObject.transform.position;
            Vector2 currentPosition2 = tile2.gameObject.transform.position;
            currentPosition1 += new Vector2((targetPosition1.x - tile1.gameObject.transform.position.x) / 5, (targetPosition1.y - tile1.gameObject.transform.position.y) / 5);
            currentPosition2 += new Vector2((targetPosition2.x - tile2.gameObject.transform.position.x) / 5, (targetPosition2.y - tile2.gameObject.transform.position.y) / 5);

            tile1.gameObject.transform.position = currentPosition1;
            tile2.gameObject.transform.position = currentPosition2;
            yield return new WaitForSeconds(.02f);
        }
        tile1.gameObject.transform.position = targetPosition1;
        tile2.gameObject.transform.position = targetPosition2;

        firstTile = null;
        secondTile = null;
    }

    public List<GameObject> FindingTheMatches(GameObject go1, GameObject go2, TileController[,] tilesArray)
    {
        List<GameObject> clearMatchList = new List<GameObject>();

        int x1 = -1, y1 = -1, x2 = -1, y2 = -1;
        for(int y = 0; y < BoardController.Instance.columnLength; y++)
        {
            for (int x = 0; x < BoardController.Instance.rowLength; x++)
            {
                if (tilesArray[x, y].gameObject == go1)
                {
                    x1 = x;
                    y1 = y;
                }
                if (tilesArray[x, y].gameObject == go2)
                {
                    x2 = x;
                    y2 = y;
                }
            }
        }

        List<GameObject> list1 = FindMatchFromSwappingTiles(go1.gameObject, x1, y1, BoardController.Instance.tiles);
        List<GameObject> list2 = FindMatchFromSwappingTiles(go2.gameObject, x2, y2, BoardController.Instance.tiles);

        clearMatchList.AddRange(list1);
        clearMatchList.AddRange(list2);
        
        return clearMatchList;
    }

    public List<GameObject> FindMatchFromSwappingTiles(GameObject go, int IndexX, int IndexY, TileController[,] tilesArray)
    {
        List<GameObject> checkingMatchListVertical = new List<GameObject>();
        List<GameObject> checkingMatchListHorizontal = new List<GameObject>();
        List<GameObject> totalList = new List<GameObject>();
        // Scanning up
        for (int i = 1; i <= IndexY; i++)
        {
            if (tilesArray[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == tilesArray[IndexX, IndexY - i].GetComponent<SpriteRenderer>().sprite)
            {
                checkingMatchListVertical.Add(tilesArray[IndexX, IndexY - i].gameObject);
            }
            else
                break;
        }
        //scanning down
        for (int i = 1; i < tilesArray.GetLength(1) - IndexY; i++)
        {
            if (tilesArray[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == tilesArray[IndexX, IndexY + i].GetComponent<SpriteRenderer>().sprite)
            {
                checkingMatchListVertical.Add(tilesArray[IndexX, IndexY + i].gameObject);
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
                checkingMatchListHorizontal.Add(tilesArray[IndexX - i, IndexY].gameObject);
            }
            else
                break;
        }
        // SCANNING RIGHT
        for (int i = 1; i < tilesArray.GetLength(0) - IndexX; i++)
        {
            if (tilesArray[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == tilesArray[IndexX + i, IndexY].GetComponent<SpriteRenderer>().sprite)
            {
                checkingMatchListHorizontal.Add(tilesArray[IndexX + i, IndexY].gameObject);
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

    public void FindAdjacentAndMatchIfPossible(Vector2 position1, Vector2 position2, TileController[,] tilesArray)
    {
        float swipeAngle = Mathf.Atan2(position2.y - position1.y, position2.x - position1.x) * 180 / Mathf.PI;            
        for (int y = 0; y < tilesArray.GetLength(1); y++)
        {
            for (int x = 0; x < tilesArray.GetLength(0); x++)
            {
                if (tilesArray[x, y].gameObject == firstTile)
                {
                    if (GetTheAdjacentTile(swipeAngle, x, y, BoardController.Instance.tiles))
                        OnTileSwapping(firstTile.GetComponent<TileController>(), secondTile.GetComponent<TileController>(), BoardController.Instance.tiles);

                    return;
                }
            }
        }
    }

    public void CheckAdjacent(Vector2 firstPosition, Vector2 lastPosition)
    {
        if (Vector2.Distance(firstPosition, lastPosition) >= 0.5f)
        {
            FindAdjacentAndMatchIfPossible(
                firstPosition,
                lastPosition,
                BoardController.Instance.tiles);
        }
    }
}
