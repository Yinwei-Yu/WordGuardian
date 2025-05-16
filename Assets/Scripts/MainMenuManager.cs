using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManagerManager : MonoBehaviour
{
    public Button loadButton;
    public Button quitButton;
    public Button settingButton;
    public Button startNewGameButton;

    void Start()
    {
        // 监听提交按钮点击事件
        loadButton.onClick.AddListener(LoadGame);
        quitButton.onClick.AddListener(QuitGame);
        settingButton.onClick.AddListener(Setting);
        startNewGameButton.onClick.AddListener(StartNewGame);
    }

    /// <summary>
    /// 加载游戏进度
    /// </summary>
    public void LoadGame()
    {
        // 使用SaveLoadManager加载游戏数据
        SaveData loadData = SaveLoadManager.LoadProgress();
        if (loadData != null)
        {
            // 加载场景
            SceneManager.LoadScene(loadData.sceneName);
        }
        else
        {
            Debug.LogWarning("No save data found.");
        }
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game quit!");
    }

    /// <summary>
    /// 开始新游戏，进入引导对话
    /// </summary>
    public void StartNewGame()
    {
        SceneManager.LoadScene("StoryScene");
    }
    /// <summary>
    /// 进入设置界面
    /// </summary>
    public void Setting()
    {
        SceneManager.LoadScene("SettingsScene");
    }
}



