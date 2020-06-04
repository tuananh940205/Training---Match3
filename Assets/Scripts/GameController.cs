﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum  TileName
{
    Apple = 0,
    Oranage = 1
}

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public Text scoreText;
    public int score {get; private set;}
    public TileController firstTile = null;
    public TileController secondTile = null;
    public Vector2 offset {get; private set;}
    [SerializeField] public GameObject tile;
    public int rowLength { get { return 8; } }
    public int columnLength {get { return 10;} }
    public Vector2 startPosition = new Vector2(-2.61f, 3.5f);
    [SerializeField] public List<Sprite> characters = new List<Sprite>();
    private Dictionary<string, Coroutine> coroutineMap = new Dictionary<string, Coroutine>();
    [SerializeField] private BoardController boardControllerObject;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = GetComponent<GameController>();

        offset = tile.GetComponent<SpriteRenderer>().bounds.size;
    }

    private void Start()
    {
        ShowScore();
        TileController.onMouseUp += OnMouseUpHandler;
        TileController.onMouseDown += OnMouseDownHandler;
        BoardController.findMatchesPassively += FindMatchesPassivelyHandler;
        BoardController.clearAllPassiveMatches += ClearAllPassiveMatchesHandler;
        CreateBoard();
        boardControllerObject.DetectMatchExist(boardControllerObject.MatchableTiles());
    }

    private void CreateBoard()
    {
        boardControllerObject.CreateBoard(rowLength, columnLength, startPosition, offset, characters, tile);
    }

    private void OnMouseUpHandler(Vector2 pos1, Vector2 pos2)
    {
        //Debug.LogFormat("Execute gameController.OnMouseUpHandler");
        CheckAdjacent(pos1, pos2, boardControllerObject.tiles);
    }

    private void OnMouseDownHandler(TileController tile)
    {
        //Debug.LogFormat("Execute OnMouseDownHandler");
        firstTile = tile;
    }

    // void TilesFaded(List<TileController> tilesList)
    // {
    //     int temp = tilesList.Count;
    //     foreach (var tile in tilesList)
    //     {
    //         tile.SpriteRenderer.DOFade(0, 0.5f).
    //             OnComplete(() => temp--).
    //             OnComplete(() => ResetColorAndFillBoard(tilesList, temp));
            
    //     }
    // }

    void ResetColorAndFillBoard(List<TileController> tilesList, int value)
    {
        if (value == 0)
        {
            foreach (var tile in tilesList)
            {
                tile.gameObject.SetActive(false);
                tile.SpriteRenderer.color = new Color (1, 1, 1, 1);
                OnScoreChanged();
            }
            firstTile = null;
            secondTile = null;
            boardControllerObject.OnBoardFilled(characters, startPosition, offset, coroutineMap);
        }
    }

    public IEnumerator AllTilesFadeOut(List<TileController> tiles)
    {
        for (int i = 10; i > 0; i--)
        {
            foreach (var tile in tiles)
                tile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i * .1f);

            yield return new WaitForSeconds(.05f);
        }
        foreach (var tile in tiles)
        {
            tile.gameObject.SetActive(false);
            tile.SpriteRenderer.color = new Color(1, 1, 1, 1);
            OnScoreChanged();
        }
        firstTile = null;
        secondTile = null;
        boardControllerObject.OnBoardFilled(characters, startPosition, offset, coroutineMap);
    }

    private void OnScoreChanged()
    {
        score += 50;
        scoreText.text = "Score: " + score.ToString();
    }

    private void ClearAllPassiveMatchesHandler(List<TileController> listGO, TileController[,] tilesList)
    {
        Debug.LogFormat("ClearAllPassiveMatchesHandler");
        List<TileController> passivelyClearTileList = new List<TileController>();
        foreach (var go in listGO)
        {
            for (int x = 0; x < tilesList.GetLength(0); x++)
            {
                for (int y = tilesList.GetLength(1) - 1; y >= 0; y--)
                {
                    if (tilesList[x, y] == go)
                    {
                        if (!passivelyClearTileList.Contains(tilesList[x, y]))
                        {

                            foreach (var tile in FindMatchesPassivelyHandler(tilesList[x, y], x, y, tilesList))
                            {
                                if (!passivelyClearTileList.Contains(tile))
                                    passivelyClearTileList.Add(tile);
                            }
                        }
                    }
                }
            }
        }
        Debug.LogFormat("ClearAllPassiveMatchesHandler Count = {0}", passivelyClearTileList.Count);
        if (passivelyClearTileList.Count > 0)
            StartCoroutine(AllTilesFadeOut(passivelyClearTileList));
    }

    public void ClearAllPassiveMatches(List<TileController> listGO, TileController[,] tilesList)
    {
        List<TileController> passivelyClearTileList = new List<TileController>();
        foreach(var go in listGO)
        {
            for(int x = 0; x < tilesList.GetLength(0); x++)
            {
                for (int y = tilesList.GetLength(1) - 1; y >= 0; y--)
                {
                    if (tilesList[x, y] == go)
                    {
                        if (!passivelyClearTileList.Contains(tilesList[x, y]))
                        {
                            foreach(var tile in FindMatchesPassivelyHandler(tilesList[x, y], x, y, tilesList))
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

    private List<TileController> FindMatchesPassivelyHandler(TileController tile, int IndexX, int IndexY, TileController[,] tilesArray)
    {
        Debug.LogFormat("Execute FindMatchPassiveLy");
        List<TileController> checkingMatchListVertical = new List<TileController>();
        List<TileController> checkingMatchListHorizontal = new List<TileController>();
        List<TileController> totalList = new List<TileController>();
        // Scanning up
        for (int i = 1; i <= IndexY; i++)
        {
            if (tilesArray[IndexX, IndexY].SpriteRenderer.sprite == tilesArray[IndexX, IndexY - i].SpriteRenderer.sprite)
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
            if (tilesArray[IndexX, IndexY].SpriteRenderer.sprite == tilesArray[IndexX, IndexY + i].SpriteRenderer.sprite)
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
            if (tilesArray[IndexX, IndexY].SpriteRenderer.sprite == tilesArray[IndexX - i, IndexY].SpriteRenderer.sprite)
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
            if (tilesArray[IndexX, IndexY].SpriteRenderer.sprite == tilesArray[IndexX + i, IndexY].SpriteRenderer.sprite)
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
            totalList.Add(tile);
        Debug.LogFormat("FindMatchesPassivelyHandler = {0}", totalList.Count);
        return totalList;
    }

    public List<GameObject> FindMatchesPassively(GameObject go, int IndexX, int IndexY, TileController[,] tilesList)
    {
        //Debug.LogFormat("Execute FindMatchPassiveLy");
        List<GameObject> checkingMatchListVertical = new List<GameObject>();
        List<GameObject> checkingMatchListHorizontal = new List<GameObject>();
        List<GameObject> totalList = new List<GameObject>();
        // Scanning up
        for (int i = 1; i <= IndexY; i++)
        {
            if (tilesList[IndexX, IndexY].SpriteRenderer.sprite == tilesList[IndexX, IndexY - i].SpriteRenderer.sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", tiles[IndexX, IndexY - i].name, IndexX, IndexY - 1);
                checkingMatchListVertical.Add(tilesList[IndexX, IndexY - i].gameObject);
            }
            else
                break;
        }
        //scanning down
        for (int i = 1; i < tilesList.GetLength(1) - IndexY; i++)
        {
            if (tilesList[IndexX, IndexY].SpriteRenderer.sprite == tilesList[IndexX, IndexY + i].SpriteRenderer.sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", tiles[IndexX, IndexY + i].name, IndexX, IndexY + 1);
                checkingMatchListVertical.Add(tilesList[IndexX, IndexY + i].gameObject);
            }
            else
                break;
        }
        if (checkingMatchListVertical.Count >= 2)
            totalList.AddRange(checkingMatchListVertical);
        // SCANNING LEFT
        for (int i = 1; i <= IndexX; i++)
        {
            if (tilesList[IndexX, IndexY].SpriteRenderer.sprite == tilesList[IndexX - i, IndexY].SpriteRenderer.sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", tiles[IndexX - i, IndexY].name, IndexX - i, IndexY);
                checkingMatchListHorizontal.Add(tilesList[IndexX - i, IndexY].gameObject);
            }
            else
                break;
        }
        // SCANNING RIGHT
        for (int i = 1; i < tilesList.GetLength(0) - IndexX; i++)
        {
            if (tilesList[IndexX, IndexY].SpriteRenderer.sprite == tilesList[IndexX + i, IndexY].SpriteRenderer.sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", BoardController.instance.tiles[IndexX + i, IndexY].name, IndexX + i, IndexY);
                checkingMatchListHorizontal.Add(tilesList[IndexX + i, IndexY].gameObject);
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

    private void ShowScore()
    {
        score = 0;
        scoreText.text = "Score: " + score.ToString();
    }

    public bool GetTheAdjacentTile(float radiant, int xIndex, int yIndex, TileController[,] tilesList)
    {
        bool findTheAdjacent;
        if (radiant > -45 && radiant < 45)
        {
            //Debug.LogFormat("Right");
            if (xIndex < tilesList.GetLength(0) - 1)
            {
                if (tilesList[xIndex + 1, yIndex] != null)
                    secondTile = tilesList[xIndex + 1, yIndex];
            }
        }
        else if (radiant > 45 && radiant < 135)
        {
            //Debug.LogFormat("Up");
            if (yIndex > 0)
            {
                if (tilesList[xIndex, yIndex - 1] != null)
                    secondTile = tilesList[xIndex, yIndex - 1];
            }
        }
        else if (radiant > 135 || radiant < -135)
        {
            //Debug.LogFormat("Left");
            if (xIndex > 0)
            {
                if (tilesList[xIndex - 1, yIndex] != null)
                    secondTile = tilesList[xIndex - 1, yIndex];
            }
        }
        else if (radiant > -135 && radiant < -45)
        {
            // Debug.LogFormat("Down");
            if (yIndex < tilesList.GetLength(1) - 1)
            {
                if (tilesList[xIndex, yIndex + 1] != null)
                    secondTile = tilesList[xIndex, yIndex + 1];
            }
        }
        if (secondTile != null)
            findTheAdjacent = true;
        else
            findTheAdjacent = false;

        return findTheAdjacent;
    }

    private void OnTileSwapping(TileController tile1, TileController tile2, TileController[,] tilesList)
    {
        Debug.LogFormat("Swapping");
        int temp1 = -1, temp2 = -1, temp3 = -1, temp4 = -1;
        for (int y = 0; y < tilesList.GetLength(1); y++)
        {
            for (int x = 0; x < tilesList.GetLength(0); x++)
            {
                if (tilesList[x, y] == tile1)
                {
                    temp1 = x;
                    temp2 = y;
                }
                if (tilesList[x, y] == tile2)
                {
                    temp3 = x;
                    temp4 = y;
                }
                if (temp1 > -1 && temp2 > -1 && temp3 > -1 && temp4 > -1)
                {
                    tilesList[temp1, temp2] = tile2;
                    tilesList[temp1, temp2].gameObject.name = "Swap [ " + temp1 + ", " + temp2 + " ]";
                    tilesList[temp3, temp4] = tile1;
                    tilesList[temp3, temp4].gameObject.name = "Swap [ " + temp3 + ", " + temp4 + " ]";
                    break;
                }
            }
        }
        Vector2 targetPosition1 = tile2.gameObject.transform.position;
        Vector2 targetPosition2 = tile1.gameObject.transform.position;

        List<GameObject> listGameObject = new List<GameObject>();
        // StartCoroutine(SwappingTiles(go1, go2, targetPosition1, targetPosition2));
        tile1.transform.DOMove(targetPosition1, 0.35f);
        tile2.transform.DOMove(targetPosition2, 0.35f).OnComplete(() => HandleSwappingTileFinalSteps(FindingTheMatches(tile1, tile2, tilesList), tile1, tile2));
        Debug.LogFormat("EndSwapFunction");
    }

    private void HandleSwappingTileFinalSteps(List<TileController> tilesList, TileController tile1, TileController tile2)
    {
        Debug.LogFormat("HandleSwappingTileFinalSteps");
        Debug.LogFormat("listCount = {0}", tilesList.Count);
        if (tilesList.Contains(tile1) || tilesList.Contains(tile2))
            StartCoroutine(AllTilesFadeOut(tilesList));
        else
            TileSwapBackOnMatchFailure(tile1, tile2, boardControllerObject.tiles);
    }
    
    private void TileSwapBackOnMatchFailure(TileController tile1, TileController tile2, TileController[,] tilesList)
    {
        // Debug.LogFormat("Swap failed");
        int temp1 = -1, temp2 = -1, temp3 = -1, temp4 = -1;
        for (int y = 0; y < tilesList.GetLength(1); y++)
        {
            for (int x = 0; x < tilesList.GetLength(0); x++)
            {
                if (tilesList[x, y] == tile1)
                {
                    temp1 = x;
                    temp2 = y;
                }
                if (tilesList[x, y] == tile2)
                {
                    temp3 = x;
                    temp4 = y;
                }
                if (temp1 > -1 && temp2 > -1 && temp3 > -1 && temp4 > -1)
                {
                    tilesList[temp1, temp2] = tile2;
                    tilesList[temp1, temp2].gameObject.name = "SwapBack [ " + temp1 + ", " + temp2 + " ]";
                    tilesList[temp3, temp4] = tile1;
                    tilesList[temp3, temp4].gameObject.name = "SwapBack [ " + temp3 + ", " + temp4 + " ]";

                    break;
                }
            }
        }
        Vector2 targetPosition1 = tile2.gameObject.transform.position;
        Vector2 targetPosition2 = tile1.gameObject.transform.position;

        tile1.transform.DOMove(targetPosition1, 0.35f).OnComplete(() => firstTile = null);
        tile2.transform.DOMove(targetPosition2, 0.35f).OnComplete(() => secondTile = null);
    }

    public List<TileController> FindingTheMatches(TileController tile1, TileController tile2, TileController[,] tilesList)
    {
        List<TileController> clearMatchList = new List<TileController>();

        int x1 = -1, y1 = -1, x2 = -1, y2 = -1;
        for(int y = 0; y < columnLength; y++)
        {
            for (int x = 0; x < rowLength; x++)
            {
                if (tilesList[x, y] == tile1)
                {
                    x1 = x;
                    y1 = y;
                    clearMatchList.AddRange(FindMatchFromSwappingTiles(tile1, x, y, boardControllerObject.tiles));
                }
                if (tilesList[x, y] == tile2)
                {
                    x2 = x;
                    y2 = y;
                    clearMatchList.AddRange(FindMatchFromSwappingTiles(tile2, x, y, boardControllerObject.tiles));
                }
            }
        }

        // List<TileController> list1 = FindMatchFromSwappingTiles(go1, x1, y1, boardControllerObject.tiles);
        // List<TileController> list2 = FindMatchFromSwappingTiles(go2, x2, y2, boardControllerObject.tiles);

        // clearMatchList.AddRange(list1);
        // clearMatchList.AddRange(list2);
        Debug.LogFormat("FindingTheMatches return {0}", clearMatchList.Count);
        return clearMatchList;
    }

    public List<TileController> FindMatchFromSwappingTiles(TileController go, int IndexX, int IndexY, TileController[,] tilesList)
    {
        Debug.LogFormat("FindMatchfromSwappingTiles {0}", go.gameObject);
        List<TileController> checkingMatchListVertical = new List<TileController>();
        List<TileController> checkingMatchListHorizontal = new List<TileController>();
        List<TileController> totalList = new List<TileController>();
        // Scanning up
        Debug.LogFormat("Step 1");
        for (int i = 1; i <= IndexY; i++)
        {
            if (tilesList[IndexX, IndexY].SpriteRenderer.sprite == tilesList[IndexX, IndexY - i].SpriteRenderer.sprite)
            {
                checkingMatchListVertical.Add(tilesList[IndexX, IndexY - i]);
            }
            else
                break;
        }
        Debug.LogFormat("Step 2");
        //scanning down
        for (int i = 1; i < tilesList.GetLength(1) - IndexY; i++)
        {
            Debug.LogFormat("Step 2.1");
            Debug.LogFormat("tilesList.GetLength(1) = {0}, indexY = {1}", tilesList.GetLength(1), IndexY);
            if (tilesList[IndexX, IndexY].SpriteRenderer.sprite == tilesList[IndexX, IndexY + i].SpriteRenderer.sprite)
            {
                Debug.LogFormat("Step 2.2.{0}",i);
                checkingMatchListVertical.Add(tilesList[IndexX, IndexY + i]);
            }
            else
                break;
        }
        Debug.LogFormat("Step 3");
        if (checkingMatchListVertical.Count >= 2)
            totalList.AddRange(checkingMatchListVertical);
        // SCANNING LEFT
        for (int i = 1; i <= IndexX; i++)
        {
            if (tilesList[IndexX, IndexY].SpriteRenderer.sprite == tilesList[IndexX - i, IndexY].SpriteRenderer.sprite)
                checkingMatchListHorizontal.Add(tilesList[IndexX - i, IndexY]);
            else
                break;
        }
        Debug.LogFormat("Step 4");
        // SCANNING RIGHT
        for (int i = 1; i < tilesList.GetLength(0) - IndexX; i++)
        {
            if (tilesList[IndexX, IndexY].SpriteRenderer.sprite == tilesList[IndexX + i, IndexY].SpriteRenderer.sprite)
            {
                checkingMatchListHorizontal.Add(tilesList[IndexX + i, IndexY]);
            }
            else
                break;
        }
        Debug.LogFormat("Step 5");
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
                if (tilesArray[x, y] == firstTile)
                {
                    if (GetTheAdjacentTile(swipeAngle, x, y, tilesArray))
                        OnTileSwapping(firstTile, secondTile, tilesArray);

                    return;
                }
            }
        }
    }

    public void CheckAdjacent(Vector2 firstPosition, Vector2 lastPosition, TileController[,] tilesList)
    {
        //Debug.LogFormat("Execute CheckAdjacent");
        if (Vector2.Distance(firstPosition, lastPosition) >= 0.5f)
        {
            //Debug.LogFormat("first = {0}, last = {1}", firstPosition, lastPosition);
            FindAdjacentAndMatchIfPossible(firstPosition,lastPosition,tilesList);
        }
    }
    private void OnDestroy()
    {
        Debug.LogFormat("Execute OnDestroy");
        TileController.onMouseUp -= OnMouseUpHandler;
        TileController.onMouseDown -= OnMouseDownHandler;
        BoardController.findMatchesPassively -= FindMatchesPassivelyHandler;
        BoardController.clearAllPassiveMatches -= ClearAllPassiveMatchesHandler;
    }    
}

