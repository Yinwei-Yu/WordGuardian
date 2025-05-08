using UnityEngine;
using System.Collections.Generic;

// ���ڵ���ƥ���������
[System.Serializable]
public class TestWordData
{
    public string word;
    public string chinese;
    public string story_en;
    public string story_cn;
    public List<string> sentences;
}

// �����б���
[System.Serializable]
public class WordList
{
    public List<TestWordData> words;
}
