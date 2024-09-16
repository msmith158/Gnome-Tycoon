using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FinalFactorySystem : MonoBehaviour
{
    [Header("Values: General")]
    public double pointScore;
    public PrestigeLevel prestigeLvl = PrestigeLevel.Prestige0;
    public double lvl1Value;
    [HideInInspector] public float lvl1InitialValue;
    public double lvl2Value;
    [HideInInspector] public float lvl2InitialValue;
    public double lvl3Value;
    [HideInInspector] public float lvl3InitialValue;
    public double lvl4Value;
    [HideInInspector] public float lvl4InitialValue;
    public double lvl5Value;
    [HideInInspector] public float lvl5InitialValue;
    public double lvl6Value;
    [HideInInspector] public float lvl6InitialValue;
    [Range(1, 14)] public int productionLineAmount;
    public int automatedLineAmount;
    private float switchPanelTime;
    private bool isProductionLinesSet = false;
    private int roomNumber;
    public float cameraPosIncrementX;

    [Header("Debug Values")]
    public bool debugMode;
    public double instantPointAddition;
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
    [SerializeField] private GameObject cameraHolderHolder;
    [SerializeField] private List<GameObject> room1 = new List<GameObject>();
    [SerializeField] private List<GameObject> room2 = new List<GameObject>();
    [SerializeField] private List<GameObject> room3 = new List<GameObject>();
    [SerializeField] private List<GameObject> room4 = new List<GameObject>();
    [SerializeField] private List<GameObject> room5 = new List<GameObject>();
    [SerializeField] private List<GameObject> room6 = new List<GameObject>();
    [SerializeField] private List<GameObject> room7 = new List<GameObject>();

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

        lvl1InitialValue = (float)lvl1Value;
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

    public void AddScore(double amount)
    {
        pointScore += amount + (amount * ddolManager.permanentValue);
        ddolManager2.totalProfitMade += (amount + (amount * ddolManager.permanentValue));
        if (debugMode && !hasDebugRun) 
        {
            pointScore += instantPointAddition;
            hasDebugRun = true;
        }

        if (pointScore >= 0 && pointScore < 1000000)
        {
            moneyText.text = "Profit: $" + RoundToNearestHundredth(pointScore).ToString("F2");
        }
        else if (pointScore >= 1000000 && pointScore < 1000000000)
        {
            moneyText.text = "Profit: $" + (RoundToNearestHundredth(pointScore) / 1000000).ToString("F2") + " Million";
        }
        else if (pointScore >= 1000000000 && pointScore < 1000000000000)
        {
            moneyText.text = "Profit: $" + (RoundToNearestHundredth(pointScore) / 1000000000).ToString("F2") + " Billion";
        }
        else if (pointScore >= 1000000000000 && pointScore < 1000000000000000)
        {
            moneyText.text = "Profit: $" + (RoundToNearestHundredth(pointScore) / 1000000000000).ToString("F2") + " Trillion";
        }
    }

    public void UpdatePrice(TextMeshProUGUI costText, bool isGnomeCoins, string beforeText, double newPrice, string afterText)
    {
        switch (isGnomeCoins)
        {
            case true:
                costText.text = beforeText + RoundToNearestHundredth(newPrice).ToString() + afterText;
                break;
            case false:
                if (newPrice >= 0 && newPrice < 1000)
                {
                    costText.text = beforeText + RoundToNearestHundredth(newPrice).ToString("F2") + afterText;
                }
                else if (newPrice >= 1000 && newPrice < 1000000)
                {
                    costText.text = beforeText + (RoundToNearestHundredth(newPrice) / 1000).ToString("F2") + "K" + afterText;
                }
                else if (newPrice >= 1000000 && newPrice < 1000000000)
                {
                    costText.text = beforeText + (RoundToNearestHundredth(newPrice) / 1000000).ToString("F2") + "M" + afterText;
                }
                else if (newPrice >= 1000000000 && newPrice < 1000000000000)
                {
                    costText.text = beforeText + (RoundToNearestHundredth(newPrice) / 1000000000).ToString("F2") + "B" + afterText;
                }
                //costText.text = beforeText + RoundToNearestHundredth(newPrice).ToString("F2") + afterText;
                break;
        }
    }
    
    public void UpdateProfit(TextMeshProUGUI costText, bool isGnomeCoins, string beforeText, double newPrice, string afterText)
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

    public double RoundToNearestHundredth(double value)
    {
        return System.Math.Round(value, 2);
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

    public void ClearDispensers()
    {
        for (int i = 0; i < productionLineAmount; i++)
        {
            string dispenserName = new string("line0" + (i + 1) + "dispenserMachine");
            foreach (GameObject g in productionLines[i].transform.Find(dispenserName).GetComponent<FinalDispenser>()
                         .objectsList)
            {
                Destroy(g);
            }
            //productionLines[i].transform.Find(dispenserName).GetComponent<FinalDispenser>().SpawnObject();
        }
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

    public void AutomatedDispenser()
    {
        string dispenserName = new string("line0" + (automatedLineAmount + 1) + "dispenserMachine");
        if (!productionLines[automatedLineAmount].transform.Find(dispenserName).GetComponent<FinalDispenser>().isAutoRunning)
        {
            productionLines[automatedLineAmount].transform.Find(dispenserName).GetComponent<FinalDispenser>()
                .isAutoRunning = true;
            StartCoroutine(productionLines[automatedLineAmount].transform.Find(dispenserName).GetComponent<FinalDispenser>().AutomatedSpawn());
        }

        automatedLineAmount++;
    }

    public void StopAutomatedDispenser()
    {
        for (int i = 0; i < productionLines.Count; i++)
        {
            string dispenserName = new string("line0" + (i + 1) + "dispenserMachine");
            if (productionLines[i].transform.Find(dispenserName).GetComponent<FinalDispenser>().isAutoRunning)
            {
                StopCoroutine(productionLines[i].transform.Find(dispenserName).GetComponent<FinalDispenser>().AutomatedSpawn());
            }
        }
    }

    public void SwitchRoom(bool increment)
    {
        int lastRoomNumber = roomNumber;
        Debug.Log("lastRoomNumber: " + lastRoomNumber);
        switch (increment)
        {
            case true:
                if (roomNumber != 6)
                {
                    roomNumber++;
                    cameraHolderHolder.transform.position = new Vector3(
                        cameraHolderHolder.transform.position.x + cameraPosIncrementX,
                        cameraHolderHolder.transform.position.y, cameraHolderHolder.transform.position.z);
                }
                else return;
                break;
            case false:
                if (roomNumber != 0)
                {
                    roomNumber--;
                    cameraHolderHolder.transform.position = new Vector3(
                        cameraHolderHolder.transform.position.x - cameraPosIncrementX,
                        cameraHolderHolder.transform.position.y, cameraHolderHolder.transform.position.z);
                }
                else return;
                break;
        }
        Debug.Log("roomNumber: " + roomNumber);

        switch (roomNumber)
        {
            case 0:
                foreach (GameObject obj in room1)
                {
                    ChangeComponentStateRecursively(true, obj);
                }
                switch (lastRoomNumber)
                {
                    case 1:
                        foreach (GameObject obj in room2)
                        {
                            ChangeComponentStateRecursively(false, obj);
                        }
                        break;
                }
                break;
            case 1:
                foreach (GameObject obj in room2)
                {
                    ChangeComponentStateRecursively(true, obj);
                }
                switch (lastRoomNumber)
                {
                    case 0:
                        foreach (GameObject obj in room1)
                        {
                            ChangeComponentStateRecursively(false, obj);
                        }
                        break;
                    case 2:
                        foreach (GameObject obj in room3)
                        {
                            ChangeComponentStateRecursively(false, obj);
                        }
                        break;
                }
                break;
            case 2:
                foreach (GameObject obj in room3)
                {
                    ChangeComponentStateRecursively(true, obj);
                }
                switch (lastRoomNumber)
                {
                    case 1:
                        foreach (GameObject obj in room2)
                        {
                            ChangeComponentStateRecursively(false, obj);
                        }
                        break;
                    case 3:
                        foreach (GameObject obj in room4)
                        {
                            ChangeComponentStateRecursively(false, obj);
                        }
                        break;
                }
                break;
            case 4:
                foreach (GameObject obj in room4)
                {
                    ChangeComponentStateRecursively(true, obj);
                }
                switch (lastRoomNumber)
                {
                    case 3:
                        foreach (GameObject obj in room3)
                        {
                            ChangeComponentStateRecursively(false, obj);
                        }
                        break;
                    case 5:
                        foreach (GameObject obj in room5)
                        {
                            ChangeComponentStateRecursively(false, obj);
                        }
                        break;
                }
                break;
            case 5:
                foreach (GameObject obj in room5)
                {
                    ChangeComponentStateRecursively(true, obj);
                }
                switch (lastRoomNumber)
                {
                    case 4:
                        foreach (GameObject obj in room4)
                        {
                            ChangeComponentStateRecursively(false, obj);
                        }
                        break;
                    case 6:
                        foreach (GameObject obj in room6)
                        {
                            ChangeComponentStateRecursively(false, obj);
                        }
                        break;
                }
                break;
            case 6:
                foreach (GameObject obj in room6)
                {
                    ChangeComponentStateRecursively(true, obj);
                }
                switch (lastRoomNumber)
                {
                    case 5:
                        foreach (GameObject obj in room5)
                        {
                            ChangeComponentStateRecursively(false, obj);
                        }
                        break;
                    case 7:
                        foreach (GameObject obj in room7)
                        {
                            ChangeComponentStateRecursively(false, obj);
                        }
                        break;
                }
                break;
            case 7:
                foreach (GameObject obj in room7)
                {
                    ChangeComponentStateRecursively(true, obj);
                }
                switch (lastRoomNumber)
                {
                    case 6:
                        foreach (GameObject obj in room6)
                        {
                            ChangeComponentStateRecursively(false, obj);
                        }
                        break;
                }
                break;
        }
        
        cameraHolderHolder.transform.GetChild(0).GetComponent<CameraMoveMouse>().ChangeBounds(increment);
        cameraHolderHolder.transform.GetChild(0).GetComponent<CameraMoveTouch>().ChangeBounds(increment);
    }

    private void ChangeComponentStateRecursively(bool enable, GameObject obj)
    {
        if (obj.GetComponent<MeshRenderer>())
        {
            obj.GetComponent<MeshRenderer>().enabled = enable;
        }
        else if (obj.GetComponent<Light>())
        {
            obj.GetComponent<Light>().enabled = enable;
        }

        foreach (Transform child in obj.transform)
        {
            ChangeComponentStateRecursively(enable, child.gameObject);
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