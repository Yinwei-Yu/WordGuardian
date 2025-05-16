using UnityEngine;
using System.Collections.Generic;

// WordMatchingMonsterµÄWordManager
public class WordMatchingWordManager : MonoBehaviour
{
    public static WordMatchingWordManager Instance { get; private set; }
    public TextAsset wordsJsonFile;
    public WordList wordList;
    public int wordsToTestCount = 3;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
            LoadWordData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadWordData()
    {
        if (wordsJsonFile != null)
        {
            wordList = JsonUtility.FromJson<WordList>(wordsJsonFile.text);
            if (wordList == null || wordList.words == null)
            {
                Debug.LogError("Failed to parse word data!");
            }
        }
        else
        {
            Debug.LogError("Words JSON file not assigned!");
        }
    }

    public List<TestWordData> GetRandomWords(int count)
    {
        if (wordList == null || wordList.words.Count == 0)
        {
            Debug.LogError("No word data available!");
            return null;
        }

        count = Mathf.Min(count, wordList.words.Count);
        List<TestWordData> randomWords = new List<TestWordData>();
        List<TestWordData> tempList = new List<TestWordData>(wordList.words);

        for (int i = 0; i < count; i++)
        {
            if (tempList.Count == 0) break;

            int randomIndex = Random.Range(0, tempList.Count);
            randomWords.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }

        return randomWords;
    }
}
