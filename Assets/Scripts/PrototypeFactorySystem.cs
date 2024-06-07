using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PrototypeFactory;

public class PrototypeFactorySystem : MonoBehaviour
{
    [Header("Values")]
    public float pointScore;
    public PrestigeLevel prestigeLvl;
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

    [Header("Object References")]
    public Collider basketTrigger;
    public TextMeshProUGUI moneyText;

    private void Start()
    {
        lvl1InitialValue = lvl1Value;
        lvl2InitialValue = lvl2Value;
        lvl3InitialValue = lvl3Value;
        lvl4InitialValue = lvl4Value;
        lvl5InitialValue = lvl5Value;
        lvl6InitialValue = lvl6Value;
        Debug.Log(lvl1InitialValue);
    }

    public void AddScore(float amount)
    {
        pointScore = pointScore + amount;
        moneyText.text = "Profit: $" + RoundToNearestHundredth(pointScore).ToString("F2");
    }

    float RoundToNearestHundredth(float value)
    {
        return (float)System.Math.Round(value, 2);
    }

    public enum PrestigeLevel
    {
        Prestige1,
        Prestige2,
        Prestige3,
        Prestige4,
        Prestige5,
        Prestige6
    }
}