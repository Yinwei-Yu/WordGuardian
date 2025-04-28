using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordSpellingMonster : MonoBehaviour
{
    [Header("测试设置")]
    public float detectionRadius = 3f;

    [Header("UI引用")]
    public GameObject testPanel;
    public TextMeshProUGUI questionText;
    public TMP_InputField answerInput;
    public Button submitButton;
    public TextMeshProUGUI feedbackText; // 提示信息文本（新增）

    [Header("测试内容")]
    public string question;
    public string answer;

    private Transform player;
    private bool isTestActive = false;
    private GameObject testBubble;
    private PlayerController playerController; // 玩家控制脚本引用

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>(); // 获取玩家控制脚本
        testPanel.SetActive(false);
        feedbackText.gameObject.SetActive(false); // 初始化时隐藏提示信息
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
            Debug.LogWarning("STKAITI SDF字体未找到，请确保字体已导入到Resources/Fonts & Materials文件夹中");
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
        feedbackText.gameObject.SetActive(false); // 开始测试时隐藏提示信息

        if (playerController != null)
        {
            playerController.enabled = false; // 禁用玩家控制脚本
        }
    }

    void SubmitAnswer()
    {
        string userAnswer = answerInput.text.Trim().ToLower();
        if (userAnswer == answer.ToLower())
        {
            feedbackText.text = "回答正确!";
            feedbackText.color = Color.green; // 设置提示文字颜色为绿色
        }
        else
        {
            feedbackText.text = "回答错误! 正确答案是: " + answer;
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