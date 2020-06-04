using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    //[SerializeField] private GameController gameControllerObject;
    public TileController[,] tiles;
    private List<Sprite> listSwapContainer = new List<Sprite>();
    private int row, column;
    private Vector2 startPosition = new Vector2(-2.61f, 3.5f);
    private Vector2 offset;
    private GameObject tile;
    private List<Sprite> characters;
    public delegate List<TileController> FindMatchesPassivelyEvent(TileController tile, int indexX, int indexY, TileController[,] tilesArray);
    public static FindMatchesPassivelyEvent findMatchesPassively;
    public delegate void ClearAllPassiveMatchesEvent(List<TileController> listGo, TileController[,] tilesArray);
    public static ClearAllPassiveMatchesEvent clearAllPassiveMatches;

    // Create board
    public void CreateBoard(int _row, int _column, Vector2 _startPosition, Vector2 _offset, List<Sprite> _characters, GameObject _tile)
    {
        row = _row;
        column = _column;
        startPosition = _startPosition;
        offset = _offset;
        characters = _characters;
        tiles = new TileController[row, column];
        tile = _tile;

       // tile = Resources.Load()
        for (int y = 0; y < column; y++)
        {
            for (int x = 0; x < row; x++)
            {
                GameObject newTile = Instantiate(tile, new Vector3(startPosition.x + (offset.x * x), startPosition.y - (offset.y * y), 0), tile.transform.rotation);
                tiles[x, y] = newTile.GetComponent<TileController>();
                newTile.name = "[ " + x + " , " + y + " ]";
                tiles[x, y].transform.parent = transform;

                List<Sprite> possibleCharacters = new List<Sprite>();
                possibleCharacters.AddRange(characters);

                if (x > 0)
                    if (tiles[x - 1, y] != null)
                        possibleCharacters.Remove(tiles[x - 1, y].SpriteRenderer.sprite);
                if (y > 0)
                    if (tiles[x, y - 1] != null)
                        possibleCharacters.Remove(tiles[x, y - 1].SpriteRenderer.sprite);

                Sprite newSprite = possibleCharacters[Random.Range(0, possibleCharacters.Count)];
                newTile.GetComponent<SpriteRenderer>().sprite = newSprite;
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

    public List<GameObject> MatchableTiles()
    {
        // Debug.LogFormat("Execute Find the match exist");
        List<GameObject> listCanBeMatch = new List<GameObject>();
        for (int y = 0; y < column; y++)
        {
            for (int x = 0; x < row; x++)
            {
                if (y < column - 3)
                {
                    if (tiles[x, y].SpriteRenderer.sprite == tiles[x, y + 2].SpriteRenderer.sprite &&
                    tiles[x, y].SpriteRenderer.sprite == tiles[x, y + 3].SpriteRenderer.sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y].gameObject);
                        listCanBeMatch.Add(tiles[x, y + 2].gameObject);
                        listCanBeMatch.Add(tiles[x, y + 3].gameObject);
                    }

                    if (tiles[x, y].SpriteRenderer.sprite == tiles[x, y + 1].SpriteRenderer.sprite &&
                        tiles[x, y].SpriteRenderer.sprite == tiles[x, y + 3].SpriteRenderer.sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y].gameObject);
                        listCanBeMatch.Add(tiles[x, y + 1].gameObject);
                        listCanBeMatch.Add(tiles[x, y + 3].gameObject);
                    }
                }

                if (x < row - 3)
                {
                    if (tiles[x, y].SpriteRenderer.sprite == tiles[x + 2, y].SpriteRenderer.sprite &&
                        tiles[x, y].SpriteRenderer.sprite == tiles[x + 3, y].SpriteRenderer.sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y].gameObject);
                        listCanBeMatch.Add(tiles[x + 2, y].gameObject);
                        listCanBeMatch.Add(tiles[x + 3, y].gameObject);
                    }

                    if (tiles[x, y].SpriteRenderer.sprite == tiles[x + 1, y].SpriteRenderer.sprite &&
                        tiles[x, y].SpriteRenderer.sprite == tiles[x + 3, y].SpriteRenderer.sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y].gameObject);
                        listCanBeMatch.Add(tiles[x + 1, y].gameObject);
                        listCanBeMatch.Add(tiles[x + 3, y].gameObject);
                    }
                }

                // new write method
                if (x < row - 1 && y < column - 2)
                {
                    if (tiles[x, y].SpriteRenderer.sprite == tiles[x + 1, y + 1].SpriteRenderer &&
                            tiles[x, y].SpriteRenderer.sprite == tiles[x, y + 2].SpriteRenderer.sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y].gameObject);
                        listCanBeMatch.Add(tiles[x + 1, y + 1].gameObject);
                        listCanBeMatch.Add(tiles[x, y + 2].gameObject);
                    }

                    if (tiles[x + 1, y].SpriteRenderer.sprite == tiles[x, y + 1].SpriteRenderer.sprite &&
                        tiles[x + 1, y].SpriteRenderer.sprite == tiles[x + 1, y + 2].SpriteRenderer.sprite)
                    {
                        listCanBeMatch.Add(tiles[x + 1, y].gameObject);
                        listCanBeMatch.Add(tiles[x, y + 1].gameObject);
                        listCanBeMatch.Add(tiles[x + 1, y + 2].gameObject);
                    }

                    if (tiles[x, y].SpriteRenderer.sprite == tiles[x + 1, y + 1].SpriteRenderer.sprite &&
                        tiles[x, y].SpriteRenderer.sprite == tiles[x + 1, y + 2].SpriteRenderer.sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y].gameObject);
                        listCanBeMatch.Add(tiles[x + 1, y + 1].gameObject);
                        listCanBeMatch.Add(tiles[x + 1, y + 2].gameObject);
                    }

                    if (tiles[x + 1, y].SpriteRenderer.sprite == tiles[x, y + 1].SpriteRenderer.sprite &&
                        tiles[x + 1, y].SpriteRenderer.sprite == tiles[x, y + 2].SpriteRenderer.sprite)
                    {
                        listCanBeMatch.Add(tiles[x + 1, y].gameObject);
                        listCanBeMatch.Add(tiles[x, y + 1].gameObject);
                        listCanBeMatch.Add(tiles[x, y + 2].gameObject);
                    }

                    if (tiles[x + 1, y].SpriteRenderer.sprite == tiles[x + 1, y + 1].SpriteRenderer.sprite &&
                        tiles[x + 1, y].SpriteRenderer.sprite == tiles[x, y + 2].SpriteRenderer.sprite)
                    {
                        listCanBeMatch.Add(tiles[x + 1, y].gameObject);
                        listCanBeMatch.Add(tiles[x + 1, y + 1].gameObject);
                        listCanBeMatch.Add(tiles[x, y + 2].gameObject);
                    }

                    if (tiles[x, y].SpriteRenderer.sprite == tiles[x, y + 1].SpriteRenderer.sprite &&
                        tiles[x, y].SpriteRenderer.sprite == tiles[x + 1, y + 2].SpriteRenderer.sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y].gameObject);
                        listCanBeMatch.Add(tiles[x, y + 1].gameObject);
                        listCanBeMatch.Add(tiles[x + 1, y + 2].gameObject);
                    }


                }

                if (x < row - 2 && y < column - 1)
                {
                    if (tiles[x, y].SpriteRenderer.sprite == tiles[x + 1, y + 1].SpriteRenderer.sprite &&
                        tiles[x, y].SpriteRenderer.sprite == tiles[x + 2, y + 1].SpriteRenderer.sprite)
                    {
                        listCanBeMatch.Add(tiles[x + 1, y + 1].gameObject);
                        listCanBeMatch.Add(tiles[x, y].gameObject);
                        listCanBeMatch.Add(tiles[x + 2, y + 1].gameObject);
                    }

                    if (tiles[x, y + 1].SpriteRenderer.sprite == tiles[x + 1, y + 1].SpriteRenderer.sprite &&
                        tiles[x, y + 1].SpriteRenderer.sprite == tiles[x + 2, y].SpriteRenderer.sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y + 1].gameObject);
                        listCanBeMatch.Add(tiles[x + 1, y + 1].gameObject);
                        listCanBeMatch.Add(tiles[x + 2, y].gameObject);
                    }



                    if (tiles[x, y + 1].SpriteRenderer.sprite == tiles[x + 1, y].SpriteRenderer.sprite &&
                        tiles[x, y + 1].SpriteRenderer.sprite == tiles[x + 2, y].SpriteRenderer.sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y + 1].gameObject);
                        listCanBeMatch.Add(tiles[x + 1, y].gameObject);
                        listCanBeMatch.Add(tiles[x + 2, y].gameObject);
                    }

                    if (tiles[x, y].SpriteRenderer.sprite == tiles[x + 1, y].SpriteRenderer.sprite &&
                        tiles[x, y].SpriteRenderer.sprite == tiles[x + 2, y + 1].SpriteRenderer.sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y].gameObject);
                        listCanBeMatch.Add(tiles[x + 1, y].gameObject);
                        listCanBeMatch.Add(tiles[x + 2, y + 1].gameObject);
                    }

                    if (tiles[x, y].SpriteRenderer.sprite == tiles[x + 2, y].SpriteRenderer.sprite &&
                        tiles[x, y].SpriteRenderer.sprite == tiles[x + 1, y + 1].SpriteRenderer.sprite)
                    {
                        listCanBeMatch.Add(tiles[x, y].gameObject);
                        listCanBeMatch.Add(tiles[x + 2, y].gameObject);
                        listCanBeMatch.Add(tiles[x + 1, y + 1].gameObject);
                    }

                    if (tiles[x, y + 1].SpriteRenderer.sprite == tiles[x + 1, y].SpriteRenderer.sprite &&
                        tiles[x, y + 1].SpriteRenderer.sprite == tiles[x + 2, y + 1].SpriteRenderer.sprite)
                    {
                        //Debug.Log("Alooooooo");
                        listCanBeMatch.Add(tiles[x, y + 1].gameObject);
                        listCanBeMatch.Add(tiles[x + 1, y].gameObject);
                        listCanBeMatch.Add(tiles[x + 2, y + 1].gameObject);
                    }
                }
            }
        }
        return listCanBeMatch;
    }

    // Find Passive Match
    public void DetectMatchExist(List<GameObject> listGO)
    {
        if (listGO.Count > 0)
        {
            // Debug.LogFormat("We found the Match!");
            foreach (var go in listGO)
                go.GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, 1);
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
                listSwapContainer.Add(tiles[x, y].SpriteRenderer.sprite);
            }
        }

        for (int y = 0; y < column; y++)
        {
            for (int x = 0; x < row; x++)
            {
                Sprite go = listSwapContainer[Random.Range(0, listSwapContainer.Count)];
                tiles[x, y].SpriteRenderer.sprite = go;
                listSwapContainer.Remove(go);
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
    public void OnBoardFilled(List<Sprite> listSprite, Vector2 startPosition, Vector2 offsetPosition, Dictionary<string, Coroutine> coroutineMap)
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
            List<GameObject> listTotal = new List<GameObject>();
            List<GameObject> nullList = new List<GameObject>();

            int nullCount = 0;
            for (int y = column - 1; y >= 0; y--)
            {
                if (tiles[x, y].gameObject.activeSelf)
                {
                    if (nullCount == 0)
                        continue;
                    else
                        listTotal.Add(tiles[x, y].gameObject);
                }
                else
                {
                    nullList.Add(tiles[x, y].gameObject);
                    nullCount++;
                }
            }
            for (int i = 0; i < nullList.Count; i++)
            {
                nullList[i].GetComponent<SpriteRenderer>().sprite = listSprite[Random.Range(0, listSprite.Count)];
                nullList[i].transform.position = new Vector2(nullList[i].transform.position.x, startPosition.y + (i + 1) * offsetPosition.y);
            }
            listTotal.AddRange(nullList);
            
            for (int i = 0; i < listTotal.Count; i++)
            {
                tiles[x, i] = listTotal[listTotal.Count - 1 - i].GetComponent<TileController>();
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
}