using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StorySceneBack : MonoBehaviour
{
    public void OnBackButtonClicked()
    {
        // 返回主界面
        SceneManager.LoadScene("MainMenuScene");
    }
}
