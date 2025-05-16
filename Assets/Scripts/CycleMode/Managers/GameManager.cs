using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("场景设置")]
    public string mainMenuScene = "MainMenu";
    public string[] jsonFilePaths; // 在Inspector中设置所有JSON路径

    private string currentJsonPath;

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