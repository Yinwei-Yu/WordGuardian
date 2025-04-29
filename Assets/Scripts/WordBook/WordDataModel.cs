using System.Collections.Generic;
using UnityEngine;
///存放数据的类
namespace WordGuardian.Data
{
    [System.Serializable]
    public class WordsData
    {
        public List<WordData> words;
    }

    [System.Serializable]
    public class WordData
    {
        public string word;
        public string chinese;
        public string story_en;
        public string story_cn;
        public string sentences;
    }
}