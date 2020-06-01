using UnityEngine;
using DG.Tweening;

public class TileController : MonoBehaviour
{
    private Vector2 firstPosition;
    private Vector2 lastPosition;
    private Tween tween;
    public SpriteRenderer SpriteRenderer { get; private set; }

    void Awake()
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

        GameController.Instance.CheckAdjacent(firstPosition, lastPosition);
    }
}