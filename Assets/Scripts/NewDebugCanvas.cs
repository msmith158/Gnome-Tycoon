using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TextMeshProUGUI))]
public class NewDebugCanvas : MonoBehaviour
{
    private TextMeshProUGUI debugText;
    [SerializeField] private FinalConveyor line01Conveyor;
    [SerializeField] private FinalFactorySystem sys;
    [SerializeField] private FinalDispenser line01Dispenser;
    [SerializeField] private GnomeCoinSystem gnomeCoinSys;

    // Dictionary to store bools by their names
    private Dictionary<string, bool> boolDictionary = new Dictionary<string, bool>();

    List<string> debugList = new List<string>();

    // Example bools (you can add more)
    [Header("General Metrics")]
    public bool enableFPSCounter;
    public bool enableFrameTiming;
    public bool enableLevelName;
    public bool enableResAndAspect;
    [Header("Gnome Tycoon Specific Metrics")]
    public bool enableGnomeValueMetric;
    public bool enableConveyorSpeedMetric;
    public bool enableManufacturingTimeMetric;

    // String to modify
    private string resultString = "";

    // String dictionary
    private string framerateString = "";
    private string frameTimingString = "";
    private string levelNameString = "";
    private string resAndAspectString = "";
    private string gnomeValueString = "";
    private string conveyorSpeedString = "";
    private string manufacturingTimeString = "";

    // Framerate values
    [Header("Metrics Settings")]
    [HideInInspector] public int fpsCounter_avgFrameRate;
    [Tooltip("Adjust how often the average framerate metre updates")] public float fpsCounter_updateInterval = 0.5F;
    private double fpsCounter_lastInterval;
    private int fpsCounter_frames;
    private float fpsCounter_fps;

    private void Start()
    {
        debugText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        gnomeCoinSys = GameObject.Find("ddolManager").GetComponent<GnomeCoinSystem>();
    }

    private void Update()
    {
        CheckBools();

        // Reset dictionary per draw to allow for adding fresh stats
        boolDictionary.Clear();

        // Add the strings with their respective bools here
        boolDictionary.Add(framerateString, enableFPSCounter);
        boolDictionary.Add(frameTimingString, enableFrameTiming);
        boolDictionary.Add(levelNameString, enableLevelName);
        boolDictionary.Add(resAndAspectString, enableResAndAspect);
        boolDictionary.Add(gnomeValueString, enableGnomeValueMetric);
        boolDictionary.Add(conveyorSpeedString, enableConveyorSpeedMetric);
        boolDictionary.Add(manufacturingTimeString, enableManufacturingTimeMetric);

        // Reset list per draw to get fresh stats
        debugList.Clear();

        foreach (var kvp in boolDictionary)
        {
            if (kvp.Value)
            {
                debugList.Add(kvp.Key);
            }
        }

        // Reset string per draw to fix an overflow issue
        resultString = "";
        resultString += "/// Debug Metrics /// \n \n";

        for (int i = 0; i < debugList.Count; i++)
        {
            if (i != (debugList.Count + 1))
            {
                resultString += debugList[i] + " \n";
            }
            else
            {
                break;
            }
        }

        debugText.text = resultString;
    }

    // This big list of switch statements allows optimisation by only allowing functions that are needed to run.
    private void CheckBools()
    {
        switch (enableFPSCounter)
        {
            case true:
                Framerate();
                break;
        }

        switch (enableFrameTiming)
        {
            case true:
                FrameTiming();
                break;
        }

        switch (enableLevelName)
        {
            case true:
                LevelName();
                break;
        }

        switch (enableResAndAspect)
        {
            case true:
                ResAndAspect();
                break;
        }
        
        switch (enableGnomeValueMetric)
        {
            case true:
                GnomeValueMetric();
                break;
        }
        
        switch (enableConveyorSpeedMetric)
        {
            case true:
                ConveyorSpeedMetric();
                break;
        }
        
        switch (enableManufacturingTimeMetric)
        {
            case true:
                ManufacturingTimeMetric();
                break;
        }
    }

    private void Framerate()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        fpsCounter_avgFrameRate = (int)current;
        framerateString = "FPS: " + fpsCounter_avgFrameRate.ToString() + " (" + fpsCounter_fps.ToString() + " avg)";

        ++fpsCounter_frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > fpsCounter_lastInterval + fpsCounter_updateInterval)
        {
            fpsCounter_fps = Mathf.Ceil((float)(fpsCounter_frames / (timeNow - fpsCounter_lastInterval))); // Mathf.Ceil will round the outputted integer up to the nearest whole number
            fpsCounter_frames = 0;
            fpsCounter_lastInterval = timeNow;
        }
    }

    private void FrameTiming()
    {
        float currentFrameTiming = Mathf.Ceil(Time.deltaTime * 1000);
        frameTimingString = "Timing: " + currentFrameTiming.ToString() + " ms";
    }

    private void LevelName()
    {
        Scene scene = SceneManager.GetActiveScene();
        levelNameString = "Active scene: \"" + scene.name + "\"";
    }

    private void ResAndAspect()
    {
        float screenResW = Screen.width;
        float screenResH = Screen.height;
        float aspect = Camera.main.aspect;
        resAndAspectString = "Screen: " + screenResW.ToString() + "x" + screenResH.ToString() + ", " + aspect; 
    }

    private void GnomeValueMetric()
    {
        gnomeValueString = "";
    }

    private void ConveyorSpeedMetric()
    {
        conveyorSpeedString = "Conveyor speed: " + (line01Conveyor.speed + gnomeCoinSys.permanentSpeed) + " (" + line01Conveyor.speed + " + " + gnomeCoinSys.permanentSpeed + ")";
    }

    private void ManufacturingTimeMetric()
    {
        manufacturingTimeString = "Manufacturing time: " + line01Dispenser.manufacturingTime + " + " + gnomeCoinSys.permanentTime;
    }
}