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
        // �����ύ��ť����¼�
        saveButton.onClick.AddListener(SaveGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        continueButton.onClick.AddListener(ContinueGame);
    }
    /// <summary>
    /// ������Ϸ����
    /// </summary>
    public void SaveGame()
    {

        // ʹ��SaveLoadManager������Һ͵��˵�����ֵ�Լ�ѧϰ�ĵ��ʺͳ�������
        // �������ѧϰ�ĵ�����"example"������ʵ������޸�
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // ����Ƿ��ǵ�һ������
        if (currentSceneIndex <= 0)
        {
            Debug.LogWarning("No previous scene exists!");
        }
        // ͨ��������ȡ��һ����������Ϣ
        Scene previousScene = SceneManager.GetSceneByBuildIndex(currentSceneIndex - 1);

        SaveLoadManager.SaveProgress(BattleManager.instance.playerStats.health, BattleManager.instance.enemyStats.health, previousScene.name, "example");
        Debug.Log("Game saved!");
    }

    /// <summary>
    /// �������˵�
    /// </summary>
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    /// <summary>
    /// ������Ϸ
    /// </summary>
    public void ContinueGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}



