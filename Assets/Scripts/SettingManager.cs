using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public Button saveButton;
    public Button mainMenuButton;
    public Button continueButton;

    void Start()
    {
        // 监听按钮点击事件
        saveButton.onClick.AddListener(SaveGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        continueButton.onClick.AddListener(ContinueGame);
    }

    /// <summary>
    /// 保存游戏进度
    /// </summary>
    public void SaveGame()
    {
        // 获取当前场景名称
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 调用 SaveLoadManager 保存进度
        SaveLoadManager.SaveProgress(currentSceneName);

        //Debug.Log($"Game saved! Scene: {currentSceneName}, Learned Words: {learnedWords}");
    }

    /// <summary>
    /// 返回主菜单
    /// </summary>
    public void GoToMainMenu()
    {
        // 加载主菜单场景
        SceneManager.LoadScene("MainMenuScene");

        // 恢复游戏
        Time.timeScale = 1f;
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void ContinueGame()
    {
        // 调用 GameManager 返回到上一个场景
        GameManager.Instance.ReturnToPreviousScene();
    }
}