using System;
using System.Collections.Generic;

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
    public int[] board;
}