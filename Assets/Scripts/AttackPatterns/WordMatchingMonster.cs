using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

// WordMatchingMonster脚本
public class WordMatchingMonster : MonoBehaviour
{
    [Header("Radius")]
    public float detectionRadius = 3f;

    [Header("UI")]
    public GameObject testPanel;
    public TextMeshProUGUI questionText;
    public Button[] optionButtons;
    public TextMeshProUGUI feedbackText;

    [Header("Settings")]
    public int numberOfOptions = 4;
    public int bubbleSize;

    private Transform player;
    private bool isTestActive = false;
    private GameObject testBubble;
    private PlayerController playerController;
    private List<TestWordData> currentTestWords;
    private int currentWordIndex = 0;
    private int correctOptionIndex;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        testPanel.SetActive(false);
        feedbackText.gameObject.SetActive(false);
        CreateTestBubble();
    }

    void Update()
    {
        if (!isTestActive)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            testBubble.SetActive(distance <= detectionRadius);
        }
    }

    void CreateTestBubble()
    {
        testBubble = new GameObject("TestBubble");
        testBubble.transform.SetParent(transform);
        testBubble.transform.localPosition = new Vector3(0, 1.5f, 0);

        var text = testBubble.AddComponent<TextMeshPro>();
        text.text = "Matching Test";
        text.fontSize = bubbleSize;
        text.alignment = TextAlignmentOptions.Center;
        text.sortingOrder = 10;

        testBubble.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTestActive)
        {
            StartTest();
        }
    }

    void StartTest()
    {
        isTestActive = true;
        testBubble.SetActive(false);
        testPanel.SetActive(true);
        feedbackText.gameObject.SetActive(false);

        currentTestWords = WordMatchingWordManager.Instance.GetRandomWords(WordMatchingWordManager.Instance.wordsToTestCount);
        currentWordIndex = 0;

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        ShowNextWord();
    }

    void ShowNextWord()
    {
        if (currentWordIndex >= currentTestWords.Count)
        {
            EndTest();
            return;
        }

        // 重新激活所有选项按钮
        foreach (Button button in optionButtons)
        {
            button.gameObject.SetActive(true);
        }

        TestWordData currentWord = currentTestWords[currentWordIndex];
        questionText.text = $"What is the Chinese meaning of '{currentWord.word}'?";

        SetupOptions(currentWord);
    }

    void SetupOptions(TestWordData correctWord)
    {
        List<TestWordData> allWords = WordMatchingWordManager.Instance.wordList.words;
        List<TestWordData> tempWordList = new List<TestWordData>(allWords);
        tempWordList.Remove(correctWord);

        List<string> wrongOptions = new List<string>();
        for (int i = 0; i < numberOfOptions - 1; i++)
        {
            if (tempWordList.Count == 0) break;

            int randomIndex = Random.Range(0, tempWordList.Count);
            wrongOptions.Add(tempWordList[randomIndex].chinese);
            tempWordList.RemoveAt(randomIndex);
        }

        correctOptionIndex = Random.Range(0, numberOfOptions);

        int wrongOptionIndex = 0;
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i == correctOptionIndex)
            {
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = correctWord.chinese;
            }
            else
            {
                if (wrongOptionIndex < wrongOptions.Count)
                {
                    optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = wrongOptions[wrongOptionIndex];
                    wrongOptionIndex++;
                }
                else
                {
                    optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Unknown";
                }
            }

            optionButtons[i].onClick.RemoveAllListeners();
            int index = i;
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }
    }

    void OnOptionSelected(int optionIndex)
    {
        TestWordData currentWord = currentTestWords[currentWordIndex];

        if (optionIndex == correctOptionIndex)
        {
            feedbackText.text = "Correct!";
            feedbackText.color = Color.green;
            feedbackText.gameObject.SetActive(true);
            Invoke("HideFeedbackText", 3f);
            currentWordIndex++;
            Invoke("ShowNextWord", 1f);
        }
        else
        {
            feedbackText.text = $"Wrong! The correct answer is: {currentWord.chinese}";
            feedbackText.color = Color.red;
            feedbackText.gameObject.SetActive(true);
            Invoke("HideFeedbackText", 3f);
            // 移除错误选项按钮
            optionButtons[optionIndex].gameObject.SetActive(false);
        }
    }

    void HideFeedbackText()
    {
        feedbackText.gameObject.SetActive(false);
    }

    void EndTest()
    {
        isTestActive = false;
        testPanel.SetActive(false);
        feedbackText.gameObject.SetActive(false);
        gameObject.SetActive(false);
        Destroy(gameObject);
        EnemyManager.Instance.OnEnemyDefeated();
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
}