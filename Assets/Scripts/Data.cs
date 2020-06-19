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
    public List<TileDetails>  tileProperties;
}

[Serializable]
public class TileDetails
{
    public string id;
    public int score;
    public string spriteName;
}