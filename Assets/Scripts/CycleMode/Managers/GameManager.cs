using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("场景设置")]
    public string mainMenuScene = "MainMenu";
    public string[] jsonFilePaths; // 在Inspector中设置所有JSON路径

    private string currentJsonPath;

    private bool isSettingsOpen = false; // 设置场景是否打开
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // 检测 ESC 键
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingsScene(); // 切换设置场景显示状态
        }
    }

    /// <summary>
    /// 切换设置界面显示状态
    /// </summary>
    public void ToggleSettingsScene()
    {
        if (!isSettingsOpen)
        {
            // 记录当前场景名称
            string currentSceneName = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetString("PreviousSceneName", currentSceneName);

            // 加载设置场景
            SceneManager.LoadScene("SettingsScene");
            isSettingsOpen = true;

            // 暂停游戏
            Time.timeScale = 0f;
        }
    }

    /// <summary>
    /// 返回到上一个场景
    /// </summary>
    public void ReturnToPreviousScene()
    {
        // 获取上一个场景名称
        string previousSceneName = PlayerPrefs.GetString("PreviousSceneName", "MainMenuScene");

        // 加载上一个场景
        SceneManager.LoadScene(previousSceneName);

        // 恢复游戏
        Time.timeScale = 1f;
        isSettingsOpen = false;
    }

    public void SetCurrentJsonPath(string path)
    {
        currentJsonPath = path;
    }

    public void StartGameWithJson(string path)
    {
        SetCurrentJsonPath(path);
        SceneManager.LoadScene("GameScene"); // 切换到游戏场景
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public string GetCurrentJsonPath()
    {
        return currentJsonPath;
    }

    public void EndGame()
    {
        // 1. 显示游戏结束UI
        Debug.Log("游戏结束");


    }
}