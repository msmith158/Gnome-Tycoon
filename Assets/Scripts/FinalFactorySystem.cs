using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

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
    [Range(1, 49)] public int productionLineAmount;
    public int automatedLineAmount;
    private float switchPanelTime;
    private bool isProductionLinesSet = false;
    private int roomNumber;
    public float cameraPosIncrementX;
    [SerializeField] private AnimationCurve movementCurve;
    private Vector3 initCameraPos;
    private Vector3 newPos;
    private bool stopCall = false;
    private bool multipleRoomVisible = false;

    [Header("Debug Values")]
    public bool debugMode;
    public double instantPointAddition;
    private bool hasDebugRun;

    [Header("Object References: General")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI coinText;
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
    [SerializeField] private TextMeshPro factoryRoomNumberText;

    [Header("Object References: Audio")]
    [SerializeField] private AudioSource buttonSfxSource;
    [SerializeField] private AudioClip buttonInSfx;
    [SerializeField] private AudioClip buttonOutSfx;
    
    [Header("Object References: Lists")]
    public List<GameObject> productionLines = new List<GameObject>();
    public List<GameObject> productionLineGeos = new List<GameObject>();
    public List<GameObject> oneOffObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> room1SpecificObjs = new List<GameObject>();
    [SerializeField] private List<GameObject> room2SpecificObjs = new List<GameObject>();
    [SerializeField] private List<GameObject> room3SpecificObjs = new List<GameObject>();
    [SerializeField] private List<GameObject> room4SpecificObjs = new List<GameObject>();
    [SerializeField] private List<GameObject> room5SpecificObjs = new List<GameObject>();
    [SerializeField] private List<GameObject> room6SpecificObjs = new List<GameObject>();
    [SerializeField] private List<GameObject> room7SpecificObjs = new List<GameObject>();
    [SerializeField] private List<GameObject> roomSwitchConstantObjs = new List<GameObject>();

    private void OnEnable()
    {
        SetUpDDOLManager();
        SetUpDDOLManagerOneOff();
        SetProductionLines();
        ddolManager.Initialise();
        
        switch (debugMode) // Set debug metrics
        {
            case true:
                debugMetrics.gameObject.SetActive(true);
                debugMetrics.GetComponent<NewDebugCanvas>().enabled = true;
                break;
            case false:
                debugMetrics.gameObject.SetActive(false);
                debugMetrics.GetComponent<NewDebugCanvas>().enabled = false;
                break;
        }

        lvl1InitialValue = (float)lvl1Value;
        Application.targetFrameRate = 60;
        // Add code here for amount of production lines from save/load system
        AddScore(0);
        initCameraPos = cameraHolderHolder.transform.position;

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
                }

                foreach (var t in productionLineGeos)
                {
                    t.SetActive(false);
                    isProductionLinesSet = true;
                }
                break;
        }
        for (int i = 0; i < productionLineAmount; i++)
        {
            productionLines[i].SetActive(true);
            switch (multipleRoomVisible) // Check whether the player has the multiple room visibility option on or off
            {
                case true:
                    switch (roomNumber) // Check which room the player is in currently
                    {
                        case 0:
                            if (i < 14) { productionLineGeos[i].SetActive(true); ChangeComponentStateRecursively(true, productionLineGeos[i]); }
                            break;
                        case 1:
                            if (i < 21) { productionLineGeos[i].SetActive(true); ChangeComponentStateRecursively(true, productionLineGeos[i]); }
                            break;
                        case 2:
                            if (i >= 7 && i < 28) { productionLineGeos[i - 7].SetActive(true); ChangeComponentStateRecursively(true, productionLineGeos[i - 7]); }
                            break;
                        case 3:
                            if (i >= 14 && i < 35) { productionLineGeos[i - 14].SetActive(true); ChangeComponentStateRecursively(true, productionLineGeos[i - 14]); }
                            break;
                        case 4:
                            if (i >= 21 && i < 42) { productionLineGeos[i - 21].SetActive(true); ChangeComponentStateRecursively(true, productionLineGeos[i - 21]); }
                            break;
                        case 5:
                            if (i >= 28 && i < 49) { productionLineGeos[i - 28].SetActive(true); ChangeComponentStateRecursively(true, productionLineGeos[i - 28]); }
                            break;
                        case 6:
                            if (i >= 35 && i < 49) { productionLineGeos[i - 35].SetActive(true); ChangeComponentStateRecursively(true, productionLineGeos[i - 35]); }
                            break;
                    }
                    break;
                case false:
                    switch (roomNumber)
                    {
                        case 0:
                            if (i < 7) { productionLineGeos[i].SetActive(true); ChangeComponentStateRecursively(true, productionLineGeos[i]); }
                            break;
                        case 1:
                            if (i >= 7 && i < 14) { productionLineGeos[i - 7].SetActive(true); ChangeComponentStateRecursively(true, productionLineGeos[i - 7]); }
                            break;
                        case 2:
                            if (i >= 14 && i < 21) { productionLineGeos[i - 14].SetActive(true); ChangeComponentStateRecursively(true, productionLineGeos[i - 14]); }
                            break;
                        case 3:
                            if (i >= 21 && i < 28) { productionLineGeos[i - 21].SetActive(true); ChangeComponentStateRecursively(true, productionLineGeos[i - 21]); }
                            break;
                        case 4:
                            if (i >= 28 && i < 35) { productionLineGeos[i - 28].SetActive(true); ChangeComponentStateRecursively(true, productionLineGeos[i - 28]); }
                            break;
                        case 5:
                            if (i >= 35 && i < 42) { productionLineGeos[i - 35].SetActive(true); ChangeComponentStateRecursively(true, productionLineGeos[i - 35]); }
                            break;
                        case 6:
                            if (i >= 42 && i < 49) { productionLineGeos[i - 42].SetActive(true); ChangeComponentStateRecursively(true, productionLineGeos[i - 42]); }
                            break;
                    }
                    break;
            }
            // Make sure all production lines are set to the same values as the first production line
            float firstManufactureTime = productionLines[0].transform.GetComponentInChildren<FinalDispenser>().manufacturingTime;
            float firstConveyorSpeed = productionLines[0].transform.GetChild(0).GetComponentInChildren<FinalConveyor>().speed;
            productionLines[i].transform.GetComponentInChildren<FinalDispenser>().manufacturingTime = firstManufactureTime;
            productionLines[i].transform.GetChild(0).GetComponentInChildren<FinalConveyor>().speed = firstConveyorSpeed;
        }
    }

    public void ToggleMultipleRoomVisibility(bool enable)
    {
        switch (enable)
        {
            case true: 
                multipleRoomVisible = true;
                break;
            case false:
                multipleRoomVisible = false;
                break;
        }
        foreach (var t in productionLineGeos)
        {
            t.SetActive(false);
        }
        SetProductionLines();
    }

    public void AddScore(double amount) // For when the gnomes reach the end of the production line
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

    public void UpdatePrice(TextMeshProUGUI costText, bool isGnomeCoins, string beforeText, double newPrice, string afterText) // For an upgrade's price when it is bought
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
    
    public void UpdateProfit(TextMeshProUGUI costText, bool isGnomeCoins, string beforeText, double newPrice, string afterText) // For the profit counter when an upgrade is bought
    {
        switch (isGnomeCoins)
        {
            case true:
                costText.text = beforeText + RoundToNearestHundredth(newPrice).ToString() + afterText;
                break;
            case false:
                //costText.text = beforeText + RoundToNearestHundredth(newPrice).ToString("F2") + afterText;
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
            string dispenserName;
            if (i < 9) dispenserName = new string("line0" + (i + 1) + "dispenserMachine");
            else dispenserName = new string("line" + (i + 1) + "dispenserMachine"); 
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
                productionLineGeos[i].SetActive(false);
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
            string dispenserName;
            if (i < 9) dispenserName = new string("line0" + (i + 1) + "dispenserMachine");
            else dispenserName = new string("line" + (i + 1) + "dispenserMachine"); 
            if (productionLines[i].transform.Find(dispenserName).GetComponent<FinalDispenser>().isAutoRunning)
            {
                StopCoroutine(productionLines[i].transform.Find(dispenserName).GetComponent<FinalDispenser>().AutomatedSpawn());
            }
        }
    }

    public void SwitchRoom(bool increment)
    {
        int lastRoomNumber = roomNumber;
        switch (increment)
        {
            case true:
                if (roomNumber != 6)
                {
                    roomNumber++;
                    stopCall = true; // Wait for the while loop in the SwitchRoomCamera coroutine to end
                    StopCoroutine(SwitchRoomCamera());
                    StartCoroutine(SwitchRoomCamera());
                    switch (multipleRoomVisible)
                    {
                        case true:
                            if (roomNumber != 0 || roomNumber != 6)
                            {
                                foreach (GameObject obj in roomSwitchConstantObjs)
                                {
                                    obj.transform.position = new Vector3(obj.transform.position.x + cameraPosIncrementX,
                                        obj.transform.position.y, obj.transform.position.z);
                                }
                            }
                            break;
                        case false:
                            foreach (GameObject obj in roomSwitchConstantObjs)
                            {
                                obj.transform.position = new Vector3(obj.transform.position.x + cameraPosIncrementX,
                                    obj.transform.position.y, obj.transform.position.z);
                            }
                            break;
                    }
                }
                else return;
                break;
            case false:
                if (roomNumber != 0)
                {
                    roomNumber--;
                    stopCall = true; // Wait for the while loop in the SwitchRoomCamera coroutine to end
                    StopCoroutine(SwitchRoomCamera());
                    StartCoroutine(SwitchRoomCamera());
                    switch (multipleRoomVisible)
                    {
                        case true:
                            if (roomNumber != 0 || roomNumber != 6)
                            {
                                foreach (GameObject obj in roomSwitchConstantObjs)
                                {
                                    obj.transform.position = new Vector3(obj.transform.position.x - cameraPosIncrementX,
                                        obj.transform.position.y, obj.transform.position.z);
                                }
                            }
                            break;
                        case false:
                            foreach (GameObject obj in roomSwitchConstantObjs)
                            {
                                obj.transform.position = new Vector3(obj.transform.position.x - cameraPosIncrementX,
                                    obj.transform.position.y, obj.transform.position.z);
                            }
                            break;
                    }
                }
                else return;
                break;
        }

        string roomNumberString = (roomNumber + 1).ToString();
        factoryRoomNumberText.text = roomNumberString;

        #region SpecificRoomSwitchingFunctions
        switch (roomNumber) // Determining which room the player has switched to
        {
            case 0:
                RoomSwitchSetGeo(room1SpecificObjs, 14, 0, 7, 0, lastRoomNumber, false, false, -1, null, true, true, 1, room3SpecificObjs, null, room2SpecificObjs);
                break;
            case 1:
                RoomSwitchSetGeo(room2SpecificObjs, 21, 0, 14, 7, lastRoomNumber, false,true, 0, null, true, true, 2, room4SpecificObjs, room1SpecificObjs, room3SpecificObjs);
                break;
            case 2:
                RoomSwitchSetGeo(room3SpecificObjs, 28, 7, 21, 14, lastRoomNumber, true, true, 1, room1SpecificObjs, true, true, 3, room5SpecificObjs, room2SpecificObjs, room4SpecificObjs);
                break;
            case 3:
                RoomSwitchSetGeo(room4SpecificObjs, 35, 14, 28, 21, lastRoomNumber, true, true, 2, room2SpecificObjs, true, true, 4, room6SpecificObjs, room3SpecificObjs, room5SpecificObjs);
                break;
            case 4:
                RoomSwitchSetGeo(room5SpecificObjs, 42, 21, 35, 28, lastRoomNumber, true, true, 3, room3SpecificObjs, true, true, 5, room7SpecificObjs, room4SpecificObjs, room6SpecificObjs);
                break;
            case 5:
                RoomSwitchSetGeo(room6SpecificObjs, 49, 28, 42, 35, lastRoomNumber, true, true, 4, room4SpecificObjs, false, true, 6, null, room5SpecificObjs, room7SpecificObjs);
                break;
            case 6:
                RoomSwitchSetGeo(room7SpecificObjs, 49, 35, 49, 42, lastRoomNumber, true, true,5, room5SpecificObjs, false, false, 7, null, room6SpecificObjs, null);
                break;
        }
        #endregion
        cameraHolderHolder.transform.GetChild(0).GetComponent<CameraMoveMouse>().ChangeBounds(increment);
        cameraHolderHolder.transform.GetChild(0).GetComponent<CameraMoveTouch>().ChangeBounds(increment);
    }

    // Req 1 is for checking rooms behind the current room, req 2 is for rooms in front
    private void RoomSwitchSetGeo(List<GameObject> roomSpecificObjs, int productionLineLimitExtended, int productionLineThresholdExtended, int productionLineLimit, int productionLineThreshold, int lastRoomNumber, bool hasLastRoomNumberExtReq1, bool hasLastRoomNumberReq1,  int lastRoomNumberReq1, List<GameObject> behindRoomSpecificObjsExtended, bool hasLastRoomNumberExtReq2, bool hasLastRoomNumberReq2, int lastRoomNumberReq2, List<GameObject> aheadRoomSpecificObjsExtended, List<GameObject> behindRoomSpecificObjs, List<GameObject> aheadRoomSpecificObjs)
    {
        foreach (GameObject obj in roomSpecificObjs) ChangeComponentStateRecursively(true, obj);
        switch (multipleRoomVisible) // Make specific calls for enabling/disabling geometry based on what the multiple room visibility option is set to
        {
            case true:
                switch (productionLineAmount) // Making calls based on whether the player has maxed out a room, e.g. disable all and enable up to the amount if not maxed out
                {
                    case var value when value >= productionLineLimitExtended: // This is a workaround for switch statements expecting constant values
                        foreach (GameObject prodObj in productionLineGeos) ChangeComponentStateRecursively(true, prodObj);
                        break;
                    case var value when value < productionLineLimitExtended: // This is a workaround for switch statements expecting constant values
                        foreach (GameObject obj in productionLineGeos) ChangeComponentStateRecursively(false, obj);
                        if (productionLineAmount > productionLineThresholdExtended)
                        {
                            for (int i = 0; i < productionLineAmount - productionLineThresholdExtended; i++)
                            {
                                ChangeComponentStateRecursively(true, productionLineGeos[i]); 
                            }
                        }
                        break;
                }
                if (hasLastRoomNumberExtReq1 && lastRoomNumber == lastRoomNumberReq1)
                {
                    foreach (GameObject obj in behindRoomSpecificObjsExtended) ChangeComponentStateRecursively(false, obj);
                }
                if (hasLastRoomNumberExtReq2 && lastRoomNumber == lastRoomNumberReq2)
                {
                    foreach (GameObject obj in aheadRoomSpecificObjsExtended) ChangeComponentStateRecursively(false, obj);
                }
                break;
            case false:
                switch (productionLineAmount) // Making calls based on whether the player has maxed out a room, e.g. disable all and enable up to the amount if not maxed out
                {
                    case var value when value >= productionLineLimit:
                        foreach (GameObject prodObj in productionLineGeos) ChangeComponentStateRecursively(true, prodObj);
                        break;
                    case var value when value <= productionLineLimit:
                        foreach (GameObject obj in productionLineGeos) ChangeComponentStateRecursively(false, obj);
                        if (productionLineAmount > productionLineThreshold)
                        {
                            for (int i = 0; i < productionLineAmount - productionLineThreshold; i++)
                            {
                                ChangeComponentStateRecursively(true, productionLineGeos[i]); 
                            }
                        }
                        break;
                }
                if (hasLastRoomNumberReq1 && lastRoomNumber == lastRoomNumberReq1)
                {
                    foreach (GameObject obj in behindRoomSpecificObjs) ChangeComponentStateRecursively(false, obj);
                }
                if (hasLastRoomNumberReq2 && lastRoomNumber == lastRoomNumberReq2)
                {
                    foreach (GameObject obj in aheadRoomSpecificObjs) ChangeComponentStateRecursively(false, obj);
                }
                break;
        }
    }

    private void ChangeComponentStateRecursively(bool enable, GameObject obj)
    {
        /*if (obj.GetComponent<MeshRenderer>())
        {
            obj.GetComponent<MeshRenderer>().enabled = enable;
        }*/
        obj.SetActive(enable);

        foreach (Transform child in obj.transform)
        {
            ChangeComponentStateRecursively(enable, child.gameObject);
        }
    }
    
    private IEnumerator SwitchRoomCamera()
    {
        yield return null;
        stopCall = false;
        if (newPos != null) { cameraHolderHolder.transform.position = newPos; }
        float timeElapsed = 0;
        float curveValue = 0;
        newPos = new Vector3(initCameraPos.x + (cameraPosIncrementX * roomNumber), initCameraPos.y, initCameraPos.z);
        Vector3 oldPos = cameraHolderHolder.transform.position;
        
        while (timeElapsed < movementCurve[movementCurve.length - 1].time && !stopCall)
        {
            curveValue = movementCurve.Evaluate(timeElapsed);
            cameraHolderHolder.transform.position = Vector3.Lerp(oldPos, newPos,
                curveValue);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        cameraHolderHolder.transform.position = newPos;
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