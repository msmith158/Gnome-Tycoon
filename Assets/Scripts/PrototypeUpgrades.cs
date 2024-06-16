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
    [SerializeField] private PrototypeConveyor conveyor;
    [SerializeField] private PrototypeManufacturer manufacturer;

    // Start is called before the first frame update
    void Start()
    {
        currentPrice = initialCost;
        sys.UpdatePrice(costText, "$", currentPrice, "");
    }

    public void SetNewValues(float percentage)
    {
        if (sys.pointScore >= currentPrice)
        {
            sys.pointScore -= currentPrice;
            sys.UpdatePrice(sys.moneyText, "Profit: $", sys.pointScore, "");

            switch (upgradeType)
            {
                case UpgradeType.GnomeValue:
                    switch (sys.prestigeLvl)
                    {
                        case PrototypeFactorySystem.PrestigeLevel.Prestige0:
                            sys.lvl1Value += (sys.lvl1InitialValue * percentage);
                            Debug.Log("Gnome value: " + sys.lvl1Value);
                            costPercentage += increaseRate;
                            currentPrice += (initialCost * (costPercentage * 2));
                            sys.UpdatePrice(costText, "$", currentPrice, "");
                            break;
                        // These will need to be tested as to whether to use each initial value or lvl1InitialValue across the board
                        case PrototypeFactorySystem.PrestigeLevel.Prestige1:
                            sys.lvl2Value += (sys.lvl2InitialValue * percentage);
                            Debug.Log("Gnome value: " + sys.lvl2Value);
                            costPercentage += increaseRate;
                            currentPrice += (initialCost * (costPercentage * 2));
                            sys.UpdatePrice(costText, "$", sys.lvl2Value, "");
                            break;
                        case PrototypeFactorySystem.PrestigeLevel.Prestige2:
                            sys.lvl3Value += (sys.lvl3InitialValue * percentage);
                            Debug.Log("Gnome value: " + sys.lvl3Value);
                            costPercentage += increaseRate;
                            currentPrice += (initialCost * (costPercentage * 2));
                            sys.UpdatePrice(costText, "$", sys.lvl3Value, "");
                            break;
                        case PrototypeFactorySystem.PrestigeLevel.Prestige3:
                            sys.lvl4Value += (sys.lvl4InitialValue * percentage);
                            Debug.Log("Gnome value: " + sys.lvl4Value);
                            costPercentage += increaseRate;
                            currentPrice += (initialCost * (costPercentage * 2));
                            sys.UpdatePrice(costText, "$", sys.lvl4Value, "");
                            break;
                        case PrototypeFactorySystem.PrestigeLevel.Prestige4:
                            sys.lvl5Value += (sys.lvl5InitialValue * percentage);
                            Debug.Log("Gnome value: " + sys.lvl5Value);
                            costPercentage += increaseRate;
                            currentPrice += (initialCost * (costPercentage * 2));
                            sys.UpdatePrice(costText, "$", sys.lvl5Value, "");
                            break;
                        case PrototypeFactorySystem.PrestigeLevel.Prestige5:
                            sys.lvl6Value += (sys.lvl6InitialValue * percentage);
                            Debug.Log("Gnome value: " + sys.lvl6Value);
                            costPercentage += increaseRate;
                            currentPrice += (initialCost * (costPercentage * 2));
                            sys.UpdatePrice(costText, "$", sys.lvl6Value, "");
                            break;
                    }
                    break;
                case UpgradeType.ConveyorSpeed:
                    conveyor.speed += (conveyor.initialSpeed * percentage);
                    Debug.Log("Conveyor speed: " + conveyor.speed);
                    costPercentage += increaseRate;
                    currentPrice += (initialCost * (costPercentage * 2));
                    sys.UpdatePrice(costText, "$", currentPrice, "");
                    break;
                case UpgradeType.ManufactureTime:
                    manufacturer.manufacturingTime -= (manufacturer.initialManuTime * percentage);
                    Debug.Log("Manufacturing time: " + manufacturer.manufacturingTime);
                    costPercentage += increaseRate;
                    currentPrice += (initialCost * (costPercentage * 2));
                    sys.UpdatePrice(costText, "$", currentPrice, "");
                    break;
            }
        }
    }

    public void ResetAndAdjustPrices(float costIncrease)
    {
        currentPrice = initialCost + (initialCost * costIncrease);
        sys.UpdatePrice(costText, "$", currentPrice, "");
    }

    public enum UpgradeType
    {
        GnomeValue,
        ConveyorSpeed,
        ManufactureTime
    }
}
