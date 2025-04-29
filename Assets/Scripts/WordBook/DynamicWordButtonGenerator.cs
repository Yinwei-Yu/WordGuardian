using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WordGuardian.Data; // 引入数据模型

public class DynamicWordButtonGenerator : MonoBehaviour
{
    public RectTransform content; // 容器
    public GameObject wordButtonPrefab;
    public WordBookManager wordBookManager;

    private List<WordData> words = new List<WordData>();

    void Start()
    {
        if (wordBookManager == null)
        {
            wordBookManager = FindObjectOfType<WordBookManager>();
            if (wordBookManager == null)
            {
                Debug.LogError("WordBookManager not found in the scene!");
                return;
            }
        }

        LoadWords();
        GenerateWordButtons();
    }

    void LoadWords()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "TeachingWords.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<WordsData>(json);

            if (data != null && data.words != null)
            {
                words.AddRange(data.words);
                Debug.Log($"Loaded {words.Count} words from JSON file.");
            }
            else
            {
                Debug.LogError("Failed to parse JSON data!");
            }
        }
        else
        {
            Debug.LogError("Words JSON file not found!");
        }
    }

    void GenerateWordButtons()
    {
        Debug.Log("Generating buttons...");
        foreach (var word in words)
        {
            // 实例化按钮
            GameObject buttonObject = Instantiate(wordButtonPrefab, content);

            // 设置按钮上的单词文本
            Transform textTransform = buttonObject.transform.Find("ButtonText"); // 推荐给 TMP 文本起名字
            if (textTransform != null)
            {
                TextMeshProUGUI buttonText = textTransform.GetComponent<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = word.word;
                }
            }
            // 获取 WordButton 组件并绑定事件
            WordButton wordButtonComponent = buttonObject.GetComponent<WordButton>();
            if (wordButtonComponent != null)
            {
                Debug.Log($"Binding click event for {word.word}");
                string currentword = word.word;
                wordButtonComponent.SetListener(() =>
                {
                    Debug.Log($"Clicked on word: {word.word}");
                    wordBookManager.UpdateWordDetails(currentword);
                });
            }
            else
            {
                Debug.LogError("WordButton component not found on the prefab!");
            }
        }

        // 只调用一次 ForceRebuild，提升性能
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
    }
}