using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WordGuardian.Data; // 引入数据模型
using System.Collections.Generic;
using System.IO;

public class WordBookManager : MonoBehaviour
{
    public TextMeshProUGUI definitionText;
    public TextMeshProUGUI storyEnglishText;
    public TextMeshProUGUI storyChineseText;
    public TextMeshProUGUI sentencesText;

    private Dictionary<string, WordData> wordDict = new Dictionary<string, WordData>();

    void Start()
    {
        LoadWords();
    }

    void LoadWords()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "TeachingWords.json");
        Debug.Log($"Loading JSON from: {path}");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<WordsData>(json);

            foreach (var word in data.words)
            {
                wordDict[word.word] = word;
            }
        }
        else
        {
            Debug.LogError("Words JSON file not found!");
        }
    }

    public void UpdateWordDetails(string wordKey)
    {
        Debug.Log($"🔄 Updating details for word: {wordKey}");

        if (wordDict.TryGetValue(wordKey, out WordData selectedWord))
        {
            definitionText.text = $"释义: {selectedWord.chinese}";
            storyEnglishText.text = $"英文背景故事: {selectedWord.story_en}";
            storyChineseText.text = $"中文背景故事: {selectedWord.story_cn}";
            sentencesText.text = $"句子:\n :{selectedWord.sentences}";
        }
        else
        {
            Debug.LogError($"Word '{wordKey}' not found in dictionary.");
        }
    }

    public void onButtonClicked(string wordKey)
    {
        Debug.Log($"Button clicked for word: {wordKey}");
        UpdateWordDetails(wordKey);
    }
}
