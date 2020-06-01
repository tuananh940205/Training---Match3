using UnityEngine;

public class TestFive : MonoBehaviour
{
    public GameObject[,] tileCovers;
    public int maxRow;
    public int maxColumn;
    void Start()
    {
        CreateTheBoard();
    }

    void CreateTheBoard()
    {
        tileCovers = new GameObject[maxRow, maxColumn];

        for (int y = 0; y < maxColumn; y++)
        {
            for (int x = 0; x < maxRow; x++)
            {
                //tileCovers[x, y] = new GameObject("Cover [ " + x)
            }
        }
        
    }
}
