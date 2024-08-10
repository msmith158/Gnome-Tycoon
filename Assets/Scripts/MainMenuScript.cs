using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private Image blackScreen;
    [SerializeField] private float fadeTime;

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
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeOut(string scene)
    {
        blackScreen.enabled = true;
        blackScreen.CrossFadeAlpha(1, fadeTime, false);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(scene);
    }


}
