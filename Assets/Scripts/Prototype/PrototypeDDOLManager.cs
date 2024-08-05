using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrototypeDDOLManager : MonoBehaviour
{
    public bool isOneOffComplete = false;
    public int resetTimes = 0;
    public string sceneToLoad;

    // Start is called before the first frame update
    void OnEnable()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
