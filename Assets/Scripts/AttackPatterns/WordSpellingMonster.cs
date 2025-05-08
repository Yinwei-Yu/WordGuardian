using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

// WordSpellingMonster½Å±¾
public class WordSpellingMonster : MonoBehaviour
{
    [Header("Radius")]
    public float detectionRadius = 3f;

    [Header("UI")]
    public GameObject testPanel;
    public TextMeshProUGUI questionText;
    public TMP_InputField answerInput;
    public Button submitButton;
    public TextMeshProUGUI feedbackText;
    public TMP_FontAsset stkaitiFont;

    private Transform player;
    private bool isTestActive = false;
    private GameObject testBubble;
    private PlayerController playerController;
    private List<TestWordData> currentTestWords;
    private int currentWordIndex = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        testPanel.SetActive(false);
        feedbackText.gameObject.SetActive(false);
        CreateTestBubble();
        submitButton.onClick.AddListener(SubmitAnswer);
    }

    void Update()
    {
        if (!isTestActive)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            testBubble.SetActive(distance <= detectionRadius);
        }

        if (isTestActive && Input.GetKeyDown(KeyCode.Return))
        {
            SubmitAnswer();
        }
    }

    void CreateTestBubble()
    {
        testBubble = new GameObject("TestBubble");
        testBubble.transform.SetParent(transform);
        testBubble.transform.localPosition = new Vector3(0, 1.5f, 0);

        var text = testBubble.AddComponent<TextMeshPro>();
        text.text = "Word Spelling Test";
        text.fontSize = 2;
        text.alignment = TextAlignmentOptions.Center;
        text.sortingOrder = 10;

        if (stkaitiFont != null)
        {
            text.font = stkaitiFont;
        }
        else
        {
            Debug.LogWarning("STKAITI SDF Not Found!");
        }

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
        answerInput.text = "";
        feedbackText.gameObject.SetActive(false);

        currentTestWords = WordSpellingWordManager.Instance.GetRandomWords(WordSpellingWordManager.Instance.wordsToTestCount);
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

        TestWordData currentWord = currentTestWords[currentWordIndex];
        questionText.text = $"Spell the word that means: '{currentWord.chinese}'";
    }

    void SubmitAnswer()
    {
        TestWordData currentWord = currentTestWords[currentWordIndex];
        string userAnswer = answerInput.text.Trim().ToLower();

        if (userAnswer == currentWord.word.ToLower())
        {
            feedbackText.text = "Correct!";
            feedbackText.color = Color.green;
            feedbackText.gameObject.SetActive(true);
            Invoke("HideFeedbackText", 3f);
        }
        else
        {
            feedbackText.text = $"Wrong! The correct spelling is: {currentWord.word}";
            feedbackText.color = Color.red;
            feedbackText.gameObject.SetActive(true);
            Invoke("HideFeedbackText", 3f);
        }

        feedbackText.gameObject.SetActive(true);
        currentWordIndex++;
        answerInput.text = "";
        ShowNextWord();
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

        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
}