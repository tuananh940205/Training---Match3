using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerData
{
    public string level;
    public int score;
    public int counter;
    public TileController[,] tilesArray;

    public PlayerData(string level, int score, int counter, TileController[,] tilesArray)
    {
        this.level = level;
        this.score = score;
        this.counter = counter;
        this.tilesArray = tilesArray;
    }

    void SetUpArray(TileController[,] tilesArray)
    {
        for (int y = 0; y < tilesArray.GetLength(1); y++)
        {
            for (int x = 0; x < tilesArray.GetLength(0); x++)
            {
                
            }
        }
    }
}
