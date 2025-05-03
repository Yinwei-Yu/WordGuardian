using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordMatchingMonster : MonoBehaviour
{
    [Header("Radius")]
    public float detectionRadius = 3f;

    [Header("UI")]
    public GameObject testPanel;
    public TextMeshProUGUI questionText;
    public Button[] optionButtons;
    public TextMeshProUGUI feedbackText;

    [Header("questions")]
    public string question;
    public string[] options;
    public int correctOptionIndex;

    private Transform player;
    private bool isTestActive = false;
    private GameObject testBubble;
    private PlayerController playerController;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        testPanel.SetActive(false);
        feedbackText.gameObject.SetActive(false);
        CreateTestBubble();
        SetupOptions();
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
        text.fontSize = 2;
        text.alignment = TextAlignmentOptions.Center;
        text.sortingOrder = 10;

        testBubble.SetActive(false);
    }

    void SetupOptions()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
            optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = options[i];
        }
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
        questionText.text = question;
        feedbackText.gameObject.SetActive(false);

        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    void OnOptionSelected(int optionIndex)
    {
        if (optionIndex == correctOptionIndex)
        {
            feedbackText.text = "Right1";
            feedbackText.color = Color.green;
            gameObject.SetActive(false);
        }
        else
        {
            feedbackText.text = "False,right answer is: " + options[correctOptionIndex];
            feedbackText.color = Color.red;
        }
        feedbackText.gameObject.SetActive(true);

        Invoke("EndTest", 2f);
    }

    void EndTest()
    {
        isTestActive = false;
        testPanel.SetActive(false);
        feedbackText.gameObject.SetActive(false); // ������ʾ��Ϣ

        if (playerController != null)
        {
            playerController.enabled = true; // ����������ҿ��ƽű�
        }
    }
}