using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalUpgradeSystem : MonoBehaviour
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
    [SerializeField] private FinalFactorySystem sys;
    [SerializeField] private List<FinalConveyor> conveyors = new List<FinalConveyor>();
    [SerializeField] private List<FinalDispenser> dispensers = new List<FinalDispenser>();
    [SerializeField] private PrototypeGnomeCoinSystem gnomeCoinSys;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnEnable()
    {
        gnomeCoinSys = GameObject.Find("ddolManager").GetComponent<PrototypeGnomeCoinSystem>();
        currentPrice = initialCost;
        switch (upgradeCost)
        {
            case UpgradeCost.Dollans:
                sys.UpdatePrice(costText, false, "$", currentPrice, "");
                break;
            case UpgradeCost.GnomeCoins:
                sys.UpdatePrice(costText, true, "c", currentPrice, "");
                break;
        }
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
                                case FinalFactorySystem.PrestigeLevel.Prestige0:
                                    sys.lvl1Value += (sys.lvl1InitialValue * percentage);
                                    Debug.Log("Gnome value: " + sys.lvl1Value);
                                    costPercentage += increaseRate;
                                    currentPrice += (initialCost * (costPercentage * 2));
                                    sys.UpdatePrice(costText, false, "$", currentPrice, "");
                                    break;
                                // These will need to be tested as to whether to use each initial value or lvl1InitialValue across the board
                                case FinalFactorySystem.PrestigeLevel.Prestige1:
                                    sys.lvl2Value += (sys.lvl2InitialValue * percentage);
                                    Debug.Log("Gnome value: " + sys.lvl2Value);
                                    costPercentage += increaseRate;
                                    currentPrice += (initialCost * (costPercentage * 2));
                                    sys.UpdatePrice(costText, false, "$", sys.lvl2Value, "");
                                    break;
                                case FinalFactorySystem.PrestigeLevel.Prestige2:
                                    sys.lvl3Value += (sys.lvl3InitialValue * percentage);
                                    Debug.Log("Gnome value: " + sys.lvl3Value);
                                    costPercentage += increaseRate;
                                    currentPrice += (initialCost * (costPercentage * 2));
                                    sys.UpdatePrice(costText, false, "$", sys.lvl3Value, "");
                                    break;
                                case FinalFactorySystem.PrestigeLevel.Prestige3:
                                    sys.lvl4Value += (sys.lvl4InitialValue * percentage);
                                    Debug.Log("Gnome value: " + sys.lvl4Value);
                                    costPercentage += increaseRate;
                                    currentPrice += (initialCost * (costPercentage * 2));
                                    sys.UpdatePrice(costText, false, "$", sys.lvl4Value, "");
                                    break;
                                case FinalFactorySystem.PrestigeLevel.Prestige4:
                                    sys.lvl5Value += (sys.lvl5InitialValue * percentage);
                                    Debug.Log("Gnome value: " + sys.lvl5Value);
                                    costPercentage += increaseRate;
                                    currentPrice += (initialCost * (costPercentage * 2));
                                    sys.UpdatePrice(costText, false, "$", sys.lvl5Value, "");
                                    break;
                                case FinalFactorySystem.PrestigeLevel.Prestige5:
                                    sys.lvl6Value += (sys.lvl6InitialValue * percentage);
                                    Debug.Log("Gnome value: " + sys.lvl6Value);
                                    costPercentage += increaseRate;
                                    currentPrice += (initialCost * (costPercentage * 2));
                                    sys.UpdatePrice(costText, false, "$", sys.lvl6Value, "");
                                    break;
                            }
                            break;
                        case UpgradeType.ConveyorSpeed:
                            for(int i = 0; i < conveyors.Count; i++)
                            {
                                switch (conveyors[i].isActiveAndEnabled)
                                {
                                    case true:
                                        conveyors[i].speed += (conveyors[i].initialSpeed * percentage);
                                        Debug.Log("Conveyor speed: " + conveyors[i].speed);
                                        costPercentage += increaseRate;
                                        currentPrice += (initialCost * (costPercentage * 2));
                                        sys.UpdatePrice(costText, false, "$", currentPrice, "");
                                        break;
                                    case false:
                                        break;
                                }

                            }
                            break;
                        case UpgradeType.ManufactureTime:
                            for(int i = 0; i < dispensers.Count; i++)
                            {
                                switch (dispensers[i].isActiveAndEnabled)
                                {
                                    case true:
                                        dispensers[i].manufacturingTime -= (dispensers[i].initialManuTime * percentage);
                                        Debug.Log("Manufacturing time: " + dispensers[i].manufacturingTime);
                                        costPercentage += increaseRate;
                                        currentPrice += (initialCost * (costPercentage * 2));
                                        sys.UpdatePrice(costText, false, "$", currentPrice, "");
                                        break;
                                    case false:
                                        break;
                                }
                            }
                            break;
                    }
                }
                break;
            case UpgradeCost.GnomeCoins:
                if (gnomeCoinSys.coinCount >= (int)currentPrice)
                {
                    gnomeCoinSys.coinCount -= (int)currentPrice;
                    sys.UpdatePrice(gnomeCoinSys.gnomeCoinText, true, "c", gnomeCoinSys.coinCount, "");

                    switch (upgradeType)
                    {
                        case UpgradeType.GnomeValue:
                            gnomeCoinSys.permanentValue += percentage;
                            Debug.Log("Permanent gnome value: " + gnomeCoinSys.permanentValue);
                            costPercentage += increaseRate;
                            currentPrice += (initialCost * (costPercentage * 2));
                            sys.UpdatePrice(costText, true, "c", currentPrice, "");
                            break;
                        case UpgradeType.ConveyorSpeed:
                            gnomeCoinSys.permanentSpeed += percentage;
                            Debug.Log("Permanent conveyor speed: " + gnomeCoinSys.permanentSpeed);
                            costPercentage += increaseRate;
                            currentPrice += (initialCost * (costPercentage * 2));
                            sys.UpdatePrice(costText, true, "c", currentPrice, "");
                            break;
                        case UpgradeType.ManufactureTime:
                            gnomeCoinSys.permanentTime += percentage;
                            Debug.Log("Permanent manufacturing time: " + gnomeCoinSys.permanentTime);
                            costPercentage += increaseRate;
                            currentPrice += (initialCost * (costPercentage * 2));
                            sys.UpdatePrice(costText, true, "c", currentPrice, "");
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