using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalText1;
    [SerializeField] private TextMeshProUGUI finalText2;
    [SerializeField] private float endingDelayTime;
    [SerializeField] private string sceneToLoad;
    private DDOLManager ddolManager;

    // Update is called once per frame
    void OnEnable()
    {
        ddolManager = GameObject.Find("ddolManager").GetComponent<DDOLManager>();

        finalText1.text = "Total gnomes manufactured:\n" + ddolManager.totalGnomesMade + "\n\nTotal upgrades bought:\n" + ddolManager.totalUpgradesBought + "\n\n\n\n\n ";
        finalText2.text = "\n\n\n\n\n\n\n\nTotal profit made:\n$" + ddolManager.RoundToNearestHundredth(ddolManager.totalProfitMade).ToString("F2");
        StartCoroutine(EndingDelay());
    }

    private IEnumerator EndingDelay()
    {
        yield return new WaitForSeconds(endingDelayTime);
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
