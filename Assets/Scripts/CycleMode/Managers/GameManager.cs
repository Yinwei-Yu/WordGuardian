// GameManager.cs
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Scene Management")]
    public string mainMenuScene = "MainMenu";

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

    public void EndGame()
    {
        // 显示结束界面或返回主菜单
        ReturnToMainMenu();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}