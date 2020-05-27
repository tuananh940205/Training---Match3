using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class TileController : MonoBehaviour
{
    static TileController instance = null;
    static TileController secondSelected = null;
    public static TileController instance2;
    Vector2 firstPosition;
    Vector2 lastPosition;
    //test
    public bool fistPositionIsChecked;
    public bool secondPostionIsChecked;
    Animator anim;
    Vector2 tempPosition;
    Tween tween;

    //private void OnEnable()
    //{
    //    GetComponent<Animator>().SetTrigger("Idle");
    //}

    private void Start()
    {
        anim = GetComponent<Animator>();
        instance2 = GetComponent<TileController>();
        
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    StartCoroutine(OnTileSwapping(gameObject, Vector2.right));
        //}
        if (instance != null)
            fistPositionIsChecked = true;
        else
            fistPositionIsChecked = false;

        if (secondSelected != null)
            secondPostionIsChecked = true;
        else
            secondPostionIsChecked = false;
    }

    // Bắt instance vào tile khi nhấn chuột xuống
    void OnMouseDown()
    {
        anim.SetBool("Animating", true);
        instance = GetComponent<TileController>();
        tempPosition = instance.transform.position;

        firstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        for (int y = 0; y < GameController.instance.columnLength; y++)
        {
            for (int x = 0; x < GameController.instance.rowLength; x++)
            {
                //if (BoardController.instance.tiles[x, y] == instance.gameObject)
                    //Debug.LogFormat("Fount the first [{0}, {1}]", x, y);
            }
        }
        tween = transform.DOPath(new Vector3[] {transform.position, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), transform.position }, .8f);
        
        //instance.transform.DOMove(new Vector2(transform.position.x, transform.position.y + 0.1f), 0.5f).OnComplete(DownOnTop);
    }

    //void DownOnTop()
    //{
    //    instance.transform.DOMove(new Vector2(transform.position.x, transform.position.y - 0.1f), 0.5f);
    //}

    // Bắt lastPosition, tính góc và xác định secondSelected
    void OnMouseUp()
    {

        tween.Complete();
        instance.anim.SetBool("Animating", false);
        instance.transform.position = tempPosition;
        lastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(firstPosition, lastPosition) >= .5f)
        {
            float swipeAngle = Mathf.Atan2(lastPosition.y - firstPosition.y, lastPosition.x - firstPosition.x) * 180 / Mathf.PI;            
            for (int y = 0; y < GameController.instance.columnLength; y++)
            {
                for (int x = 0; x < GameController.instance.rowLength; x++)
                {
                    if (GameController.instance.tiles[x, y] == instance.gameObject)
                    {
                        instance.transform.position = new Vector2(transform.position.x, GameController.instance.startPosition.y - GameController.instance.offset.y * y);
                        StartCoroutine(WaitForEndOfFrame());
                        //Debug.LogFormat("tiles selected tiles[{0}, {1}]", x, y);

                        if (GetTheAdjacentTile(swipeAngle, x, y))
                        {
                            //Debug.LogFormat("Execute if true; directionMoving = {0}, 1st = {1}, 2nd = {2}", directionMoving, instance.gameObject.name, secondSelected.gameObject.name);
                            StartCoroutine(OnTileSwapping(instance.gameObject, secondSelected.gameObject));
                        }
                        return;
                    }
                }
            }
        }
    }
    

    IEnumerator WaitForEndOfFrame()
    {
        yield return new WaitForSeconds(1);
    }

    bool GetTheAdjacentTile(float radiant, int xIndex, int yIndex)
    {
        bool findTheAdjacent;
        if (radiant > -45 && radiant < 45)
        {
            //Debug.LogFormat("Right");
            if (xIndex < GameController.instance.rowLength - 1)
            {
                if (GameController.instance.tiles[xIndex + 1, yIndex] != null)
                {
                    secondSelected = GameController.instance.tiles[xIndex + 1, yIndex].GetComponent<TileController>();

                    Debug.LogFormat("tiles [{0}, {1}] is second Selected {2}", xIndex + 1, yIndex, secondSelected.gameObject.name);
                }
            }
        }
        else if (radiant > 45 && radiant < 135)
        {
            //Debug.LogFormat("Up");
            if (yIndex > 0)
            {
                if (GameController.instance.tiles[xIndex, yIndex - 1] != null)
                {
                    secondSelected = GameController.instance.tiles[xIndex, yIndex - 1].GetComponent<TileController>();
                    //Debug.LogFormat("tiles [{0}, {1}] is second Selected {2}", xIndex, yIndex - 1, secondSelected.gameObject.name);
                }
            }
        }
        else if (radiant > 135 || radiant < -135)
        {
            //Debug.LogFormat("Left");
            if (xIndex > 0)
            {
                if (GameController.instance.tiles[xIndex - 1, yIndex] != null)
                {
                    secondSelected = GameController.instance.tiles[xIndex - 1, yIndex].GetComponent<TileController>();
                    //Debug.LogFormat("tiles [{0}, {1}] is second Selected {2}", xIndex - 1, yIndex, secondSelected.gameObject.name);
                }
            }
        }
        else if (radiant > -135 && radiant < -45)
        {
            //Debug.LogFormat("Down");
            if (yIndex < GameController.instance.columnLength - 1)
            {
                if (GameController.instance.tiles[xIndex, yIndex + 1] != null)
                {
                    secondSelected = GameController.instance.tiles[xIndex, yIndex + 1].GetComponent<TileController>();
                    //Debug.LogFormat("tiles [{0}, {1}] is second Selected {2}", xIndex, yIndex + 1, secondSelected.gameObject.name);
                }
            }
        }
        if (secondSelected != null)
            findTheAdjacent = true;
        else
            findTheAdjacent = false;

        return findTheAdjacent;
    }

    IEnumerator OnTileSwapping(GameObject go1, GameObject go2)
    {
        Debug.LogFormat("Swapping");
        int temp1 = -1, temp2 = -1, temp3 = -1, temp4 = -1;
        for (int y = 0; y < GameController.instance.columnLength; y++)
        {
            
            for (int x = 0; x < GameController.instance.rowLength; x++)
            {
                if (GameController.instance.tiles[x, y] == go1)
                {
                    temp1 = x;
                    temp2 = y;
                }
                if (GameController.instance.tiles[x, y] == go2)
                {
                    temp3 = x;
                    temp4 = y;
                }
                if (temp1 > -1 && temp2 > -1 && temp3 > -1 && temp4 > -1)
                {
                    GameController.instance.tiles[temp1, temp2] = go2;
                    GameController.instance.tiles[temp1, temp2].name = "Swap [ " + temp1 + ", " + temp2 + " ]";
                    GameController.instance.tiles[temp3, temp4] = go1;
                    GameController.instance.tiles[temp3, temp4].name = "Swap [ " + temp3 + ", " + temp4 + " ]";
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
        if (FindingTheMatches(go1, go2).Contains(go1) || FindingTheMatches(go1, go2).Contains(go2))
        {
            //BoardController.instance.StartCoroutine(AllTilesFadeOut(FindingTheMatches(go1, go2)));
            StartCoroutine(GameController.instance.AllTilesFadeOut(FindingTheMatches(go1, go2)));
            //Debug.LogFormat("Matched");
        }
        else
        {
            StartCoroutine(TileSwapBackOnMatchFailure(go1, go2));
        }
    }

    IEnumerator TileSwapBackOnMatchFailure(GameObject go1, GameObject go2)
    {
        Debug.LogFormat("Swap failed");
        int temp1 = -1, temp2 = -1, temp3 = -1, temp4 = -1;
        for (int y = 0; y < GameController.instance.columnLength; y++)
        {
            for (int x = 0; x < GameController.instance.rowLength; x++)
            {
                if (temp1 > -1 && temp2 > -1 && temp3 > -1 && temp4 > -1)
                {
                    GameController.instance.tiles[temp1, temp2] = go2;
                    GameController.instance.tiles[temp1, temp2].name = "SwapBack [ " + temp1 + ", " + temp2 + " ]";
                    GameController.instance.tiles[temp3, temp4] = go1;
                    GameController.instance.tiles[temp3, temp4].name = "SwapBack [ " + temp3 + ", " + temp4 + " ]";

                    break;
                }
                if (GameController.instance.tiles[x, y] == go1)
                {
                    temp1 = x;
                    temp2 = y;
                }
                if (GameController.instance.tiles[x, y] == go2)
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
        instance = null;
        secondSelected = null;
    }

    List<GameObject> FindingTheMatches(GameObject go1, GameObject go2)
    {
        List<GameObject> clearMatchList = new List<GameObject>();
        //List<GameObject> checkingMatchListVertical = new List<GameObject>();
        //List<GameObject> checkingMatchListHorizontal = new List<GameObject>();

        int x1 = -1, y1 = -1, x2 = -1, y2 = -1;
        for(int y = 0; y < GameController.instance.columnLength; y++)
        {
            for (int x = 0; x < GameController.instance.rowLength; x++)
            {
                
                if (GameController.instance.tiles[x, y] == go1)
                {
                    x1 = x;
                    y1 = y;
                }
                if (GameController.instance.tiles[x, y] == go2)
                {
                    x2 = x;
                    y2 = y;
                }
            }
        }

        List<GameObject> list1 = FindMatchFromSwappingTiles(go1, x1, y1);
        List<GameObject> list2 = FindMatchFromSwappingTiles(go2, x2, y2);

        clearMatchList.AddRange(list1);
        clearMatchList.AddRange(list2);
        
        return clearMatchList;
    }

    public List<GameObject> FindMatchFromSwappingTiles(GameObject go, int IndexX, int IndexY)
    {
        List<GameObject> checkingMatchListVertical = new List<GameObject>();
        List<GameObject> checkingMatchListHorizontal = new List<GameObject>();
        List<GameObject> totalList = new List<GameObject>();
        // Scanning up
        for (int i = 1; i <= IndexY; i++)
        {
            if (GameController.instance.tiles[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == GameController.instance.tiles[IndexX, IndexY - i].GetComponent<SpriteRenderer>().sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", BoardController.instance.tiles[IndexX, IndexY - i].name, IndexX, IndexY - 1);
                checkingMatchListVertical.Add(GameController.instance.tiles[IndexX, IndexY - i]);
            }
            else
                break;
        }
        //scanning down
        for (int i = 1; i < GameController.instance.columnLength - IndexY; i++)
        {
            if (GameController.instance.tiles[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == GameController.instance.tiles[IndexX, IndexY + i].GetComponent<SpriteRenderer>().sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", BoardController.instance.tiles[IndexX, IndexY + i].name, IndexX, IndexY + 1);
                checkingMatchListVertical.Add(GameController.instance.tiles[IndexX, IndexY + i]);
            }
            else
                break;
        }
        if (checkingMatchListVertical.Count >= 2)
            totalList.AddRange(checkingMatchListVertical);
        // SCANNING LEFT
        for (int i = 1; i <= IndexX; i++)
        {
            if (GameController.instance.tiles[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == GameController.instance.tiles[IndexX - i, IndexY].GetComponent<SpriteRenderer>().sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", BoardController.instance.tiles[IndexX - i, IndexY].name, IndexX - i, IndexY);
                checkingMatchListHorizontal.Add(GameController.instance.tiles[IndexX - i, IndexY]);
            }
            else
                break;
        }
        // SCANNING RIGHT
        for (int i = 1; i < GameController.instance.rowLength - IndexX; i++)
        {
            if (GameController.instance.tiles[IndexX, IndexY].GetComponent<SpriteRenderer>().sprite == GameController.instance.tiles[IndexX + i, IndexY].GetComponent<SpriteRenderer>().sprite)
            {
                //Debug.LogFormat("gameObject {0} tiles [{1}, {2}]", BoardController.instance.tiles[IndexX + i, IndexY].name, IndexX + i, IndexY);
                checkingMatchListHorizontal.Add(GameController.instance.tiles[IndexX + i, IndexY]);
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

    // TEMPORARiLY NOT USE
    //IEnumerator TileFadeOut (GameObject go)
    //{
    //    for (int i = 10; i >= 0; i--)
    //    {
    //        //Debug.LogFormat("count = {0}", 1 - i / 5);
    //        go.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i * .1f);
    //        yield return new WaitForSeconds(.05f);
    //    }
    //    go.SetActive(false);
    //    go.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

    //    if (BoardController.instance.Number > 1)
    //        BoardController.instance.Number--;
    //    else
    //        BoardController.instance.GetNewUpperTiles();
    //}

    //public IEnumerator AllTilesFadeOut(List<GameObject> tiles)
    //{
    //    for (int i = 10; i > 0; i--)
    //    {
    //        foreach (var tile in tiles)
    //        {
    //            tile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i * .1f);
    //        }
    //        yield return new WaitForSeconds(.05f);
    //    }
    //    foreach (var tile in tiles)
    //        tile.SetActive(false);
    //    BoardController.instance.GetNewUpperTiles();
    //}
}