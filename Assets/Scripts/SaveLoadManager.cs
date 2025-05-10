using System.IO;
using UnityEngine;

// ���屣������ݽṹ
[System.Serializable]
public class SaveData
{
    public int playerHealth;
    public int enemyHealth;
    public string sceneName;
    public string learnedWord;
}

public static class SaveLoadManager
{
    private const string SAVE_FILE_NAME = "game_save.json";

    /// <summary>
    /// ������Ϸ���ȵ��ļ�
    /// </summary>
    /// <param name="playerHealth">��ҵ�ǰ����ֵ</param>
    /// <param name="enemyHealth">���˵�ǰ����ֵ</param>
    /// <param name="sceneName">��ǰ��������</param>
    /// <param name="learnedWord">ѧϰ�ĵ���</param>
    public static void SaveProgress(int playerHealth, int enemyHealth, string sceneName, string learnedWord)
    {
        // �����������ݶ���
        SaveData data = new SaveData
        {
            playerHealth = playerHealth,
            enemyHealth = enemyHealth,
            sceneName = sceneName,
            learnedWord = learnedWord
        };

        // ���������л�ΪJSON�ַ���
        string json = JsonUtility.ToJson(data);
        // ��ȡ�־û�����·��
        string filePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
        // ��JSON�ַ���д���ļ�
        File.WriteAllText(filePath, json);
    }

    /// <summary>
    /// ���ļ�������Ϸ����
    /// </summary>
    /// <returns>���ص���Ϸ����</returns>
    public static SaveData LoadProgress()
    {
        // ��ȡ�־û�����·��
        string filePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
        // ����ļ��Ƿ����
        if (File.Exists(filePath))
        {
            // ��ȡ�ļ�����
            string json = File.ReadAllText(filePath);
            // �����л�JSON�ַ���ΪSaveData����
            return JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            // �ļ�������ʱ����null
            return null;
        }
    }
}



