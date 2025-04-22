using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    public void OnReturnButtonClicked()
    {
        // 返回主界面
        SceneManager.LoadScene("MainMenuScene");
    }
}
