using System.IO;
using UnityEngine;

// 定义保存的数据结构
[System.Serializable]
public class SaveData
{
    public string sceneName;
}

public static class SaveLoadManager
{
    private const string SAVE_FILE_NAME = "game_save.json";

    /// <summary>
    /// 保存游戏进度到文件
    /// </summary>
    /// <param name="sceneName">当前场景名称</param>
    public static void SaveProgress(string sceneName)
    {
        // 创建保存数据对象
        SaveData data = new SaveData
        {
            sceneName = sceneName,
        };
        // 将数据序列化为JSON字符串
        string json = JsonUtility.ToJson(data);
        // 获取持久化数据路径
        string filePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
        // 将JSON字符串写入文件
        File.WriteAllText(filePath, json);
    }

    /// <summary>
    /// 从文件加载游戏进度
    /// </summary>
    /// <returns>加载的游戏数据</returns>
    public static SaveData LoadProgress()
    {
        // 获取持久化数据路径
        string filePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
        // 检查文件是否存在
        if (File.Exists(filePath))
        {
            // 读取文件内容
            string json = File.ReadAllText(filePath);
            // 反序列化JSON字符串为SaveData对象
            return JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            // 文件不存在时返回null
            return null;
        }
    }
}



