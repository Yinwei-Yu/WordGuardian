using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button optionAButton;
    [SerializeField] private Button optionBButton;
    [SerializeField] private Button optionCButton;

    [Header("Error Dialog")]
    [SerializeField] private GameObject errorDialog;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private Button closeButton;
    [SerializeField] private float autoHideDelay = 3f;

    [Header("Animation Settings")]
    [SerializeField]
    private AnimationCurve easeOutBackCurve = new AnimationCurve(
        new Keyframe(0, 0, 0, 0),
        new Keyframe(1, 1, 3.5f, 3.5f)
    );
    [SerializeField]
    private AnimationCurve easeInBackCurve = new AnimationCurve(
        new Keyframe(0, 0, 3.5f, 3.5f),
        new Keyframe(1, 1, 0, 0)
    );

    [Header("Questions")]
    [SerializeField] private List<QuestionData> questions = new List<QuestionData>();

    private int currentQuestionIndex = 0;

    [System.Serializable]
    public class QuestionData
    {
        public string questionText;
        public string optionA;
        public string optionB;
        public string optionC;
        public int correctOption; // 1=A, 2=B, 3=C

        public string errorMessageA;
        public string errorMessageB;
        public string errorMessageC;

        public string GetErrorMessage(int optionIndex)
        {
            switch (optionIndex)
            {
                case 1: return string.IsNullOrEmpty(errorMessageA) ? "A选项不正确" : errorMessageA;
                case 2: return string.IsNullOrEmpty(errorMessageB) ? "B选项不正确" : errorMessageB;
                case 3: return string.IsNullOrEmpty(errorMessageC) ? "C选项不正确" : errorMessageC;
                default: return "这个选项不正确";
            }
        }
    }

    private void Start()
    {
        ConfigureButtonText(optionAButton);
        ConfigureButtonText(optionBButton);
        ConfigureButtonText(optionCButton);

        optionAButton.onClick.AddListener(() => OnOptionSelected(1));
        optionBButton.onClick.AddListener(() => OnOptionSelected(2));
        optionCButton.onClick.AddListener(() => OnOptionSelected(3));

        closeButton.onClick.AddListener(HideErrorDialog);
        LoadQuestion(currentQuestionIndex);
        errorDialog.SetActive(false);
    }

    private void ConfigureButtonText(Button button)
    {
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            buttonText.enableAutoSizing = true;
            buttonText.fontSizeMin = 12;
            buttonText.fontSizeMax = 36;
            buttonText.enableWordWrapping = true;
            buttonText.overflowMode = TextOverflowModes.Ellipsis;
            buttonText.margin = new Vector4(10, 5, 10, 5);
        }
    }

    private void LoadQuestion(int index)
    {
        if (index < 0 || index >= questions.Count)
        {
            questionText.text = "测验完成！";
            optionAButton.gameObject.SetActive(false);
            optionBButton.gameObject.SetActive(false);
            optionCButton.gameObject.SetActive(false);
            return;
        }

        QuestionData currentQuestion = questions[index];
        questionText.text = currentQuestion.questionText;

        optionAButton.gameObject.SetActive(true);
        optionBButton.gameObject.SetActive(true);
        optionCButton.gameObject.SetActive(true);

        SetButtonText(optionAButton, "A. " + currentQuestion.optionA);
        SetButtonText(optionBButton, "B. " + currentQuestion.optionB);
        SetButtonText(optionCButton, "C. " + currentQuestion.optionC);
    }

    private void SetButtonText(Button button, string text)
    {
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            buttonText.text = text;
            LayoutRebuilder.ForceRebuildLayoutImmediate(button.GetComponent<RectTransform>());
            Canvas.ForceUpdateCanvases();
        }
    }

    private void OnOptionSelected(int selectedOption)
    {
        QuestionData currentQuestion = questions[currentQuestionIndex];

        if (selectedOption == currentQuestion.correctOption)
        {
            currentQuestionIndex++;
            LoadQuestion(currentQuestionIndex);
        }
        else
        {
            ShowErrorDialog(currentQuestion, selectedOption);

            switch (selectedOption)
            {
                case 1: optionAButton.gameObject.SetActive(false); break;
                case 2: optionBButton.gameObject.SetActive(false); break;
                case 3: optionCButton.gameObject.SetActive(false); break;
            }
        }
    }

    private void ShowErrorDialog(QuestionData question, int selectedOption)
    {
        string message = question.GetErrorMessage(selectedOption);
        errorText.text = message;
        errorDialog.SetActive(true);

        StartCoroutine(ScaleDialog(errorDialog.transform, Vector3.one, 0.3f, easeOutBackCurve));
        CancelInvoke("HideErrorDialog");
        Invoke("HideErrorDialog", autoHideDelay);
    }

    private void HideErrorDialog()
    {
        StartCoroutine(ScaleDialog(errorDialog.transform, Vector3.zero, 0.2f, easeInBackCurve,
            () => errorDialog.SetActive(false)));
    }

    private IEnumerator ScaleDialog(Transform target, Vector3 targetScale, float duration,
                                  AnimationCurve easeCurve, System.Action onComplete = null)
    {
        Vector3 initialScale = target.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = easeCurve.Evaluate(elapsed / duration);
            target.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }

        target.localScale = targetScale;
        onComplete?.Invoke();
    }
}
