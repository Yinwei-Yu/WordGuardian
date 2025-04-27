using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // 对话文本
    public Image characterImage; // NPC 头像
    public TextMeshProUGUI characterNameText; // NPC 名字
    public Button nextButton; // 继续按钮

    private Queue<DialogueLine> dialogueQueue = new Queue<DialogueLine>(); // 对话队列
    private bool isDialogueLooping = false; // 是否循环对话

    void Start()
    {
        nextButton.interactable = false; // 禁用按钮
        Debug.Log("Initializing dialogue system...");

        LoadDialogueData();
        DisplayNextSentence();

        nextButton.interactable = true; // 启用按钮
    }

    void LoadDialogueData()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "StartTalk.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<DialogueData>(json);

            if (data != null && data.dialogue != null)
            {
                foreach (var line in data.dialogue)
                {
                    dialogueQueue.Enqueue(line); // 将每一句对话加入队列
                }
            }
            else
            {
                Debug.LogError("对话数据为空！");
            }
        }
        else
        {
            Debug.LogError("对话文件未找到！");
        }
    }

    public void DisplayNextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            if (isDialogueLooping)
            {
                LoadDialogueData(); // 如果需要循环，重新加载对话数据
            }
            else
            {
                EndDialogue(); // 结束对话
            }
            return;
        }

        var line = dialogueQueue.Dequeue(); // 获取下一句对话
        StopAllCoroutines(); // 停止之前的协程
        StartCoroutine(TypeSentence(line));
    }

    IEnumerator TypeSentence(DialogueLine line)
    {
        // 更新头像和名字
        characterNameText.text = line.name;
        string portraitPath = line.portrait;
        Sprite sprite = Resources.Load<Sprite>(portraitPath);
        if (sprite == null)
        {
            Debug.LogError("Failed to load sprite from path: " + portraitPath);
        }
        else
        {
            characterImage.sprite = sprite;
        }

        // 逐字显示对话内容
        dialogueText.text = "";
        foreach (char letter in line.sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null; // 每帧显示一个字符
        }
    }

    void EndDialogue()
    {
        Debug.Log("对话结束！");
        SceneManager.LoadScene("TeachingBook"); // 返回主菜单
    }

    public void OnNextButtonClick()
    {
        DisplayNextSentence(); // 显示下一句对话
    }
}

// 定义 JSON 数据结构
[System.Serializable]
public class DialogueData
{
    public List<DialogueLine> dialogue;
}

[System.Serializable]
public class DialogueLine
{
    public string name;
    public string portrait;
    public string sentence;
}