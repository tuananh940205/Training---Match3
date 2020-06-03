using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
            Destroy(this.gameObject);
            return;
        }
        Instance = GetComponent<GameController>();

        offset = tile.GetComponent<SpriteRenderer>().bounds.size;
    }

    void CreateBoard()
    {
        boardControllerObject.CreateBoard(rowLength, columnLength, startPosition, offset, characters, tile);
    }

    private void Start()
    {
        ShowScore();
        TileController.onMouseUp += OnMouseUpHandler;
        TileController.onMouseDown += OnMouseDownHandler;
        CreateBoard();
    }

    private void OnMouseUpHandler(Vector2 pos1, Vector2 pos2)
    {
        Debug.LogFormat("Execute gameController.OnMouseUpHandler");
        Debug.LogFormat("aa" + boardControllerObject.tiles.Length);
        CheckAdjacent(pos1, pos2, boardControllerObject.tiles);
        
    }

    private void OnMouseDownHandler(TileController tile)
    {
        Debug.LogFormat("Execute OnMouseDownHandler");
        firstTile = tile;
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
        boardControllerObject.OnBoardFilled(characters, startPosition, offset, coroutineMap);
    }

    private void OnScoreChanged()
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
        //Debug.LogFormat("Execute FindMatchPassiveLy");
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

        return totalList;
    }

    private void ShowScore()
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
                if (tilesArray[xIndex + 1, yIndex] != null)
                    secondTile = tilesArray[xIndex + 1, yIndex];
            }
        }
        else if (radiant > 45 && radiant < 135)
        {
            //Debug.LogFormat("Up");
            if (yIndex > 0)
            {
                if (tilesArray[xIndex, yIndex - 1] != null)
                    secondTile = tilesArray[xIndex, yIndex - 1];
            }
        }
        else if (radiant > 135 || radiant < -135)
        {
            //Debug.LogFormat("Left");
            if (xIndex > 0)
            {
                if (tilesArray[xIndex - 1, yIndex] != null)
                    secondTile = tilesArray[xIndex - 1, yIndex];
            }
        }
        else if (radiant > -135 && radiant < -45)
        {
            // Debug.LogFormat("Down");
            if (yIndex < tilesArray.GetLength(1) - 1)
            {
                if (tilesArray[xIndex, yIndex + 1] != null)
                    secondTile = tilesArray[xIndex, yIndex + 1];
            }
        }
        if (secondTile != null)
            findTheAdjacent = true;
        else
            findTheAdjacent = false;

        return findTheAdjacent;
    }

    private void OnTileSwapping(TileController tile1, TileController tile2, TileController[,] tilesArray)
    {
        // Debug.LogFormat("Swapping");
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
                    tilesArray[temp1, temp2].gameObject.name = "Swap [ " + temp1 + ", " + temp2 + " ]";
                    tilesArray[temp3, temp4] = tile1;
                    tilesArray[temp3, temp4].gameObject.name = "Swap [ " + temp3 + ", " + temp4 + " ]";
                    break;
                }
            }
        }
        Vector2 targetPosition1 = tile2.gameObject.transform.position;
        Vector2 targetPosition2 = tile1.gameObject.transform.position;

        List<GameObject> listGameObject = new List<GameObject>();
        // StartCoroutine(SwappingTiles(go1, go2, targetPosition1, targetPosition2));
        tile1.transform.DOMove(targetPosition1, 0.35f);
        tile2.transform.DOMove(targetPosition2, 0.35f).OnComplete(() => HandleSwappingTileFinalSteps(FindingTheMatches(tile1.gameObject, tile2.gameObject, tilesArray), tile1, tile2));
    }

    private void HandleSwappingTileFinalSteps(List<GameObject> listGO, TileController tile1, TileController tile2)
    {
        if (listGO.Contains(tile1.gameObject) || listGO.Contains(tile2.gameObject))
            StartCoroutine(AllTilesFadeOut(listGO));
        else
            TileSwapBackOnMatchFailure(tile1, tile2, boardControllerObject.tiles);
    }

    
    private void TileSwapBackOnMatchFailure(TileController tile1, TileController tile2, TileController[,] tilesArray)
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

        tile1.transform.DOMove(targetPosition1, 0.35f).OnComplete(() => firstTile = null);
        tile2.transform.DOMove(targetPosition2, 0.35f).OnComplete(() => secondTile = null);
    }

    public List<GameObject> FindingTheMatches(GameObject go1, GameObject go2, TileController[,] tilesArray)
    {
        List<GameObject> clearMatchList = new List<GameObject>();

        int x1 = -1, y1 = -1, x2 = -1, y2 = -1;
        for(int y = 0; y < columnLength; y++)
        {
            for (int x = 0; x < rowLength; x++)
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

        List<GameObject> list1 = FindMatchFromSwappingTiles(go1.GetComponent<TileController>(), x1, y1, boardControllerObject.tiles);
        List<GameObject> list2 = FindMatchFromSwappingTiles(go2.GetComponent<TileController>(), x2, y2, boardControllerObject.tiles);

        clearMatchList.AddRange(list1);
        clearMatchList.AddRange(list2);
        
        return clearMatchList;
    }

    public List<GameObject> FindMatchFromSwappingTiles(TileController go, int IndexX, int IndexY, TileController[,] tilesArray)
    {
        List<GameObject> checkingMatchListVertical = new List<GameObject>();
        List<GameObject> checkingMatchListHorizontal = new List<GameObject>();
        List<GameObject> totalList = new List<GameObject>();
        // Scanning up
        for (int i = 1; i <= IndexY; i++)
        {
            if (tilesArray[IndexX, IndexY].SpriteRenderer.sprite == tilesArray[IndexX, IndexY - i].SpriteRenderer.sprite)
            {
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
                checkingMatchListHorizontal.Add(tilesArray[IndexX - i, IndexY].gameObject);
            else
                break;
        }
        // SCANNING RIGHT
        for (int i = 1; i < tilesArray.GetLength(0) - IndexX; i++)
        {
            if (tilesArray[IndexX, IndexY].SpriteRenderer.sprite == tilesArray[IndexX + i, IndexY].SpriteRenderer.sprite)
            {
                checkingMatchListHorizontal.Add(tilesArray[IndexX + i, IndexY].gameObject);
            }
            else
                break;
        }
        if (checkingMatchListHorizontal.Count >= 2)
            totalList.AddRange(checkingMatchListHorizontal);

        if (totalList.Count >= 2)
            totalList.Add(go.gameObject);

        return totalList;
    }

    public void FindAdjacentAndMatchIfPossible(Vector2 position1, Vector2 position2, TileController[,] tilesArray)
    {
        float swipeAngle = Mathf.Atan2(position2.y - position1.y, position2.x - position1.x) * 180 / Mathf.PI;
        Debug.LogFormat("swipeAngle = {0}", swipeAngle);
        Debug.LogFormat("arr1 = {0}", tilesArray.Length);
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

    public void CheckAdjacent(Vector2 firstPosition, Vector2 lastPosition, TileController[,] tilesArray)
    {
        Debug.LogFormat("Execute CheckAdjacent");
        if (Vector2.Distance(firstPosition, lastPosition) >= 0.5f)
        {
            Debug.LogFormat("first = {0}, last = {1}", firstPosition, lastPosition);
            FindAdjacentAndMatchIfPossible(firstPosition,lastPosition,tilesArray);
        }
    }
    private void OnDestroy()
    {
        Debug.LogFormat("Execute OnDestroy");
        TileController.onMouseUp -= OnMouseUpHandler;
        TileController.onMouseDown -= OnMouseDownHandler;
    }

    
}