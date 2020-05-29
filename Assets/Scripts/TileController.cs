using UnityEngine;
using DG.Tweening;

public class TileController : MonoBehaviour
{
    Vector2 firstPosition;
    Vector2 lastPosition;
    //test
    // Vector2 tempPosition;
    Tween tween;
    public SpriteRenderer spriteRenderer { get; private set;}

    void Start() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnMouseDown()
    {
        GameController.instance.firstTile = this.gameObject;
        // tempPosition = GameController.instance.firstTile.transform.position;

        firstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tween = transform.DOPath(new Vector3[] {transform.position, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), transform.position }, .8f);
    }

    void OnMouseUp()
    {
        tween.Complete();
        // GameController.instance.firstTile.transform.position = tempPosition;
        lastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(firstPosition, lastPosition) >= .5f)
            GameController.instance.FindAdjacentAndMatchIfPossible(
                firstPosition,
                lastPosition,
                BoardController.instance.tiles,
                BoardController.instance.offset,
                BoardController.instance.startPosition);
    }
}