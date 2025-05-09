// QuestionManager.cs
// 在QuestionManager中添加



public class QuestionManager : MonoBehaviour
{
    [Header("Settings")]
    public string jsonFileName = "questions.json";
    public int optionsCount = 4;

    [Header("References")]
    public Text questionText;
    public Text explanationText;
    public Transform optionsPanel;
    public GameObject optionButtonPrefab;

    private List<QuestionData> allQuestions = new List<QuestionData>();
    private List<QuestionData> availableQuestions = new List<QuestionData>();
    private QuestionData currentQuestion;

    void Start()
    {
        LoadQuestions();
        GetNewQuestion();
    }

    void LoadQuestions()
    {
        string path = Path.Combine(Application.streamingAssetsPath, jsonFileName);
        string jsonData = File.ReadAllText(path);
        QuestionList loadedData = JsonUtility.FromJson<QuestionList>(jsonData);
        
        allQuestions = loadedData.questions;
        availableQuestions = new List<QuestionData>(allQuestions);
    }

    public void GetNewQuestion()
    {
        if (availableQuestions.Count == 0)
        {
            availableQuestions = new List<QuestionData>(allQuestions);
        }

        int randomIndex = Random.Range(0, availableQuestions.Count);
        currentQuestion = availableQuestions[randomIndex];
        availableQuestions.RemoveAt(randomIndex);

        DisplayQuestion();
    }

    void DisplayQuestion()
    {
        questionText.text = $"{currentQuestion.word}\n{currentQuestion.sentence}";
        explanationText.text = currentQuestion.explanation;
        CreateOptions();
    }

    void CreateOptions()
    {
        // 清空旧选项
        foreach (Transform child in optionsPanel)
        {
            Destroy(child.gameObject);
        }

        // 生成正确选项
        CreateOptionButton(currentQuestion.correctAnswer, true);

        // 生成错误选项
        List<string> wrongAnswers = allQuestions
            .Where(q => q != currentQuestion)
            .Select(q => q.correctAnswer)
            .Distinct()
            .OrderBy(x => Guid.NewGuid())
            .Take(optionsCount - 1)
            .ToList();

        foreach (string answer in wrongAnswers)
        {
            CreateOptionButton(answer, false);
        }

        // 随机排序选项
        foreach (Transform child in optionsPanel)
        {
            child.SetSiblingIndex(Random.Range(0, optionsCount));
        }
    }

    void CreateOptionButton(string answer, bool isCorrect)
    {
        GameObject buttonObj = Instantiate(optionButtonPrefab, optionsPanel);
        Button button = buttonObj.GetComponent<Button>();
        Text buttonText = buttonObj.GetComponentInChildren<Text>();
        
        buttonText.text = answer;
        button.onClick.AddListener(() => OnOptionSelected(isCorrect));
    }

    public void OnOptionSelected(bool isCorrect)
    {
        if (isCorrect)
        {
            GetNewQuestion();
        }
        else
        {
            // 处理游戏结束逻辑
            GameManager.Instance.EndGame();
        }
    }
}