using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInitializer : MonoBehaviour
{
    public int totalEnemies = 2; // 场景中的敌人总数
    public string nextSceneName; // 下一个场景名称

    void Start()
    {
        // 初始化 EnemyManager
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.Initialize(totalEnemies, nextSceneName);
        }
        else
        {
            Debug.LogError("EnemyManager instance not found!");
        }
    }
}