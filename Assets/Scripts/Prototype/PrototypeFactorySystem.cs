using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PrototypeFactorySystem : MonoBehaviour
{
    [Header("Values: General")]
    public float pointScore;
    public PrestigeLevel prestigeLvl = PrestigeLevel.Prestige0;
    public float lvl1Value;
    [HideInInspector] public float lvl1InitialValue;
    public float lvl2Value;
    [HideInInspector] public float lvl2InitialValue;
    public float lvl3Value;
    [HideInInspector] public float lvl3InitialValue;
    public float lvl4Value;
    [HideInInspector] public float lvl4InitialValue;
    public float lvl5Value;
    [HideInInspector] public float lvl5InitialValue;
    public float lvl6Value;
    [HideInInspector] public float lvl6InitialValue;

    [Header("Debug Values")]
    public bool debugMode;
    public float instantPointAddition;
    private bool hasDebugRun;

    [Header("Object References: General")]
    public Collider basketTrigger;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI coinText;
    public List<GameObject> oneOffObjects = new List<GameObject>();
    private PrototypeGnomeCoinSystem ddolManager;

    private void OnEnable()
    {
        SetUpDDOLManager();
        SetUpDDOLManagerOneOff();
        ddolManager.Initialise();

        lvl1InitialValue = lvl1Value;
        Application.targetFrameRate = 60;
        AddScore(0);
    }

    private void SetUpDDOLManager()
    {
        ddolManager = GameObject.Find("proto_ddolManager").GetComponent<PrototypeGnomeCoinSystem>();
        ddolManager.gnomeCoinText = coinText;
    }

    private void SetUpDDOLManagerOneOff()
    {
        switch (ddolManager.gameObject.GetComponent<PrototypeDDOLManager>().isOneOffComplete)
        {
            case true:
                for (int i = 0; i < ddolManager.oneTimeObjects.Count; i++)
                {
                    Destroy(ddolManager.oneTimeObjects[i]);
                }
                Debug.Log("Bingo 2");
                break;
            case false:
                for (int i = 0; i < oneOffObjects.Count; i++)
                {
                    //ddolManager.oneTimeObjects.Add(oneOffObjects[i]);
                    //ddolManager.oneTimeObjectNames.Add(oneOffObjects[i].name);
                }
                ddolManager.gameObject.GetComponent<PrototypeDDOLManager>().isOneOffComplete = true;
                Debug.Log("Bingo 1");
                break;
        }
    }

    public void AddScore(float amount)
    {
        pointScore += amount + (amount * ddolManager.permanentValue);
        if (debugMode && !hasDebugRun) 
        {
            pointScore += instantPointAddition;
            hasDebugRun = true;
        }
        moneyText.text = "Profit: $" + RoundToNearestHundredth(pointScore).ToString("F2");
    }

    public void UpdatePrice(TextMeshProUGUI costText, bool isGnomeCoins, string beforeText, float newPrice, string afterText)
    {
        switch (isGnomeCoins)
        {
            case true:
                costText.text = beforeText + RoundToNearestHundredth(newPrice).ToString() + afterText;
                break;
            case false:
                costText.text = beforeText + RoundToNearestHundredth(newPrice).ToString("F2") + afterText;
                break;
        }
    }

    public float RoundToNearestHundredth(float value)
    {
        return (float)System.Math.Round(value, 2);
    }

    public void PauseGame(bool isPaused)
    {
        switch (isPaused)
        {
            case true:
                Time.timeScale = 0.0f;
                break;
            case false:
                Time.timeScale = 1.0f;
                break;
        }
    }

    public enum PrestigeLevel
    {
        Prestige0,
        Prestige1,
        Prestige2,
        Prestige3,
        Prestige4,
        Prestige5
    }
}