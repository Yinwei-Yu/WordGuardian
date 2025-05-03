using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PictureToWordMonster : MonoBehaviour
{
    [Header("Radius and fonts")]
    public float detectionRadius = 3f;
    public TMP_FontAsset stkaitiFont;

    [Header("UI")]
    public GameObject testPanel;
    public Image questionImage;
    public TMP_InputField answerInput;
    public Button submitButton;
    public TextMeshProUGUI feedbackText;

    [Header("Image and answer")]
    public Sprite picture;
    public string answer;

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
        text.text = "Picture to Word Test";
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
        Debug.Log("Trigger!");
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
        questionImage.sprite = picture;
        answerInput.text = "";
        feedbackText.gameObject.SetActive(false); //

        if (playerController != null)
        {
            playerController.enabled = false; //
        }
    }

    void SubmitAnswer()
    {
        string userAnswer = answerInput.text.Trim().ToLower();
        if (userAnswer == answer.ToLower())
        {
            feedbackText.text = "Right!";
            feedbackText.color = Color.green; // 
            gameObject.SetActive(false); // Deactivate the monster after correct answer
        }
        else
        {
            feedbackText.text = "False,answer is:" + answer;
            feedbackText.color = Color.red;
        }
        feedbackText.gameObject.SetActive(true);

        Invoke("EndTest", 2f);
    }

    void EndTest()
    {
        isTestActive = false;
        testPanel.SetActive(false);
        feedbackText.gameObject.SetActive(false);

        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
}