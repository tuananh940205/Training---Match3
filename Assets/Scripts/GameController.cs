using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    private static GameController Instance;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text scoreRequireText;
    [SerializeField] private Text counterText;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text levelText;
    [SerializeField]private int score;
    [SerializeField] private int counter;
    private int scoreTarget;
    private TileController firstTile = null;
    private TileController secondTile = null;
    private Vector2 offset {get; set;}
    [SerializeField] private GameObject tile;
    [SerializeField] private int level;
    private int rowLength = 8;
    private int columnLength = 10;
    private Vector2 startPosition = new Vector2(-2.61f, 3.5f - 1.7f);
    private Dictionary<TileController, Coroutine> coroutineMap = new Dictionary<TileController, Coroutine>();
    [SerializeField] private BoardController boardControllerObject;
    // [SerializeField] private List<Sprite> sprites;
    // private Dictionary<name, Sprite> spriteDict = new Dictionary<name, Sprite>();
    // [SerializeField] private List<name> names;
    // private Dictionary <string, TileController[,]> tileBoardDictionary;
    public Data data;
    // private Dictionary <int, name> intnameDict = new Dictionary<int, name>();
    //private int[] tileIntNumberList;
    [SerializeField] TilePointData tilePointData;
    
    
    void Awake()
    {
        // data = new Data();
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = GetComponent<GameController>();
        offset = tile.GetComponent<SpriteRenderer>().bounds.size;

        // Test methods
        //Debug.LogFormat("GetEnumTest: {0}", (int)name.Flower);
    }

    void Start()
    {
        StartLevel();
        gameOverUI.SetActive(false);
        
        AddEvent();
        // AddDict();

        CreateBoard();
        DetectMatchExist();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            SceneManager.LoadScene(1);
    }

    void StartLevel()
    {
        level = 0;
        levelText.text = "Level " + (level + 1);
        // SetDictionaryData();

        TextAsset asset = Resources.Load("JsonData/LevelConfig") as TextAsset;
        TextAsset asset2 = Resources.Load("JsonData/TilePointsConfig") as TextAsset;
        
        if (asset != null)
        {
            data = JsonUtility.FromJson<Data>(asset.text);

            SetLevelValue(level);
        }
        else
        {
            Debug.Log("Asset is null");
        }

        if (asset2 != null)
        {
            tilePointData = JsonUtility.FromJson<TilePointData>(asset2.text);
        }
        else
        {
            Debug.Log("Asset2 is null");
        }
    }

    void SetLevelValue(int levelGame)
    {
        Debug.LogFormat("Execute SetLevelValue");
        score = 0;
        scoreTarget = data.items.levels[levelGame].scoreTarget;
        counter = data.items.levels[levelGame].counter;
        // tileIntNumberList = data.items.levels[levelGame].boards;
        Debug.LogFormat("score = {0}, scoreTarget = {1}, counter = {2}", score, scoreTarget, counter);
        scoreRequireText.text = "Target Score: " + scoreTarget.ToString();
        scoreText.text = "Score: " + score.ToString();
        counterText.text = counter.ToString();
        levelText.text = "Level " + (level + 1);
    }

    void ResetBoard(int level)
    {
        // boardControllerObject.ResetBoard(level, intArray);
        boardControllerObject.ResetBoard(level);
        
    }

    // void SetDictionaryData()
    // {
    //     for (int i = 0; i < sprites.Count; i++)
    //     {
    //         intnameDict.Add(i, (name)i);
    //     }
    // }
    void AddEvent()
    {
        TileController.onMouseUp += OnMouseUpHandler;
        TileController.onMouseDown += OnMouseDownHandler;
        BoardController.findMatchesPassively += FindMatchesPassivelyHandler;
        BoardController.clearAllPassiveMatches += ClearAllPassiveMatchesHandler;
    }

    void DetectMatchExist()
    {
        boardControllerObject.DetectMatchExist(boardControllerObject.MatchableTiles());
    }


    // void AddDict()
    // {
    //     for(int i = 0; i < sprites.Count; i++)
    //         spriteDict.Add(names[i], sprites[i]);
    // }

    private void CreateBoard()
    {
        //boardControllerObject.CreateBoard(rowLength, columnLength, startPosition, offset, tile, names, spriteDict);
        // boardControllerObject.CreateBoardByLevelInfo(
        //     rowLength, columnLength, startPosition, offset, tile, names, spriteDict, intnameDict, tileIntNumberList
        //     );
        boardControllerObject.CreateBoardWithIndexAndString(rowLength, columnLength, startPosition, offset, tile, data, tilePointData);
        
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

    //void ResetColorAndFillBoard(List<TileController> tilesList, int value)
    //{
    //    if (value == 0)
    //    {
    //        foreach (var tile in tilesList)
    //        {
    //            tile.gameObject.SetActive(false);
    //            tile.SpriteRenderer.color = new Color (1, 1, 1, 1);
    //            OnScoreChanged();
    //        }
    //        firstTile = null;
    //        secondTile = null;
    //        boardControllerObject.OnBoardFilled(startPosition, offset, coroutineMap);
    //    }
    //}

    public IEnumerator AllTilesFadeOut(List<TileController> tiles)
    {
        for (int i = 10; i > 0; i--)
        {
            foreach (var tile in tiles)
            {
                tile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i * .1f);
            }
            yield return new WaitForSeconds(.05f);
        }
        foreach (var tile in tiles)
        {
            tile.gameObject.SetActive(false);
            tile.SpriteRenderer.color = new Color(1, 1, 1, 1);
            string s = tile.SpriteRenderer.sprite.name;
            int temp = 0;

            TileDetails tileData = tilePointData.items.tileProperties.Find(x => x.id == s);
            temp = tileData.score;
            
            // for (int i = 0; i < tilePointData.items.tileProperties.Count; i++)
            // {
            //     if (s == tilePointData.items.tileProperties[i].id)
            //     {
            //         temp = tilePointData.items.tileProperties[i].score;
            //         break;
            //     }
            // }

            // Debug.LogFormat("string = {0}, int = {1}", s, temp);
            if (temp != 0)
            {
                OnScoreChanged(temp);
            }
                
        }
        firstTile = null;
        secondTile = null;

        if (score >= scoreTarget)
        {
            if (level < 9)
            {
                Debug.LogFormat("LevelComplete");
                GenerateNewLevelBoard(level);
            }
            else
                Debug.LogFormat("Victory");
            
        }
        else
        {
            if (counter > 0)
                boardControllerObject.OnBoardFilled(startPosition, offset, coroutineMap);
            else
                if (score < scoreTarget)
                    gameOverUI.SetActive(true);
        }
    }

    private void OnScoreChanged(int tileScore)
    {
        score += tileScore;
        scoreText.text = "Score: " + score.ToString();
    }

    private void ClearAllPassiveMatchesHandler(List<TileController> listGO, TileController[,] tilesList)
    {
        // Debug.LogFormat("ClearAllPassiveMatchesHandler");
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
                                {
                                    passivelyClearTileList.Add(tile);
                                }
                            }
                        }
                    }
                }
            }
        }
        // Debug.LogFormat("ClearAllPassiveMatchesHandler Count = {0}", passivelyClearTileList.Count);
        if (passivelyClearTileList.Count > 0)
        {
            StartCoroutine(AllTilesFadeOut(passivelyClearTileList));
        }
            
    }

    //public void ClearAllPassiveMatches(List<TileController> listGO, TileController[,] tilesList)
    //{
    //    List<TileController> passivelyClearTileList = new List<TileController>();
    //    foreach(var go in listGO)
    //    {
    //        for(int x = 0; x < tilesList.GetLength(0); x++)
    //        {
    //            for (int y = tilesList.GetLength(1) - 1; y >= 0; y--)
    //            {
    //                if (tilesList[x, y] == go)
    //                {
    //                    if (!passivelyClearTileList.Contains(tilesList[x, y]))
    //                    {
    //                        foreach(var tile in FindMatchesPassivelyHandler(tilesList[x, y], x, y, tilesList))
    //                        {
    //                            if (!passivelyClearTileList.Contains(tile))
    //                                passivelyClearTileList.Add(tile);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    if (passivelyClearTileList.Count > 0)
    //        StartCoroutine(AllTilesFadeOut(passivelyClearTileList));
    //}

    private List<TileController> FindMatchesPassivelyHandler(TileController tile, int IndexX, int IndexY, TileController[,] tilesArray)
    {
        // Debug.LogFormat("Execute FindMatchPassiveLy");
        List<TileController> checkingMatchListVertical = new List<TileController>();
        List<TileController> checkingMatchListHorizontal = new List<TileController>();
        List<TileController> totalList = new List<TileController>();
        // Scanning up
        for (int i = 1; i <= IndexY; i++)
        {
            if (tilesArray[IndexX, IndexY].name == tilesArray[IndexX, IndexY - i].name)
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
            if (tilesArray[IndexX, IndexY].name == tilesArray[IndexX, IndexY + i].name)
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
            if (tilesArray[IndexX, IndexY].name == tilesArray[IndexX - i, IndexY].name)
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
            if (tilesArray[IndexX, IndexY].name == tilesArray[IndexX + i, IndexY].name)
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
        // Debug.LogFormat("FindMatchesPassivelyHandler = {0}", totalList.Count);
        return totalList;
    }

    //public List<GameObject> FindMatchesPassively(GameObject go, int IndexX, int IndexY, TileController[,] tilesList)
    //{
    //    //Debug.LogFormat("Execute FindMatchPassiveLy");
    //    List<GameObject> checkingMatchListVertical = new List<GameObject>();
    //    List<GameObject> checkingMatchListHorizontal = new List<GameObject>();
    //    List<GameObject> totalList = new List<GameObject>();
    //    // Scanning up
    //    for (int i = 1; i <= IndexY; i++)
    //    {
    //        if (tilesList[IndexX, IndexY].name == tilesList[IndexX, IndexY - i].name)
    //        {
    //            //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", tiles[IndexX, IndexY - i].name, IndexX, IndexY - 1);
    //            checkingMatchListVertical.Add(tilesList[IndexX, IndexY - i].gameObject);
    //        }
    //        else
    //            break;
    //    }
    //    //scanning down
    //    for (int i = 1; i < tilesList.GetLength(1) - IndexY; i++)
    //    {
    //        if (tilesList[IndexX, IndexY].name == tilesList[IndexX, IndexY + i].name)
    //        {
    //            //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", tiles[IndexX, IndexY + i].name, IndexX, IndexY + 1);
    //            checkingMatchListVertical.Add(tilesList[IndexX, IndexY + i].gameObject);
    //        }
    //        else
    //            break;
    //    }
    //    if (checkingMatchListVertical.Count >= 2)
    //        totalList.AddRange(checkingMatchListVertical);
    //    // SCANNING LEFT
    //    for (int i = 1; i <= IndexX; i++)
    //    {
    //        if (tilesList[IndexX, IndexY].name == tilesList[IndexX - i, IndexY].name)
    //        {
    //            //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", tiles[IndexX - i, IndexY].name, IndexX - i, IndexY);
    //            checkingMatchListHorizontal.Add(tilesList[IndexX - i, IndexY].gameObject);
    //        }
    //        else
    //            break;
    //    }
    //    // SCANNING RIGHT
    //    for (int i = 1; i < tilesList.GetLength(0) - IndexX; i++)
    //    {
    //        if (tilesList[IndexX, IndexY].name == tilesList[IndexX + i, IndexY].name)
    //        {
    //            //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", BoardController.instance.tiles[IndexX + i, IndexY].name, IndexX + i, IndexY);
    //            checkingMatchListHorizontal.Add(tilesList[IndexX + i, IndexY].gameObject);
    //        }
    //        else
    //            break;
    //    }
    //    if (checkingMatchListHorizontal.Count >= 2)
    //        totalList.AddRange(checkingMatchListHorizontal);

    //    if (totalList.Count >= 2)
    //        totalList.Add(go);

    //    return totalList;
    //}

    
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
        // Debug.LogFormat("Swapping");
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
                    tilesList[temp3, temp4] = tile1;
                    tilesList[temp1, temp2] = tile2;
                    // tilesList[temp1, temp2].name = "Swap [ " + temp1 + ", " + temp2 + " ]";
                    // tilesList[temp3, temp4].name = "Swap [ " + temp3 + ", " + temp4 + " ]";
                    break;
                }
            }
        }
        Vector2 targetPosition1 = tile2.transform.position;
        Vector2 targetPosition2 = tile1.transform.position;

        List<TileController> matchesTileGrabList = FindingTheMatches(tile1, tile2, tilesList);

        tile1.transform.DOMove(targetPosition1, 0.35f);
        tile2.transform.DOMove(targetPosition2, 0.35f).OnComplete(() => HandleSwappingTileFinalSteps(matchesTileGrabList, tile1, tile2));
        // Debug.LogFormat("EndSwapFunction");
    }

    private void HandleSwappingTileFinalSteps(List<TileController> tilesList, TileController tile1, TileController tile2)
    {
        // Debug.LogFormat("HandleSwappingTileFinalSteps");
        // Debug.LogFormat("listCount = {0}", tilesList.Count);
        if (tilesList.Count > 2)
        {
            counter--;
            counterText.text = counter.ToString();
            StartCoroutine(AllTilesFadeOut(tilesList));
        }
            
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
                    // tilesList[temp1, temp2].name = "SwapBack [ " + temp1 + ", " + temp2 + " ]";
                    tilesList[temp3, temp4] = tile1;
                    // tilesList[temp3, temp4].name = "SwapBack [ " + temp3 + ", " + temp4 + " ]";

                    break;
                }
            }
        }
        Vector2 targetPosition1 = tile2.transform.position;
        Vector2 targetPosition2 = tile1.transform.position;

        tile1.transform.DOMove(targetPosition1, 0.35f).OnComplete(() => firstTile = null);
        tile2.transform.DOMove(targetPosition2, 0.35f).OnComplete(() => secondTile = null);
    }

    public List<TileController> FindingTheMatches(TileController tile1, TileController tile2, TileController[,] tilesList)
    {
        List<TileController> clearMatchList = new List<TileController>();

        for(int y = 0; y < columnLength; y++)
        {
            for (int x = 0; x < rowLength; x++)
            {
                if (tilesList[x, y] == tile1)
                    clearMatchList.AddRange(FindMatchFromSwappingTiles(tile1, x, y, boardControllerObject.tiles));
                if (tilesList[x, y] == tile2)
                    clearMatchList.AddRange(FindMatchFromSwappingTiles(tile2, x, y, boardControllerObject.tiles));
            }
        }
        // Debug.LogFormat("FindingTheMatches return {0}", clearMatchList.Count);
        return clearMatchList;
    }

    public List<TileController> FindMatchFromSwappingTiles(TileController tile, int IndexX, int IndexY, TileController[,] tilesArray)
    {
        // Debug.LogFormat("FindMatchfromSwappingTiles {0}", go.gameObject);
        List<TileController> checkingMatchListVertical = new List<TileController>();
        List<TileController> checkingMatchListHorizontal = new List<TileController>();
        List<TileController> totalList = new List<TileController>();
        // Scanning up
        // Debug.LogFormat("Step 1");
        for (int i = 1; i <= IndexY; i++)
        {
            if (tilesArray[IndexX, IndexY].name == tilesArray[IndexX, IndexY - i].name)
                checkingMatchListVertical.Add(tilesArray[IndexX, IndexY - i]);
            else
                break;

        }
        // Debug.LogFormat("Step 2");
        //scanning down
        for (int i = 1; i < tilesArray.GetLength(1) - IndexY; i++)
        {
            // Debug.LogFormat("Step 2.1");
            // Debug.LogFormat("tilesList.GetLength(1) = {0}, indexY = {1}", tilesList.GetLength(1), IndexY);

            if (tilesArray[IndexX, IndexY].name == tilesArray[IndexX, IndexY + i].name)
            {
                checkingMatchListVertical.Add(tilesArray[IndexX, IndexY + i]);
            }
            else
            {
                break;
            }
                
        }
        // Debug.LogFormat("Step 3");
        if (checkingMatchListVertical.Count >= 2)
            totalList.AddRange(checkingMatchListVertical);
        // SCANNING LEFT
        for (int i = 1; i <= IndexX; i++)
        {
            if (tilesArray[IndexX, IndexY].name == tilesArray[IndexX - i, IndexY].name)
                checkingMatchListHorizontal.Add(tilesArray[IndexX - i, IndexY]);
            else
                break;
        }
        // Debug.LogFormat("Step 4");
        // SCANNING RIGHT
        for (int i = 1; i < tilesArray.GetLength(0) - IndexX; i++)
        {
            if (tilesArray[IndexX, IndexY].name == tilesArray[IndexX + i, IndexY].name)
            {
                checkingMatchListHorizontal.Add(tilesArray[IndexX + i, IndexY]);
            }
            else
                break;
        }
        // Debug.LogFormat("Step 5");
        if (checkingMatchListHorizontal.Count >= 2)
            totalList.AddRange(checkingMatchListHorizontal);

        if (totalList.Count >= 2)
            totalList.Add(tile);

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
            FindAdjacentAndMatchIfPossible(firstPosition, lastPosition, tilesList);
        }
    }
    private void OnDestroy()
    {
        // Debug.LogFormat("Execute OnDestroy");
        TileController.onMouseUp -= OnMouseUpHandler;
        TileController.onMouseDown -= OnMouseDownHandler;
        BoardController.findMatchesPassively -= FindMatchesPassivelyHandler;
        BoardController.clearAllPassiveMatches -= ClearAllPassiveMatchesHandler;
    }

    void GenerateNewLevelBoard(int levelGame)
    {
        level = levelGame + 1;
        Debug.LogFormat("Generate new level {0}", level);
        boardControllerObject.GenerateNewLevelBoard(level);
        SetLevelValue(level);
        ResetBoard(level);
        
    }
}

