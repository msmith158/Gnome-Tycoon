using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DDOLManager : MonoBehaviour
{
    public bool isOneOffComplete = false;
    public int resetTimes = 0;
    public string sceneToLoad;
    public bool introSkipped = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(LoadDDOLManager());
    }

    private IEnumerator LoadDDOLManager()
    {
        yield return null;
        DontDestroyOnLoad(this.gameObject);
        yield return null;
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
