using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
    public static BoardController instance;


    //TEST

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start() {}

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    for (int y = 0; y < GameController.instance.columnLength; y++)
        //    {
        //        for (int x = 0; x < GameController.instance.rowLength; x++)
        //        {
        //            StopCoroutine(GameController.instance.MoveTiles2(tiles[x, y], x, y));
        //            StartCoroutine(GameController.instance.MoveTiles2(tiles[x, y], x, y));
        //        }
        //    }
        //}
    }
    public void CreateBoard(float xOffset, float yOffset)
    {
        GameController.instance.tiles = new GameObject[GameController.instance.rowLength, GameController.instance.columnLength];

        for (int y = 0; y < GameController.instance.columnLength; y++)
        {
            for (int x = 0; x < GameController.instance.rowLength; x++)
            {
                GameObject newTile = Instantiate(GameController.instance.tile, new Vector3(GameController.instance.startPosition.x + (xOffset * x), GameController.instance.startPosition.y - (yOffset * y), 0), GameController.instance.tile.transform.rotation);

                GameController.instance.tiles[x, y] = newTile;
                newTile.name = "[ " + x + " , " + y + " ]";
                GameController.instance.tiles[x, y].transform.parent = transform;

                List<Sprite> possibleCharacters = new List<Sprite>();

                possibleCharacters.AddRange(GameController.instance.characters);

                if (x > 0)
                    if (GameController.instance.tiles[x - 1, y] != null)
                        possibleCharacters.Remove(GameController.instance.tiles[x - 1, y].GetComponent<SpriteRenderer>().sprite);
                if (y > 0)
                    if (GameController.instance.tiles[x, y - 1] != null)
                        possibleCharacters.Remove(GameController.instance.tiles[x, y - 1].GetComponent<SpriteRenderer>().sprite);
                Debug.LogFormat("possibleCharacter count is {0}", possibleCharacters.Count);
                Sprite newSprite = possibleCharacters[Random.Range(0, possibleCharacters.Count)];
                newTile.GetComponent<SpriteRenderer>().sprite = newSprite;
            }
        }
    }
}