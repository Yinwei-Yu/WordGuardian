using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance; // 单例模式

    private int enemyCount = 0; // 当前场景中的敌人数量
    private string nextSceneName; // 下一个场景名称

    void Awake()
    {
        // 确保 EnemyManager 是唯一的实例
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // 不随场景切换销毁
        }
        else
        {
            Destroy(gameObject); // 防止重复实例化
        }
    }

    /// <summary>
    /// 初始化敌人数量
    /// </summary>
    /// <param name="count">敌人总数</param>
    /// <param name="nextScene">下一个场景名称</param>
    public void Initialize(int count, string nextScene)
    {
        enemyCount = count;
        nextSceneName = nextScene;
    }

    /// <summary>
    /// 敌人被击败时调用
    /// </summary>
    public void OnEnemyDefeated()
    {
        enemyCount--;
        Debug.Log($"Enemies remaining: {enemyCount}");

        // 如果所有敌人都被消灭，加载下一个场景
        if (enemyCount <= 0)
        {
            LoadNextScene();
        }
    }

    /// <summary>
    /// 加载下一个场景
    /// </summary>
    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name is not set!");
        }
    }
}