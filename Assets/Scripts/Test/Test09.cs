using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataOne
{
    public string itemss;
    public DataTwo items;
}
[Serializable]
public class DataTwo
{
    public string testDataTwo;
    public DataThree levels;
}
[Serializable]
public class DataThree
{
    public string testDataThree;
    public int score;
    public int counter;
}
public class Test09 : MonoBehaviour
{
    private int score;
    private int counter;
    void Start()
    {
        string jsonData = File.ReadAllText(Application.dataPath + "/Scripts/Test/Test01.json");
        Debug.LogFormat("All contents: {0}", jsonData);

        DataOne dataOne = JsonUtility.FromJson<DataOne>(jsonData);
        // "items": "abc"
        Debug.LogFormat(dataOne.itemss);
        // "items": { "testDataTwo": "string testDataTwo" }
        Debug.LogFormat(dataOne.items.testDataTwo);
        // "levels": { "testDataThree": "string testDataThree" }
        Debug.LogFormat(dataOne.items.levels.testDataThree);
        Debug.LogFormat(dataOne.items.levels.score.ToString());
        Debug.LogFormat(dataOne.items.levels.counter.ToString());
        Debug.Log("------------------");

        score = dataOne.items.levels.score;
        counter = dataOne.items.levels.counter;

        Debug.LogFormat("score = {0}, counter = {1}", score, counter);
    }
}
