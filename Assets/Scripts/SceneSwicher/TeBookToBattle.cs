using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TeBookToBattle : MonoBehaviour
{
    public string nextScene;
    public void OnClick()
    {
        SceneManager.LoadScene(nextScene);
    }

}
