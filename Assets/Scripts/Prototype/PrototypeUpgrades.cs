using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrototypeUpgrades : MonoBehaviour
{
    [Header("Values")]
    public UpgradeType upgradeType;
    public UpgradeCost upgradeCost;
    public float initialCost;
    [Tooltip("The rate at which the price increases in a curve.")] public float increaseRate;
    [Tooltip("How many of these upgrades the player can buy before reaching the max. Set to 0 for infinity.")] public int upgradeLimit;
    private float currentPrice;
    private float costPercentage;


    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private PrototypeFactorySystem sys;
    [SerializeField] private PrototypeConveyor conveyor;
    [SerializeField] private PrototypeManufacturer manufacturer;
    [SerializeField] private PrototypeGnomeCoinSystem gnomeCoinSys;

    // Start is called before the first frame update
    void Start()
    {
        currentPrice = initialCost;
        switch (upgradeCost)
        {
            case UpgradeCost.Dollans:
                sys.UpdatePrice(costText, false, "$", currentPrice, "");
                break;
            case UpgradeCost.GnomeCoins:
                sys.UpdatePrice(costText, true, "¢", currentPrice, "");
                break;
        }
    }

    public void OnEnable()
    {
        gnomeCoinSys = GameObject.Find("proto_ddolManager").GetComponent<PrototypeGnomeCoinSystem>();
    }

    public void SetNewValues(float percentage)
    {
        switch (upgradeCost)
        {
            case UpgradeCost.Dollans:
                if (sys.pointScore >= currentPrice)
                {
                    sys.pointScore -= currentPrice;
                    sys.UpdatePrice(sys.moneyText, false, "Profit: $", sys.pointScore, "");

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
                                    sys.UpdatePrice(costText, false, "$", currentPrice, "");
                                    break;
                                // These will need to be tested as to whether to use each initial value or lvl1InitialValue across the board
                                case PrototypeFactorySystem.PrestigeLevel.Prestige1:
                                    sys.lvl2Value += (sys.lvl2InitialValue * percentage);
                                    Debug.Log("Gnome value: " + sys.lvl2Value);
                                    costPercentage += increaseRate;
                                    currentPrice += (initialCost * (costPercentage * 2));
                                    sys.UpdatePrice(costText, false, "$", sys.lvl2Value, "");
                                    break;
                                case PrototypeFactorySystem.PrestigeLevel.Prestige2:
                                    sys.lvl3Value += (sys.lvl3InitialValue * percentage);
                                    Debug.Log("Gnome value: " + sys.lvl3Value);
                                    costPercentage += increaseRate;
                                    currentPrice += (initialCost * (costPercentage * 2));
                                    sys.UpdatePrice(costText, false, "$", sys.lvl3Value, "");
                                    break;
                                case PrototypeFactorySystem.PrestigeLevel.Prestige3:
                                    sys.lvl4Value += (sys.lvl4InitialValue * percentage);
                                    Debug.Log("Gnome value: " + sys.lvl4Value);
                                    costPercentage += increaseRate;
                                    currentPrice += (initialCost * (costPercentage * 2));
                                    sys.UpdatePrice(costText, false, "$", sys.lvl4Value, "");
                                    break;
                                case PrototypeFactorySystem.PrestigeLevel.Prestige4:
                                    sys.lvl5Value += (sys.lvl5InitialValue * percentage);
                                    Debug.Log("Gnome value: " + sys.lvl5Value);
                                    costPercentage += increaseRate;
                                    currentPrice += (initialCost * (costPercentage * 2));
                                    sys.UpdatePrice(costText, false, "$", sys.lvl5Value, "");
                                    break;
                                case PrototypeFactorySystem.PrestigeLevel.Prestige5:
                                    sys.lvl6Value += (sys.lvl6InitialValue * percentage);
                                    Debug.Log("Gnome value: " + sys.lvl6Value);
                                    costPercentage += increaseRate;
                                    currentPrice += (initialCost * (costPercentage * 2));
                                    sys.UpdatePrice(costText, false, "$", sys.lvl6Value, "");
                                    break;
                            }
                            break;
                        case UpgradeType.ConveyorSpeed:
                            conveyor.speed += (conveyor.initialSpeed * percentage);
                            Debug.Log("Conveyor speed: " + conveyor.speed);
                            costPercentage += increaseRate;
                            currentPrice += (initialCost * (costPercentage * 2));
                            sys.UpdatePrice(costText, false, "$", currentPrice, "");
                            break;
                        case UpgradeType.ManufactureTime:
                            manufacturer.manufacturingTime -= (manufacturer.initialManuTime * percentage);
                            Debug.Log("Manufacturing time: " + manufacturer.manufacturingTime);
                            costPercentage += increaseRate;
                            currentPrice += (initialCost * (costPercentage * 2));
                            sys.UpdatePrice(costText, false, "$", currentPrice, "");
                            break;
                    }
                }
                break;
            case UpgradeCost.GnomeCoins:
                if (gnomeCoinSys.coinCount >= (int)currentPrice)
                {
                    gnomeCoinSys.coinCount -= (int)currentPrice;
                    sys.UpdatePrice(gnomeCoinSys.gnomeCoinText, true, "¢", gnomeCoinSys.coinCount, "");

                    switch (upgradeType)
                    {
                        case UpgradeType.GnomeValue:
                            gnomeCoinSys.permanentValue += percentage;
                            Debug.Log("Permanent gnome value: " + gnomeCoinSys.permanentValue);
                            costPercentage += increaseRate;
                            currentPrice += (initialCost * (costPercentage * 2));
                            sys.UpdatePrice(costText, true, "¢", currentPrice, "");
                            break;
                        case UpgradeType.ConveyorSpeed:
                            conveyor.speed += (conveyor.initialSpeed * percentage);
                            Debug.Log("Conveyor speed: " + conveyor.speed);
                            costPercentage += increaseRate;
                            currentPrice += (initialCost * (costPercentage * 2));
                            sys.UpdatePrice(costText, true, "$", currentPrice, "");
                            break;
                        case UpgradeType.ManufactureTime:
                            // Code here
                            break;
                    }
                }
                break;
        }
    }
    

    public void ResetAndAdjustPrices(float costIncrease)
    {
        currentPrice = initialCost + (initialCost * costIncrease);
        sys.UpdatePrice(costText, false, "$", currentPrice, "");
    }

    public enum UpgradeType
    {
        GnomeValue,
        ConveyorSpeed,
        ManufactureTime
    }

    public enum UpgradeCost
    {
        Dollans,
        GnomeCoins
    }
}
