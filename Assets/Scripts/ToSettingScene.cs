using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSceneController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("SettingsScene");
        }
    }
}