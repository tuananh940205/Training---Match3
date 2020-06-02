using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private GameController gcVar;
    public TileController[,] tiles;
    private List<Sprite> listSwapContainer = new List<Sprite>();
    private int row, column;

    void Start()
    {
        // offset = new Vector2 (0.8f, 0.8f);
        DetectMatchExist(FindTheMatchExist());
    }


    public void CreateBoard(int row, int column)
    {
        this.row = row;
        this.column = column;
        CreateBoard();
    }
    private IEnumerator MoveTiles2(GameObject go, int indexX, int indexY, Vector2 startPosition, Vector2 offset, TileController[,] tiles)
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

        List<GameObject> listmatch = gcVar.FindMatchesPassively(go, indexX, indexY, tiles);
        gcVar.ClearAllPassiveMatches(listmatch, tiles);

        DetectMatchExist(FindTheMatchExist());
    }

    

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
            
            for (int y = 0; y < columnLength; y++)
            {
                for (int x = 0; x < rowLength; x++)
                {
                    listSwapContainer.Add(tiles[x, y].SpriteRenderer.sprite);
                }
            }

            for (int y = 0; y < columnLength; y++)
            {
                for (int x = 0; x < rowLength; x++)
                {
                    Sprite go = listSwapContainer[Random.Range(0, listSwapContainer.Count)];
                    tiles[x, y].SpriteRenderer.sprite = go;
                    listSwapContainer.Remove(go);
                }
            }

            for (int y = 0; y < columnLength; y++)
            {
                for (int x = 0; x < rowLength; x++)
                {
                    //List<GameObject> listMatch = gcVar.FindMatchesPassively(tiles[x, y].gameObject, x, y, tiles);
                    //gcVar.ClearAllPassiveMatches(listMatch, tiles);
                    FindAndClearMatchPassively(x, y);
                }
            }
        }
    }

    public void FindAndClearMatchPassively(int xIndex, int yIndex)
    {
        List<GameObject> listMatch = gcVar.FindMatchesPassively(tiles[xIndex, yIndex].gameObject, xIndex, yIndex, tiles);
        if (listMatch.Count >= 3)
            gcVar.ClearAllPassiveMatches(listMatch, tiles);
        
    }

    public void GetNewUpperTiles2()
    {
        for (int y = 0; y < columnLength; y++)
        {
            for (int x = 0; x < rowLength; x++)
            {
                if (tiles[x, y].SpriteRenderer.color != new Color (1, 1, 1, 1))
                    tiles[x, y].SpriteRenderer.color = new Color (1, 1, 1, 1);
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
                nullList[i].GetComponent<SpriteRenderer>().sprite = characters[Random.Range(0, characters.Count)];
                nullList[i].transform.position = new Vector2(nullList[i].transform.position.x, startPosition.y + (i + 1) * offset.y);
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
                coroutineMap[tiles[x, i].name] = StartCoroutine(MoveTiles2(gcVar.tiles[x, i].gameObject, x, i, gcVar.startPosition, gcVar. offset));
            }
        }
    }
}