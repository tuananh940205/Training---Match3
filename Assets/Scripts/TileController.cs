using UnityEngine;
using DG.Tweening;

public class TileController : MonoBehaviour
{
    private Vector2 firstPosition;
    private Vector2 lastPosition;
    private Tween tween;
    public SpriteRenderer SpriteRenderer { get; private set; }
    public delegate void OnMouseUpEvent(Vector2 pos1, Vector2 pos2);
    public static OnMouseUpEvent onMouseUp;
    public delegate void OnMouseDownEvent(TileController tile);
    public static OnMouseDownEvent onMouseDown;
    // public TileName tileName { get; set; }
    
    // Test
    [SerializeField] string a;

    void Update()
    {
        // a = tileName.ToString();
    }
    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnMouseDown()
    {
        //Debug.LogFormat("OnMouseDown");
        //Debug.LogFormat("OnMouseDown != null");
        onMouseDown?.Invoke(this);

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

        //Debug.LogFormat("onMouseUp != null");
        onMouseUp?.Invoke(firstPosition, lastPosition);
    }
}