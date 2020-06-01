using UnityEngine;
using DG.Tweening;

public class TileController : MonoBehaviour
{
    Vector2 firstPosition;
    Vector2 lastPosition;
    Tween tween;
    public SpriteRenderer SpriteRenderer { get; private set; }

    void Start() 
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnMouseDown()
    {
        GameController.Instance.firstTile = gameObject;

        firstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tween = transform.DOPath(
            new Vector3[] {transform.position,
                new Vector3(transform.position.x,
                transform.position.y + 0.1f,
                transform.position.z), transform.position },
            .8f);
    }

    void OnMouseUp()
    {
        tween.Complete();
        lastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(firstPosition, lastPosition) >= .5f)
            GameController.Instance.FindAdjacentAndMatchIfPossible(
                firstPosition,
                lastPosition,
                BoardController.Instance.tiles);
    }
}
