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
        // 监听提交按钮点击事件
        saveButton.onClick.AddListener(SaveGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        continueButton.onClick.AddListener(ContinueGame);
    }
    /// <summary>
    /// 保存游戏进度
    /// </summary>
    public void SaveGame()
    {

        // 使用SaveLoadManager保存玩家和敌人的生命值以及学习的单词和场景名称
        // 这里假设学习的单词是"example"，根据实际情况修改
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // 检查是否是第一个场景
        if (currentSceneIndex <= 0)
        {
            Debug.LogWarning("No previous scene exists!");
        }
        // 通过索引获取上一个场景的信息
        Scene previousScene = SceneManager.GetSceneByBuildIndex(currentSceneIndex - 1);

        SaveLoadManager.SaveProgress(BattleManager.instance.playerStats.health, BattleManager.instance.enemyStats.health, previousScene.name, "example");
        Debug.Log("Game saved!");
    }

    /// <summary>
    /// 返回主菜单
    /// </summary>
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void ContinueGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}



