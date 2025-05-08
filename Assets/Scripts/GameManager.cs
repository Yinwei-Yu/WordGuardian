using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    public GameObject settingsPanel;
    public bool isGamePaused = false;

    private SaveData currentSave;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
    }

    void ToggleSettings()
    {
        isGamePaused = !isGamePaused;
        settingsPanel.SetActive(isGamePaused);
        Time.timeScale = isGamePaused ? 0 : 1;
    }

    // 保存当前游戏进度
    public void SaveGame()
    {
        currentSave = new SaveData
        {
            sceneName = SceneManager.GetActiveScene().name,
            playerPosition = FindObjectOfType<PlayerController>().transform.position,
            // 添加其他需要保存的数据
            gameTime = Time.time
        };

        string jsonData = JsonUtility.ToJson(currentSave);
        PlayerPrefs.SetString("SaveData", jsonData);
        PlayerPrefs.Save();
    }

    // 加载保存的游戏进度
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("SaveData"))
        {
            string jsonData = PlayerPrefs.GetString("SaveData");
            currentSave = JsonUtility.FromJson<SaveData>(jsonData);
            SceneManager.LoadScene(currentSave.sceneName);
            // 场景加载完成后恢复数据
        }
    }

    // 新游戏
    public void NewGame()
    {
        PlayerPrefs.DeleteKey("SaveData");
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

[System.Serializable]
public class SaveData
{
    public string sceneName;
    public Vector3 playerPosition;
    public float gameTime;
    // 添加其他需要保存的字段
}



public class SettingsMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField volumeInput;
    public Button saveButton;
    public Button quitButton;

    void Start()
    {
        saveButton.onClick.AddListener(SaveSettings);
        quitButton.onClick.AddListener(QuitGame);
        LoadSettings();
    }

    void SaveSettings()
    {
        // 保存游戏进度
        GameManager.Instance.SaveGame();

        // 保存游戏设置
        PlayerPrefs.SetFloat("MasterVolume", float.Parse(volumeInput.text));
        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            volumeInput.text = PlayerPrefs.GetFloat("MasterVolume").ToString();
        }
    }

    void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
