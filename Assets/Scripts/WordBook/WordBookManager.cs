using UnityEngine;
using TMPro; // 新增命名空间
using UnityEngine.UI;
using System.Linq;
using System.Collections;
[System.Serializable]
public class WordData
{
    public string word;
    public string chinese;
    public string story_en;
    public string story_cn;
    public string[] sentences;
}

[System.Serializable]
public class WordBook
{
    public WordData[] words;
}

public class WordBookManager : MonoBehaviour
{
    [Header("UI Reference")]
    public Transform contentParent;      // ScrollView的Content对象
    public GameObject buttonPrefab;      // TMP按钮预制体

    [Header("右侧显示区域")]
    public TMP_Text wordText;          // 全部改为TMP_Text
    public TMP_Text chineseText;
    public TMP_Text storyEnText;
    public TMP_Text storyCnText;
    public TMP_Text sentencesText;

    public string filePath;
    private WordData[] allWords;

    void Start()
    {
        LoadData();
        CreateButtons();
        ShowWord(0);
    }

    void LoadData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(filePath);
        WordBook data = JsonUtility.FromJson<WordBook>(jsonFile.text);
        allWords = data.words;
    }

    void CreateButtons()
    {
        foreach (WordData word in allWords)
        {
            GameObject newButton = Instantiate(buttonPrefab, contentParent);

            // 强制应用预制体布局设置
            LayoutElement layout = newButton.GetComponent<LayoutElement>();
            layout.minHeight = 60;
            layout.preferredWidth = -1; // 自动适应父容器

            // 获取TMP组件
            TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = word.word; // 直接使用word字段

            Button btn = newButton.GetComponent<Button>();
            btn.onClick.AddListener(() => ShowWord(System.Array.IndexOf(allWords, word)));
        }
        // 布局强制刷新
        StartCoroutine(RefreshLayout());
    }

    public void ShowWord(int index)
    {
        WordData current = allWords[index];

        wordText.text = $"<b>{current.word}</b>"; // 使用TMP富文本
        chineseText.text = $"{current.chinese}";
        storyEnText.text = current.story_en;
        storyCnText.text = current.story_cn;
        sentencesText.text = string.Join("\n",
            current.sentences.Select(s => $"• <i>{s}</i>"));
    }

    IEnumerator RefreshLayout()
    {
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentParent.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
    }
}