using UnityEngine;
using System.Collections.Generic;

// 用于单词匹配的数据类
[System.Serializable]
public class TestWordData
{
    public string word;
    public string chinese;
    public string story_en;
    public string story_cn;
    public List<string> sentences;
}

// 单词列表类
[System.Serializable]
public class WordList
{
    public List<TestWordData> words;
}
