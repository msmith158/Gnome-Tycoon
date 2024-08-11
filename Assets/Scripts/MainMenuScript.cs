using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private Image blackScreen;
    [SerializeField] private float fadeTime = 1f;

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
        Color fixedColor = blackScreen.color;
        fixedColor.a = 0;
        blackScreen.color = fixedColor;
        blackScreen.CrossFadeAlpha(1, 2.0f, false);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(scene);
    }


}
