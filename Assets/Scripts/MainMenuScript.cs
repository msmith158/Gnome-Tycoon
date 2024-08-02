using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        {
            EditorApplication.isPlaying = false;
        }
#else
        {
            Application.Quit();
        }
#endif
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
