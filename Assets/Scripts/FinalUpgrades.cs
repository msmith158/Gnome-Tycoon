using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalUpgrades : MonoBehaviour
{
    [Header("Values")]
    public UpgradeType upgradeType;
    public UpgradeCost upgradeCost;
    public float initialCost;
    [Tooltip("The rate at which the price increases in a curve.")] public float increaseRate;
    [Tooltip("How many of these upgrades the player can buy before reaching the max. Set to 0 for infinity.")] public int upgradeLimit;
    [Tooltip("The amount that the upgrade limit increases each prestige.")] [SerializeField] private int upgradeLimitIncrease;
    private float currentPrice;
    private float costPercentage;
    private int currentBuyAmount;
    private Color initialSliderColour;
    private bool hasRanOnce = false;


    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Slider slider;
    [SerializeField] private FinalFactorySystem sys;
    [SerializeField] private List<FinalConveyor> conveyors = new List<FinalConveyor>();
    [SerializeField] private List<FinalDispenser> dispensers = new List<FinalDispenser>();
    [SerializeField] private GnomeCoinSystem gnomeCoinSys;

    public void OnEnable()
    {
        switch (hasRanOnce)
        {
            case false:
                gnomeCoinSys = GameObject.Find("ddolManager").GetComponent<GnomeCoinSystem>();

                // Set the first prices for upgrades
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

                initialSliderColour = slider.transform.GetChild(0).GetComponent<Image>().color;

                if (upgradeLimit == 0)
                {
                    slider.transform.GetChild(1).gameObject.SetActive(false);
                }

                hasRanOnce = true;
                break;
            case true:
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
                    if (currentBuyAmount != upgradeLimit)
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
                                for (int i = 0; i < conveyors.Count; i++)
                                {
                                    switch (conveyors[i].isActiveAndEnabled)
                                    {
                                        case true:
                                            conveyors[i].speed += (conveyors[i].initialSpeed * percentage);
                                            Debug.Log("Conveyor speed: " + conveyors[i].speed);
                                            break;
                                        case false:
                                            break;
                                    }

                                }
                                costPercentage += increaseRate;
                                currentPrice += (initialCost * (costPercentage * 2));
                                sys.UpdatePrice(costText, false, "$", currentPrice, "");
                                break;
                            case UpgradeType.ManufactureTime:
                                for (int i = 0; i < dispensers.Count; i++)
                                {
                                    switch (dispensers[i].isActiveAndEnabled)
                                    {
                                        case true:
                                            dispensers[i].manufacturingTime -= (dispensers[i].initialManuTime * percentage);
                                            Debug.Log("Manufacturing time: " + dispensers[i].manufacturingTime);
                                            break;
                                        case false:
                                            break;
                                    }
                                }
                                costPercentage += increaseRate;
                                currentPrice += (initialCost * (costPercentage * 2));
                                sys.UpdatePrice(costText, false, "$", currentPrice, "");
                                break;
                            case UpgradeType.ProductionLines:

                                sys.productionLineAmount += (int)percentage;
                                sys.SetProductionLines();
                                costPercentage += increaseRate;
                                currentPrice += (initialCost * (costPercentage * 2));
                                sys.UpdatePrice(costText, false, "$", currentPrice, "");
                                break;
                        }
                        currentBuyAmount++;
                        if (currentBuyAmount != upgradeLimit)
                        {
                            slider.value = Mathf.InverseLerp(0f, upgradeLimit, currentBuyAmount);
                        }
                        else if (currentBuyAmount == upgradeLimit)
                        {
                            slider.value = Mathf.InverseLerp(0f, upgradeLimit, currentBuyAmount);
                            upgradeButton.interactable = false;
                            Color sliderColour = slider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color;
                            slider.transform.GetChild(1).gameObject.SetActive(false);
                            slider.transform.GetChild(0).GetComponent<Image>().color = sliderColour;
                        }
                    }
                }
                break;
            case UpgradeCost.GnomeCoins:
                if (gnomeCoinSys.coinCount >= (int)currentPrice)
                {
                    if (currentBuyAmount != upgradeLimit)
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
                                gnomeCoinSys.permanentTime -= percentage;
                                Debug.Log("Permanent manufacturing time: " + gnomeCoinSys.permanentTime);
                                costPercentage += increaseRate;
                                currentPrice += (initialCost * (costPercentage * 2));
                                sys.UpdatePrice(costText, true, "c", currentPrice, "");
                                break;
                        }

                        currentBuyAmount++;
                        if (currentBuyAmount != upgradeLimit)
                        {
                            slider.value = Mathf.InverseLerp(0f, upgradeLimit, currentBuyAmount);
                        }
                        else if (currentBuyAmount == upgradeLimit)
                        {
                            slider.value = Mathf.InverseLerp(0f, upgradeLimit, currentBuyAmount);
                            upgradeButton.interactable = false;
                            Color sliderColour = slider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color;
                            slider.transform.GetChild(1).gameObject.SetActive(false);
                            slider.transform.GetChild(0).GetComponent<Image>().color = sliderColour;
                        }
                    }
                }
                break;
        }
    }
    

    public void ResetAndAdjustPrices(float costIncrease)
    {
        currentPrice = initialCost + (initialCost * costIncrease);
        costPercentage = 0;
        sys.UpdatePrice(costText, false, "$", currentPrice, "");
        slider.value = 0;
        currentBuyAmount = 0;
        switch (sys.prestigeLvl)
        {
            case FinalFactorySystem.PrestigeLevel.Prestige1:
                upgradeLimit += (1 * upgradeLimitIncrease);
                break;
            case FinalFactorySystem.PrestigeLevel.Prestige2:
                upgradeLimit += (2 * upgradeLimitIncrease);
                break;
            case FinalFactorySystem.PrestigeLevel.Prestige3:
                upgradeLimit += (3 * upgradeLimitIncrease);
                break;
            case FinalFactorySystem.PrestigeLevel.Prestige4:
                upgradeLimit += (4 * upgradeLimitIncrease);
                break;
            case FinalFactorySystem.PrestigeLevel.Prestige5:
                upgradeLimit += (5 * upgradeLimitIncrease);
                break;
        }
        slider.transform.GetChild(1).gameObject.SetActive(true);
        slider.transform.GetChild(0).GetComponent<Image>().color = initialSliderColour;
        upgradeButton.interactable = true;
    }

    public enum UpgradeType
    {
        GnomeValue,
        ConveyorSpeed,
        ManufactureTime,
        ProductionLines
    }

    public enum UpgradeCost
    {
        Dollans,
        GnomeCoins
    }
}