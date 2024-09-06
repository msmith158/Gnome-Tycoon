using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FinalFactorySystem : MonoBehaviour
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
    [Range(1, 7)] public int productionLineAmount;
    private float switchPanelTime;
    private bool isProductionLinesSet = false;

    [Header("Debug Values")]
    public bool debugMode;
    public float instantPointAddition;
    private bool hasDebugRun;

    [Header("Object References: General")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI coinText;
    public List<GameObject> productionLines = new List<GameObject>();
    public List<GameObject> oneOffObjects = new List<GameObject>();
    private GnomeCoinSystem ddolManager;
    private DDOLManager ddolManager2;
    private GameObject switchPanelDismissVar;
    private GameObject switchPanelActivateVar;
    public Image gnomeCoinVignetteReference;
    [SerializeField] private TextMeshProUGUI debugMetrics;
    public Image manufacturingButtonImage;
    public Sprite manufacturingButtonUnpressed;
    public Sprite manufacturingButtonPressed;

    [Header("Object References: Audio")]
    [SerializeField] private AudioSource buttonSfxSource;
    [SerializeField] private AudioClip buttonInSfx;
    [SerializeField] private AudioClip buttonOutSfx;

    private void OnEnable()
    {
        SetUpDDOLManager();
        SetUpDDOLManagerOneOff();
        SetProductionLines();
        ddolManager.Initialise();
        
        switch (debugMode)
        {
            case true:
                debugMetrics.gameObject.SetActive(true);
                debugMetrics.GetComponent<NewDebugCanvas>().enabled = true;
                Debug.Log("Booper 1");
                break;
            case false:
                debugMetrics.gameObject.SetActive(false);
                debugMetrics.GetComponent<NewDebugCanvas>().enabled = false;
                Debug.Log("Booper 2");
                break;
        }

        lvl1InitialValue = lvl1Value;
        Application.targetFrameRate = 60;
        // Add code here for amount of production lines from save/load system
        AddScore(0);

        // Start intro sequence
        GetComponent<IntroSequence>().ProgressIntroStates();
    }

    private void SetUpDDOLManager()
    {
        // Set up all the DDOL manager stuff
        ddolManager = GameObject.Find("ddolManager").GetComponent<GnomeCoinSystem>();
        ddolManager2 = GameObject.Find("ddolManager").GetComponent<DDOLManager>();
        ddolManager.gnomeCoinText = coinText;
    }

    private void SetUpDDOLManagerOneOff()
    {
        // Functions for the one-off objects, meant for only one interaction
        for (int i = 0; i < oneOffObjects.Count; i++)
        {
            if (ddolManager.oneTimeObjectNames.Count != 0)
            {
                if (oneOffObjects[i].name != ddolManager.oneTimeObjectNames[i])
                {
                    // It's going here once you restart the level after the nuke detonation. See if you can figure out what's going on.
                    ddolManager.oneTimeObjectNames.Add(oneOffObjects[i].name);
                }
                else if (oneOffObjects[i].name == ddolManager.oneTimeObjectNames[i])
                {
                    Destroy(oneOffObjects[i]);
                    oneOffObjects.Remove(oneOffObjects[i]);
                }
            }
            else
            {
                ddolManager.oneTimeObjectNames.Add(oneOffObjects[i].name);
            }
        }
    }

    public void SetProductionLines()
    {
        // This code is just temporary to show off the feature, add code once save/load system is in
        switch (isProductionLinesSet)
        {
            case true:
                break;
            case false:
                foreach (var t in productionLines)
                {
                    t.SetActive(false);
                    isProductionLinesSet = true;
                }
                break;
        }
        for (int i = 0; i < productionLineAmount; i++)
        {
            productionLines[i].SetActive(true);
            float firstManufactureTime = productionLines[0].transform.GetComponentInChildren<FinalDispenser>().manufacturingTime;
            float firstConveyorSpeed = productionLines[0].transform.GetChild(0).GetComponentInChildren<FinalConveyor>().speed;
            productionLines[i].transform.GetComponentInChildren<FinalDispenser>().manufacturingTime = firstManufactureTime;
            productionLines[i].transform.GetChild(0).GetComponentInChildren<FinalConveyor>().speed = firstConveyorSpeed;
        }
    }

    public void AddScore(float amount)
    {
        pointScore += amount + (amount * ddolManager.permanentValue);
        ddolManager2.totalProfitMade += (amount + (amount * ddolManager.permanentValue));
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
                if (newPrice >= 0 && newPrice < 1000)
                {
                    Debug.Log("Bingus 1");
                    costText.text = beforeText + RoundToNearestHundredth(newPrice).ToString("F2") + afterText;
                }
                else if (newPrice >= 1000 && newPrice < 1000000)
                {
                    Debug.Log("Bingus 2");
                    costText.text = beforeText + (RoundToNearestHundredth(newPrice) / 1000).ToString("F2") + "K" + afterText;
                }
                else if (newPrice >= 1000000 && newPrice < 1000000000)
                {
                    Debug.Log("Bingus 3");
                    costText.text = beforeText + (RoundToNearestHundredth(newPrice) / 1000000).ToString("F2") + "M" + afterText;
                }
                else if (newPrice >= 1000000000 && newPrice < 1000000000000)
                {
                    Debug.Log("Bingus 4");
                    costText.text = beforeText + (RoundToNearestHundredth(newPrice) / 1000000000).ToString("F2") + "B" + afterText;
                }
                else
                {
                    Debug.Log("No Bingus");
                }
                //costText.text = beforeText + RoundToNearestHundredth(newPrice).ToString("F2") + afterText;
                break;
        }
    }

    public float RoundToNearestHundredth(float value)
    {
        return (float)System.Math.Round(value, 2);
    }

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

    public void ActivateDispensers()
    {
        buttonSfxSource.clip = buttonInSfx;
        buttonSfxSource.Play();
        manufacturingButtonImage.sprite = manufacturingButtonPressed;
        
        for (int i = 0; i < productionLineAmount; i++)
        {
            string dispenserName = new string("line0" + (i + 1) + "dispenserMachine");
            productionLines[i].transform.Find(dispenserName).GetComponent<FinalDispenser>().SpawnObject();
        }
    }

    public void FinishDispensing()
    {
        buttonSfxSource.clip = buttonOutSfx;
        buttonSfxSource.Play();
        manufacturingButtonImage.sprite = manufacturingButtonUnpressed;
    }

    public void ResetProductionLineAmount()
    {
        if (productionLineAmount != 1)
        {
            for (int i = 1; i < productionLines.Count; i++)
            {
                productionLines[i].SetActive(false);
            }

            productionLineAmount = 1;
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