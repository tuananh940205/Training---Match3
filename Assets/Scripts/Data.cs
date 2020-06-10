using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Data
{
    public PlayerData items;
}
[Serializable]
public class PlayerData
{
    public LevelData level1;
    public LevelData level2;
    public LevelData level3;
    public LevelData level4;
    public LevelData level5;
    public LevelData level6;
    public LevelData level7;
    public LevelData level8;
    public LevelData level9;
    public LevelData level10;
}

[Serializable]
public class LevelData
{
    public int scoreTarget;
    public int counter;
    public string board;
}