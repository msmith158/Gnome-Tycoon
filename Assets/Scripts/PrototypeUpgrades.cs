using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrototypeUpgrades : MonoBehaviour
{
    [Header("Values")]
    public UpgradeType upgradeType;
    public float initialCost;
    public float increaseRate;
    private float currentPrice;
    private float costPercentage;


    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private PrototypeFactorySystem sys;

    // Start is called before the first frame update
    void Start()
    {
        currentPrice = initialCost;
        UpdatePrice(currentPrice);
    }

    public void SetNewValues(float percentage)
    {
        if (sys.pointScore >= currentPrice)
        {
            sys.pointScore -= currentPrice;
            sys.moneyText.text = "Profit: $" + RoundToNearestHundredth(sys.pointScore).ToString("F2");

            switch (upgradeType)
            {
                case UpgradeType.GnomeValue:
                    switch (sys.prestigeLvl)
                    {
                        case PrototypeFactorySystem.PrestigeLevel.Prestige1:
                            sys.lvl1Value += (sys.lvl1InitialValue * percentage);
                            Debug.Log(sys.lvl1Value);
                            costPercentage += increaseRate;
                            currentPrice += (initialCost * (costPercentage * 2));
                            Debug.Log(currentPrice);
                            UpdatePrice(currentPrice);
                            break;
                        // ADD THESE WHEN PRESTIGE SYSTEM IS SET UP
                        /*case PrototypeFactorySystem.PrestigeLevel.Prestige2:
                            sys.lvl2Value += (sys.lvl2InitialValue * percentage);
                            Debug.Log(sys.lvl2Value);
                            UpdatePrice(sys.lvl2Value);
                            break;
                        case PrototypeFactorySystem.PrestigeLevel.Prestige3:
                            sys.lvl3Value += (sys.lvl3InitialValue * percentage);
                            Debug.Log(sys.lvl3Value);
                            UpdatePrice(sys.lvl3Value);
                            break;
                        case PrototypeFactorySystem.PrestigeLevel.Prestige4:
                            sys.lvl4Value += (sys.lvl4InitialValue * percentage);
                            Debug.Log(sys.lvl4Value);
                            UpdatePrice(sys.lvl4Value);
                            break;
                        case PrototypeFactorySystem.PrestigeLevel.Prestige5:
                            sys.lvl5Value += (sys.lvl5InitialValue * percentage);
                            Debug.Log(sys.lvl5Value);
                            UpdatePrice(sys.lvl5Value);
                            break;
                        case PrototypeFactorySystem.PrestigeLevel.Prestige6:
                            sys.lvl6Value += (sys.lvl6InitialValue * percentage);
                            Debug.Log(sys.lvl6Value);
                            UpdatePrice(sys.lvl6Value);
                            break;*/
                    }
                    break;
                case UpgradeType.ConveyorSpeed:
                    break;
                case UpgradeType.ManufactureTime:
                    break;
            }
        }
    }

    void UpdatePrice(float newPrice)
    {
        costText.text = "$" + RoundToNearestHundredth(newPrice).ToString("F2");
    }

    float RoundToNearestHundredth(float value)
    {
        return (float)System.Math.Round(value, 2);
    }

    public enum UpgradeType
    {
        GnomeValue,
        ConveyorSpeed,
        ManufactureTime
    }
}
