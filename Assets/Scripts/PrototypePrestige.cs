using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PrototypePrestige : MonoBehaviour
{
    [Header("General Values")]
    public PrestigeType prestigeType;
    public float price;
    [Header("Reward Values")]
    public float unitMultiplication;
    [Tooltip("Write the percentage as a decimal, e.g. 10% is 0.10")] public float manufacturerDecreasePercent;
    [Tooltip("Write the percentage as a decimal, e.g. 10% is 0.10")] public float priceIncreasePercent;

    [Header("Object References")]
    [SerializeField] private PrototypeFactorySystem sys;
    [SerializeField] private PrototypeFinalPrestigeSystem finalPrestigeSys;
    [SerializeField] private PrototypeManufacturer manufacturer;
    [SerializeField] private TextMeshProUGUI costText;

    void Start()
    {
        sys.UpdatePrice(costText, "$", price, "");
    }

    public void UpdatePrestige()
    {
        if (sys.pointScore >= price)
        {
            sys.pointScore -= price;
            sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
            Debug.Log("Hello");

            switch (prestigeType)
            {
                case PrestigeType.Prestige1:
                    sys.prestigeLvl = PrototypeFactorySystem.PrestigeLevel.Prestige1;
                    sys.pointScore = 0;
                    sys.moneyText.text = "Profit: $" + sys.pointScore;
                    Debug.Log("Upgraded Prestige to " + sys.prestigeLvl);
                    break;
                case PrestigeType.Prestige2:
                    break;
                case PrestigeType.Prestige3:
                    break;
                case PrestigeType.Prestige4:
                    break;
                case PrestigeType.Prestige5:
                    break;
                case PrestigeType.FinalPrestige: // The nuke sequence
                    //StartCoroutine(finalPrestigeSys.NukeSequence());
                    finalPrestigeSys.SendMessage("StartNukeSequence", null);
                    break;
            }
        }
    }

    /*private void ForTimer(float condition, float delay)
    {
        if (condition < delay)
        {
            print("ERROR: Delay larger than time");
            return;
        }
        for (float i = 0; i < condition; i += Time.deltaTime)
        {
            if (i <= delay)
            {
                print("bye");
                continue;
            }
            else
            {
                print("Hi");
            }
        }
    }*/

    public enum PrestigeType
    {
        Prestige1,
        Prestige2,
        Prestige3,
        Prestige4,
        Prestige5,
        FinalPrestige
    }
}
