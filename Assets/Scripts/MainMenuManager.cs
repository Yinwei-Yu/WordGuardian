using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OnNewGameButtonClicked()
    {
        // 加载背景故事场景
        SceneManager.LoadScene("StoryScene");
    }

    public void OnSettingsButtonClicked()
    {
        // 加载设置界面场景
        SceneManager.LoadScene("SettingsScene");
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();

        Debug.Log("退出游戏!");
    }
}
