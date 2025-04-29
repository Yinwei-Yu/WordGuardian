using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TextSwitcher : MonoBehaviour
{
    public TMP_Text[] textObjects; // 引用多个 TextMeshPro 对象
    public float displayDuration = 2f; // 每段文字显示的时间
    public float fadeDuration = 0.5f; // 淡入淡出持续时间
    public AudioSource audioSource; // 引用音频管理器
    private string nextSceneName = "StartTalk";
    void Start()
    {
        Debug.Log("textObjects.Length: " + textObjects.Length);
        // 开始协程以逐步显示文字
        StartCoroutine(SwitchTexts());
    }

    IEnumerator SwitchTexts()
    {
        int length = 0;
        foreach (TMP_Text textObject in textObjects)
        {
            CanvasGroup canvasGroup = textObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = textObject.gameObject.AddComponent<CanvasGroup>();
            }

            // 淡入
            canvasGroup.alpha = 0;
            // 显示完整段文字
            textObject.gameObject.SetActive(true);
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / fadeDuration;
                yield return null;
            }

            // 持续时间
            yield return new WaitForSeconds(displayDuration);

            // 淡出
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / fadeDuration;
                yield return null;
            }
            length += 1;
            Debug.Log("length: " + length);

            if (length == textObjects.Length - 1)
            {
                audioSource.Play(); // 播放音效
            }

            textObject.gameObject.SetActive(false);
        }
        SceneManager.LoadScene(nextSceneName);
    }
}