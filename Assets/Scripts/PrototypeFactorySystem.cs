using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private void Start()
    {
        lvl1InitialValue = lvl1Value;
        Application.targetFrameRate = 60;
        AddScore(0);
    }

    public void AddScore(float amount)
    {
        pointScore = pointScore + amount;
        if (debugMode && !hasDebugRun) 
        {
            pointScore += instantPointAddition;
            hasDebugRun = true;
        }
        moneyText.text = "Profit: $" + RoundToNearestHundredth(pointScore).ToString("F2");
    }

    public void UpdatePrice(TextMeshProUGUI costText, string beforeText, float newPrice, string afterText)
    {
        costText.text = beforeText + RoundToNearestHundredth(newPrice).ToString("F2") + afterText;
    }

    public float RoundToNearestHundredth(float value)
    {
        return (float)System.Math.Round(value, 2);
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