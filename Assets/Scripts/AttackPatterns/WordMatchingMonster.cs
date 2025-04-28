using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordMatchingMonster : MonoBehaviour
{
    [Header("��������")]
    public float detectionRadius = 3f; // �����ҵİ뾶

    [Header("UI����")]
    public GameObject testPanel; // �������
    public TextMeshProUGUI questionText; // �����ı�
    public Button[] optionButtons; // ѡ�ť
    public TextMeshProUGUI feedbackText; // ��ʾ��Ϣ�ı���������

    [Header("��������")]
    public string question; // �����ı�
    public string[] options; // ѡ��
    public int correctOptionIndex; // ��ȷѡ������

    private Transform player; // �������
    private bool isTestActive = false; // �����Ƿ񼤻�
    private GameObject testBubble; // ͷ������������
    private PlayerController playerController; // ��ҿ��ƽű�����

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>(); // ��ȡ��ҿ��ƽű�
        testPanel.SetActive(false);
        feedbackText.gameObject.SetActive(false); // ��ʼ��ʱ������ʾ��Ϣ
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
            int index = i; // �հ���Ҫ�ֲ�����
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
        feedbackText.gameObject.SetActive(false); // ��ʼ����ʱ������ʾ��Ϣ

        if (playerController != null)
        {
            playerController.enabled = false; // ������ҿ��ƽű�
        }
    }

    void OnOptionSelected(int optionIndex)
    {
        if (optionIndex == correctOptionIndex)
        {
            feedbackText.text = "�ش���ȷ!";
            feedbackText.color = Color.green; // ������ʾ������ɫΪ��ɫ
        }
        else
        {
            feedbackText.text = "�ش����! ��ȷ����: " + options[correctOptionIndex];
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