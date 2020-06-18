using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Data
{
    public PlayerData items;
}



[Serializable]
public class PlayerData
{
    public List<LevelData> levels = new List<LevelData>();
}

[Serializable]
public class LevelData
{
    public int level;
    public int scoreTarget;
    public int counter;
    public BoardData[] boards;
    // public TileData[] tileData;
}

[Serializable]
public class BoardData
{
    public int x;
    public int y;
    public string tileId;
    // public Sprite spriteTile;

    public BoardData()
    {
        // Debug.LogFormat("Run constructor of BoardData");
        // Can't assign here cause of null value
        // Debug.LogFormat("tileId = {0}", tileId);
        // spriteTile = Resources.Load<Sprite>(tileId);
        // Debug.LogFormat(spriteTile.ToString());
    }
}

[Serializable]
public class TileData
{
    public string id;
    public string spriteName;
    public string score;

}

[Serializable]
public class TilePointData
{
    public TilePoints items;
}
[Serializable]
public class TilePoints
{
    public int milkPoint;
    public int applePoint;
    public int orangePoint;
    public int breadPoint;
    public int coconutPoint;
    public int flowerPoint;
}