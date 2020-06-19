using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardController : MonoBehaviour
{
    public TileController[,] tiles { get; private set; }
    // private List<name> listSwapContainer = new List<name>();
    private int row, column;
    private Vector2 startPosition = new Vector2(-2.61f, 3.5f);
    private Vector2 offset;
    private GameObject tile;
    // private  List<name> names;
    public delegate List<TileController> FindMatchesPassivelyEvent(TileController tile, int indexX, int indexY, TileController[,] tilesArray);
    public static FindMatchesPassivelyEvent findMatchesPassively;
    public delegate void ClearAllPassiveMatchesEvent(List<TileController> listGo, TileController[,] tilesArray);
    public static ClearAllPassiveMatchesEvent clearAllPassiveMatches;
    // private Dictionary<name, Sprite> spriteDict = new Dictionary<name, Sprite>();
    // private Dictionary<int, name> intTileDict;
    private int[] intTileArray;
    private int levelBoard;
    private Data data;
    private TilePointData tilePointData;
    // private TilePointData tilePointData;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            string text = "";
            for (int y = 0; y < column; y++)
            {
                for (int x = 0; x < row; x++)
                    text = text + tiles[x, y].name + " ";
            }
            Debug.Log(text);
        }
    }

    // Create board
    // public void CreateBoard(int _row, int _column, Vector2 _startPosition, Vector2 _offset, GameObject _tile, List<name> _names, Dictionary<name, Sprite> _spriteDict, Data _data)
    // {
    //     spriteDict = _spriteDict;
    //     row = _row;
    //     column = _column;
    //     startPosition = _startPosition;
    //     offset = _offset;
    //     tiles = new TileController[row, column];
    //     tile = _tile;
    //     names = _names;
    //     BoardData[] boardData = data.items.levels[0].boards;

    //     // tile = Resources.Load()
    //     for (int y = 0; y < column; y++)
    //     {
    //         for (int x = 0; x < row; x++)
    //         {
    //             GameObject newTile = Instantiate(tile, new Vector3(startPosition.x + (offset.x * x), startPosition.y - (offset.y * y), 0), tile.transform.rotation);
    //             tiles[x, y] = newTile.GetComponent<TileController>();
    //             newTile.name = "[ " + x + " , " + y + " ]";
    //             tiles[x, y].transform.parent = transform;

    //             List<name> listname = new List<name>();
    //             listname.AddRange(names);

    //             if (x > 0)
    //                     listname.Remove(tiles[x - 1, y].name);
    //             if (y > 0)
    //                     listname.Remove(tiles[x, y - 1].name);

    //             name name = listname[UnityEngine.Random.Range(0, listname.Count)];
    //             // Debug.LogFormat("x = {0}, y = {1}, tilesNames count = {2}", x, y , listname.Count);
    //             tiles[x, y].name = name;
    //             newTile.name = name.ToString();
    //             tiles[x, y].SpriteRenderer.sprite = spriteDict[name];
    //         }
    //     }
    // }

    // Create board by level
    // public void CreateBoardByLevelInfo(int _row, int _column, Vector2 _startPosition, Vector2 _offset, GameObject _tile, List<name> _names, Dictionary<name, Sprite> _spriteDict, Dictionary<int, name> intnameDict, int[] intTileArrays, Data _data)
    // {
    //     data = _data;
    //     spriteDict = _spriteDict;
    //     row = _row;
    //     column = _column;
    //     startPosition = _startPosition;
    //     offset = _offset;
    //     tiles = new TileController[row, column];
    //     tile = _tile;
    //     names = _names;
    //     intTileDict = intnameDict;
    //     intTileArray = intTileArrays;

    //     for (int y = 0; y < column; y++)
    //     {
    //         for (int x = 0; x < row; x++)
    //         {
    //             GameObject newTile = Instantiate(tile, new Vector3(startPosition.x + (offset.x * x), startPosition.y - (offset.y * y), 0), tile.transform.rotation);
    //             tiles[x, y] = newTile.GetComponent<TileController>();
                
    //             // string spriteId = BillboardAsset.Find ( e => e.x == x && e.y == y ).id;
    //             // string spriteName = objects.Find ( e => e.id == spriteId).spriteName;
    //             // SpriteRenderer sprite = Resources.Load<SpriteRenderer>()
    //             // tile[x,y].spriterRedner = sprite;
    //             // tile[x,y].score = object.score;

    //             //newTile.name = "[ " + x + " , " + y + " ]";
                
    //             // Ba vì có con bò vàng


    //             int soNguyenDaiDienChoTileTrongTuDien = intTileArray[x + y * row];
    //             name name = intTileDict[soNguyenDaiDienChoTileTrongTuDien];

    //             tiles[x, y].name = name;
    //             // Get the sprite through index
    //             tiles[x, y].SpriteRenderer.sprite = Resources.Load<Sprite>("Character/" + (int)name); //spriteDict[name];
    //             newTile.name = soNguyenDaiDienChoTileTrongTuDien.ToString();
    //             //name theTile = intTileDict[]

    //         }
    //     }
    // }

    public void CreateBoardWithIndexAndString(int _row, int _column, Vector2 _startPosition, Vector2 _offset, GameObject _tile, int[] intTileArrays, Data _data, TilePointData _tilePointData)
    {
        // spriteDict = _spriteDict;
        row = _row;
        column = _column;
        startPosition = _startPosition;
        offset = _offset;
        tiles = new TileController[row, column];
        tile = _tile;
        // names = _names;
        // intTileDict = intnameDict;
        intTileArray = intTileArrays;
        data = _data;
        tilePointData = _tilePointData;

        for (int y = 0; y < column; y++)
        {
            for (int x = 0; x < row; x++)
            {
                int posX = data.items.levels[0].boards[x + y * row].x;
                int posY = data.items.levels[0].boards[x + y * row].y;
                // Debug.Log(data.items.levels[0].boards[x * 8 + y].x);

                GameObject newTile = Instantiate(tile, new Vector3(startPosition.x + (offset.x * posX), startPosition.y - (offset.y * posY), 0), tile.transform.rotation);
                tiles[x, y] = newTile.GetComponent<TileController>();
                // int soNguyenDaiDienChoTileTrongTuDien = ;
                string nameLoaded = data.items.levels[0].boards[x + y * row].tileId;
                tiles[x, y].name = nameLoaded;
                TileDetails tileData = tilePointData.items.tileProperties.Find( e => e.id == nameLoaded);
                tiles[x, y].SpriteRenderer.sprite = Resources.Load<Sprite>(tileData.spriteName);
                // Get the sprite through index
                newTile.name  = tileData.spriteName;

            
            }
        }
    }
    
    private IEnumerator MoveTilesDown(TileController go, int indexX, int indexY, Vector2 startPosition, Vector2 offset, TileController[,] tilesArray)
    {
        Vector2 finalPosition = new Vector2(go.transform.position.x, startPosition.y - offset.y * indexY);
        while (Vector2.Distance(go.transform.position, finalPosition) >= .1f)
        {
            go.transform.position = Vector2.MoveTowards(go.transform.position, finalPosition, 0.25f);
            if (go.transform.position.y <= startPosition.y + offset.y)
                go.gameObject.SetActive(true);
            yield return new WaitForSeconds(.05f);
        }
        go.transform.position = finalPosition;
        List<TileController> listmatch;
        if (findMatchesPassively != null && clearAllPassiveMatches != null)
        {
            listmatch = findMatchesPassively(go, indexX, indexY, tilesArray);
            clearAllPassiveMatches(listmatch, tilesArray);
        }
        //List<GameObject> listmatch = gameControllerObject.FindMatchesPassively(go, indexX, indexY, tiles);
        //gameControllerObject.ClearAllPassiveMatches(listmatch, tilesArray);

        DetectMatchExist(MatchableTiles());
    }

    public List<TileController> MatchableTiles()
    {
        // Debug.LogFormat("Execute Find the match exist");
        List<TileController> listCanBeMatch = new List<TileController>();
        for (int y = 0; y < column; y++)
        {
            for (int x = 0; x < row; x++)
            {
                if (y < column - 3)
                {
                    if (tiles[x, y].name == tiles[x, y + 2].name &&
                    tiles[x, y].name == tiles[x, y + 3].name)
                    {
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x, y + 2]);
                        listCanBeMatch.Add(tiles[x, y + 3]);
                    }

                    if (tiles[x, y].name == tiles[x, y + 1].name &&
                        tiles[x, y].name == tiles[x, y + 3].name)
                    {
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x, y + 1]);
                        listCanBeMatch.Add(tiles[x, y + 3]);
                    }
                }

                if (x < row - 3)
                {
                    if (tiles[x, y].name == tiles[x + 2, y].name &&
                        tiles[x, y].name == tiles[x + 3, y].name)
                    {
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x + 2, y]);
                        listCanBeMatch.Add(tiles[x + 3, y]);
                    }

                    if (tiles[x, y].name == tiles[x + 1, y].name &&
                        tiles[x, y].name == tiles[x + 3, y].name)
                    {
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x + 1, y]);
                        listCanBeMatch.Add(tiles[x + 3, y]);
                    }
                }

                // new write method
                if (x < row - 1 && y < column - 2)
                {
                    if (tiles[x, y].name == tiles[x + 1, y + 1].name &&
                            tiles[x, y].name == tiles[x, y + 2].name)
                    {
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x + 1, y + 1]);
                        listCanBeMatch.Add(tiles[x, y + 2]);
                    }

                    if (tiles[x + 1, y].name == tiles[x, y + 1].name &&
                        tiles[x + 1, y].name == tiles[x + 1, y + 2].name)
                    {
                        listCanBeMatch.Add(tiles[x + 1, y]);
                        listCanBeMatch.Add(tiles[x, y + 1]);
                        listCanBeMatch.Add(tiles[x + 1, y + 2]);
                    }

                    if (tiles[x, y].name == tiles[x + 1, y + 1].name &&
                        tiles[x, y].name == tiles[x + 1, y + 2].name)
                    {
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x + 1, y + 1]);
                        listCanBeMatch.Add(tiles[x + 1, y + 2]);
                    }

                    if (tiles[x + 1, y].name == tiles[x, y + 1].name &&
                        tiles[x + 1, y].name == tiles[x, y + 2].name)
                    {
                        listCanBeMatch.Add(tiles[x + 1, y]);
                        listCanBeMatch.Add(tiles[x, y + 1]);
                        listCanBeMatch.Add(tiles[x, y + 2]);
                    }

                    if (tiles[x + 1, y].name == tiles[x + 1, y + 1].name &&
                        tiles[x + 1, y].name == tiles[x, y + 2].name)
                    {
                        listCanBeMatch.Add(tiles[x + 1, y]);
                        listCanBeMatch.Add(tiles[x + 1, y + 1]);
                        listCanBeMatch.Add(tiles[x, y + 2]);
                    }

                    if (tiles[x, y].name == tiles[x, y + 1].name &&
                        tiles[x, y].name == tiles[x + 1, y + 2].name)
                    {
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x, y + 1]);
                        listCanBeMatch.Add(tiles[x + 1, y + 2]);
                    }
                }

                if (x < row - 2 && y < column - 1)
                {
                    if (tiles[x, y].name == tiles[x + 1, y + 1].name &&
                        tiles[x, y].name == tiles[x + 2, y + 1].name)
                    {
                        listCanBeMatch.Add(tiles[x + 1, y + 1]);
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x + 2, y + 1]);
                    }

                    if (tiles[x, y + 1].name == tiles[x + 1, y + 1].name &&
                        tiles[x, y + 1].name == tiles[x + 2, y].name)
                    {
                        listCanBeMatch.Add(tiles[x, y + 1]);
                        listCanBeMatch.Add(tiles[x + 1, y + 1]);
                        listCanBeMatch.Add(tiles[x + 2, y]);
                    }



                    if (tiles[x, y + 1].name == tiles[x + 1, y].name &&
                        tiles[x, y + 1].name == tiles[x + 2, y].name)
                    {
                        listCanBeMatch.Add(tiles[x, y + 1]);
                        listCanBeMatch.Add(tiles[x + 1, y]);
                        listCanBeMatch.Add(tiles[x + 2, y]);
                    }

                    if (tiles[x, y].name == tiles[x + 1, y].name &&
                        tiles[x, y].name == tiles[x + 2, y + 1].name)
                    {
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x + 1, y]);
                        listCanBeMatch.Add(tiles[x + 2, y + 1]);
                    }

                    if (tiles[x, y].name == tiles[x + 2, y].name &&
                        tiles[x, y].name == tiles[x + 1, y + 1].name)
                    {
                        listCanBeMatch.Add(tiles[x, y]);
                        listCanBeMatch.Add(tiles[x + 2, y]);
                        listCanBeMatch.Add(tiles[x + 1, y + 1]);
                    }

                    if (tiles[x, y + 1].name == tiles[x + 1, y].name &&
                        tiles[x, y + 1].name == tiles[x + 2, y + 1].name)
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

    // Find Passive Match
    public void DetectMatchExist(List<TileController> listGO)
    {
        if (listGO.Count > 0)
        {
            // Debug.LogFormat("We found the Match!");
            foreach (var go in listGO)
                go.SpriteRenderer.color = new Color(.5f, .5f, .5f, 1);
        }
        else
        {
            //Debug.LogFormat("Match 404 not found. Reset the board");
            ShuffleBoard();
        }
    }

    //ShuffleBoard
    void ShuffleBoard()
    {
        for (int y = 0; y < column; y++)
        {
            for (int x = 0; x < row; x++)
            {
                // listSwapContainer.Add(tiles[x, y].name);
            }
        }

        for (int y = 0; y < column; y++)
        {
            for (int x = 0; x < row; x++)
            {
                // name go = listSwapContainer[UnityEngine.Random.Range(0, listSwapContainer.Count)];
                // tiles[x, y].name = go;
                // tiles[x, y].SpriteRenderer.sprite = spriteDict[go];
                // listSwapContainer.Remove(go);
            }
        }

        for (int y = 0; y < column; y++)
        {
            for (int x = 0; x < row; x++)
            {
                if (findMatchesPassively != null && clearAllPassiveMatches != null)
                {
                    List<TileController> listMatch = findMatchesPassively(tiles[x, y], x, y, tiles);
                    clearAllPassiveMatches(listMatch, tiles);
                    FindAndClearMatchPassively(x, y);
                }
                //List<GameObject> listMatch = gameControllerObject.FindMatchesPassively(tiles[x, y].gameObject, x, y, tiles);
                //gameControllerObject.ClearAllPassiveMatches(listMatch, tiles);
            }
        }
    }
    public void FindAndClearMatchPassively(int xIndex, int yIndex)
    {
        List<TileController> listMatch = new List<TileController>();
        if (findMatchesPassively != null)
            listMatch = findMatchesPassively(tiles[xIndex, yIndex], xIndex, yIndex, tiles);
        //List<GameObject> listMatch = gameControllerObject.FindMatchesPassively(tiles[xIndex, yIndex].gameObject, xIndex, yIndex, tiles);
        if (listMatch.Count >= 3 && clearAllPassiveMatches != null)
            //gameControllerObject.ClearAllPassiveMatches(listMatch, tiles);
            clearAllPassiveMatches(listMatch, tiles);
        
    }
    
    // Fill board
    public void OnBoardFilled(Vector2 startPosition, Vector2 offsetPosition, Dictionary<string, Coroutine> coroutineMap)
    {
        for (int y = 0; y < column; y++)
        {
            for (int x = 0; x < row; x++)
            {
                if (tiles[x, y].SpriteRenderer.color != new Color (1, 1, 1, 1))
                    tiles[x, y].SpriteRenderer.color = new Color (1, 1, 1, 1);
            }
        }

        for (int x = 0; x < row; x++)
        {
            //Debug.LogFormat("Execute GetNewUpperTiles2");
            List<TileController> listTotal = new List<TileController>();
            List<TileController> nullList = new List<TileController>();

            int nullCount = 0;
            for (int y = column - 1; y >= 0; y--)
            {
                if (tiles[x, y].gameObject.activeSelf)
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
                //nullList[i].SpriteRenderer.sprite = listSprite[Random.Range(0, listSprite.Count)];
                nullList[i].transform.position = new Vector2(nullList[i].transform.position.x, startPosition.y + (i + 1) * offsetPosition.y);

                // nullList[i].name = names[UnityEngine.Random.Range(0, names.Count)];
                
                // nullList[i].SpriteRenderer.sprite = spriteDict[nullList[i].name];

                string[] nameStringArray = new string[]{"Milk", "Apple", "Orange", "Bread", "Vegetable", "Coconut", "Flower" };
                string nameString = nameStringArray[UnityEngine.Random.Range(0, nameStringArray.Length)];
                // nullList[i].name = (name)Enum.Parse(typeof(name), nameString);
                nullList[i].SpriteRenderer.sprite = (Resources.Load<Sprite>(nameString));
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
                coroutineMap[tiles[x, i].name] = StartCoroutine(MoveTilesDown(tiles[x, i], x, i, startPosition, offsetPosition, tiles));
            }
        }
    }

    public void GenerateNewLevelBoard(int level)
    {
        levelBoard = level + 1;

        for (int y = 0; y < column; y++)
        {
            for (int x = 0; x < row; x++)
            {
                if (tiles[x, y].gameObject.activeSelf == false)
                    tiles[x, y].gameObject.SetActive(true);
            }
        }
    }

    public void ResetBoard(int levelGame)
    {
        Debug.LogFormat("New level {0}", levelGame);
        for (int y = 0; y < column; y++)
        {
            for (int x = 0; x < row; x++)
            {
                // string nameLoaded = data.items.levels[levelGame].boards[x + y * row].tileId;
                string nameLoaded = data.items.levels[levelGame].boards[x + y * row].tileId;
                // name name = (name)Enum.Parse(typeof(name), nameLoaded);

                // tiles[x, y].name = name;
                Debug.LogFormat("tileSprite {0}, {1}, {2} ",x ,y ,data.items.levels[levelGame].boards[x + y * row].tileId);
                tiles[x, y].SpriteRenderer.sprite = Resources.Load<Sprite>(data.items.levels[levelGame].boards[x + y * row].tileId);;
            }
        }
        DetectMatchExist(MatchableTiles());
    }
}