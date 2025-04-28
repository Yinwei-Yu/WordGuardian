using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordSpellingMonster : MonoBehaviour
{
    [Header("��������")]
    public float detectionRadius = 3f;

    [Header("UI����")]
    public GameObject testPanel;
    public TextMeshProUGUI questionText;
    public TMP_InputField answerInput;
    public Button submitButton;
    public TextMeshProUGUI feedbackText; // ��ʾ��Ϣ�ı���������

    [Header("��������")]
    public string question;
    public string answer;

    private Transform player;
    private bool isTestActive = false;
    private GameObject testBubble;
    private PlayerController playerController; // ��ҿ��ƽű�����

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>(); // ��ȡ��ҿ��ƽű�
        testPanel.SetActive(false);
        feedbackText.gameObject.SetActive(false); // ��ʼ��ʱ������ʾ��Ϣ
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

        TMP_FontAsset stkaitiFont = Resources.Load<TMP_FontAsset>("Fonts & Materials/STKAITI SDF");
        if (stkaitiFont != null)
        {
            text.font = stkaitiFont;
        }
        else
        {
            Debug.LogWarning("STKAITI SDF����δ�ҵ�����ȷ�������ѵ��뵽Resources/Fonts & Materials�ļ�����");
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
        questionText.text = question;
        answerInput.text = "";
        feedbackText.gameObject.SetActive(false); // ��ʼ����ʱ������ʾ��Ϣ

        if (playerController != null)
        {
            playerController.enabled = false; // ������ҿ��ƽű�
        }
    }

    void SubmitAnswer()
    {
        string userAnswer = answerInput.text.Trim().ToLower();
        if (userAnswer == answer.ToLower())
        {
            feedbackText.text = "�ش���ȷ!";
            feedbackText.color = Color.green; // ������ʾ������ɫΪ��ɫ
        }
        else
        {
            feedbackText.text = "�ش����! ��ȷ����: " + answer;
            feedbackText.color = Color.red; // ������ʾ������ɫΪ��ɫ
        }
        feedbackText.gameObject.SetActive(true); // ��ʾ��ʾ��Ϣ

        Invoke("EndTest", 2f); // �ӳ�2����������
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