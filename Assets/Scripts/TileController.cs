using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class TileController : MonoBehaviour
{
    public GameObject firstTile = null;
    public GameObject secondTile = null;
    Vector2 firstPosition;
    Vector2 lastPosition;
    //test
    public bool fistPositionIsChecked;
    public bool secondPostionIsChecked;
    Vector2 tempPosition;
    Tween tween;


    private void Update()
    {
        if (firstTile != null)
            fistPositionIsChecked = true;
        else
            fistPositionIsChecked = false;

        if (secondTile != null)
            secondPostionIsChecked = true;
        else
            secondPostionIsChecked = false;
    }

    void OnMouseDown()
    {
        firstTile = this.gameObject;
        tempPosition = firstTile.transform.position;

        firstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tween = transform.DOPath(new Vector3[] {transform.position, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), transform.position }, .8f);
        
    }

    void OnMouseUp()
    {
        tween.Complete();
        firstTile.transform.position = tempPosition;
        lastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(firstPosition, lastPosition) >= .5f)
        {
            float swipeAngle = Mathf.Atan2(lastPosition.y - firstPosition.y, lastPosition.x - firstPosition.x) * 180 / Mathf.PI;            
            for (int y = 0; y < BoardController.instance.columnLength; y++)
            {
                for (int x = 0; x < BoardController.instance.rowLength; x++)
                {
                    if (BoardController.instance.tiles[x, y] == firstTile)
                    {
                        firstTile.transform.position = new Vector2(transform.position.x, BoardController.instance.startPosition.y - BoardController.instance.offset.y * y);

                        if (GameController.instance.GetTheAdjacentTile(swipeAngle, x, y, secondTile))
                            StartCoroutine(GameController.instance.OnTileSwapping(firstTile, secondTile, BoardController.instance.tiles));

                        return;
                    }
                }
            }
        }
    }

    void GetTheSecond()
    {
        
    }
}