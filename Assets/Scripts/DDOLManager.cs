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

    // Counter values for the end of the game
    public int totalGnomesMade;
    public int totalUpgradesBought;
    public float totalProfitMade;

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

    public float RoundToNearestHundredth(float value)
    {
        return (float)System.Math.Round(value, 2);
    }
}
