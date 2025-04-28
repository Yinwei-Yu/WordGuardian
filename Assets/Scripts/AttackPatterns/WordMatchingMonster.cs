using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordMatchingMonster : MonoBehaviour
{
    [Header("测试设置")]
    public float detectionRadius = 3f; // 检测玩家的半径

    [Header("UI引用")]
    public GameObject testPanel; // 测试面板
    public TextMeshProUGUI questionText; // 问题文本
    public Button[] optionButtons; // 选项按钮
    public TextMeshProUGUI feedbackText; // 提示信息文本（新增）

    [Header("测试内容")]
    public string question; // 问题文本
    public string[] options; // 选项
    public int correctOptionIndex; // 正确选项索引

    private Transform player; // 玩家引用
    private bool isTestActive = false; // 测试是否激活
    private GameObject testBubble; // 头顶测试题气泡
    private PlayerController playerController; // 玩家控制脚本引用

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>(); // 获取玩家控制脚本
        testPanel.SetActive(false);
        feedbackText.gameObject.SetActive(false); // 初始化时隐藏提示信息
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
            int index = i; // 闭包需要局部变量
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
        feedbackText.gameObject.SetActive(false); // 开始测试时隐藏提示信息

        if (playerController != null)
        {
            playerController.enabled = false; // 禁用玩家控制脚本
        }
    }

    void OnOptionSelected(int optionIndex)
    {
        if (optionIndex == correctOptionIndex)
        {
            feedbackText.text = "回答正确!";
            feedbackText.color = Color.green; // 设置提示文字颜色为绿色
        }
        else
        {
            feedbackText.text = "回答错误! 正确答案是: " + options[correctOptionIndex];
            feedbackText.color = Color.red; // 设置提示文字颜色为红色
        }
        feedbackText.gameObject.SetActive(true); // 显示提示信息

        Invoke("EndTest", 2f); // 延迟2秒后结束测试
    }

    void EndTest()
    {
        isTestActive = false;
        testPanel.SetActive(false);
        feedbackText.gameObject.SetActive(false); // 隐藏提示信息

        if (playerController != null)
        {
            playerController.enabled = true; // 重新启用玩家控制脚本
        }
    }
}