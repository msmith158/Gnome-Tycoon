using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrototypePrestige : MonoBehaviour
{
    [Header("General Values")]
    public PrestigeType prestigeType;
    public float price;
    [Header("Reward Values")]
    public float unitMultiplication;
    [Tooltip("Write the percentage as a decimal, e.g. 10% is 0.10")] public float manufacturerDecreasePercent;
    [Tooltip("Write the percentage as a decimal, e.g. 10% is 0.10")] public float priceIncreasePercent;
    private int displayablePrestigeLevel = 0;

    [Header("Object References")]
    [SerializeField] private PrototypeFactorySystem sys;
    [SerializeField] private PrototypeFinalPrestigeSystem finalPrestigeSys;
    [SerializeField] private List<PrototypeUpgrades> upgradeSys = new List<PrototypeUpgrades>();
    [SerializeField] private PrototypeManufacturer manufacturer;
    [SerializeField] private PrototypeConveyor conveyor;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI currentPrestigeText;
    [SerializeField] private TextMeshProUGUI prestigeBodyText;
    [SerializeField] private GameObject promptParent;
    [SerializeField] private List<GameObject> promptUIToDisable = new List<GameObject>();
    [SerializeField] private List<GameObject> PromptUIToEnable = new List<GameObject>();

    void Start()
    {
        sys.UpdatePrice(costText, false, "$", price, "");
        currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
        if (prestigeType != PrestigeType.FinalPrestige)
        {
            // Example: 2x gnome value, 95% manufacturing speed & 110% prices.
            prestigeBodyText.text = unitMultiplication + "x gnome value, " + (100 - (manufacturerDecreasePercent * 100)) + "% manufacturing speed & " + (100 + (priceIncreasePercent * 100)) + "% prices.";
        }
            
    }

    public void UpdatePrestige()
    {
        if (sys.pointScore >= price)
        {
            switch (prestigeType)
            {
                case PrestigeType.Prestige1:
                    if (sys.prestigeLvl == PrototypeFactorySystem.PrestigeLevel.Prestige0 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige1);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige1);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.Prestige2:
                    if (sys.prestigeLvl == PrototypeFactorySystem.PrestigeLevel.Prestige1 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige2);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige2);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.Prestige3:
                    if (sys.prestigeLvl == PrototypeFactorySystem.PrestigeLevel.Prestige2 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige3);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige3);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.Prestige4:
                    if (sys.prestigeLvl == PrototypeFactorySystem.PrestigeLevel.Prestige3 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige4);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige4);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.Prestige5:
                    if (sys.prestigeLvl == PrototypeFactorySystem.PrestigeLevel.Prestige4 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige5);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige5);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.FinalPrestige: // The nuke sequence
                    if (sys.prestigeLvl == PrototypeFactorySystem.PrestigeLevel.Prestige5 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        finalPrestigeSys.SendMessage("StartNukeSequence", null);
                    }
                    else if (sys.debugMode)
                    {
                        Debug.Log("Hi");
                        finalPrestigeSys.SendMessage("StartNukeSequence", null);
                    }
                    else PromptWindow();
                    break;
            }
        }
    }

    private void ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel newPrestLvl)
    {
        // Reset money to 0
        sys.prestigeLvl = newPrestLvl;
        switch (sys.debugMode)
        {
            case true:
                break;
            case false:
                sys.pointScore = 0;
                break;
        }
        sys.moneyText.text = "Profit: $" + sys.pointScore;

        // Reset gnome values (and update current prestige text)
        switch (newPrestLvl)
        {
            // Dev note: Yes, I know now that this isn't the best method to do this. I'll change it if I can.
            case PrototypeFactorySystem.PrestigeLevel.Prestige0:
                displayablePrestigeLevel = 0;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case PrototypeFactorySystem.PrestigeLevel.Prestige1:
                sys.lvl2InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl2Value = sys.lvl2InitialValue;
                Debug.Log("New gnome value: " + sys.lvl2Value);
                displayablePrestigeLevel = 1;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case PrototypeFactorySystem.PrestigeLevel.Prestige2:
                sys.lvl3InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl3Value = sys.lvl3InitialValue;
                Debug.Log("New gnome value: " + sys.lvl3Value);
                displayablePrestigeLevel = 2;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case PrototypeFactorySystem.PrestigeLevel.Prestige3:
                sys.lvl4InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl4Value = sys.lvl4InitialValue;
                Debug.Log("New gnome value: " + sys.lvl4Value);
                displayablePrestigeLevel = 3;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case PrototypeFactorySystem.PrestigeLevel.Prestige4:
                sys.lvl5InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl5Value = sys.lvl5InitialValue;
                Debug.Log("New gnome value: " + sys.lvl5Value);
                displayablePrestigeLevel = 4;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case PrototypeFactorySystem.PrestigeLevel.Prestige5:
                sys.lvl6InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl6Value = sys.lvl6InitialValue;
                Debug.Log("New gnome value: " + sys.lvl6Value);
                displayablePrestigeLevel = 5;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
        }

        // Reset conveyor values
        conveyor.speed = conveyor.initialSpeed;
        Debug.Log("Conveyor belt speed reset to " + conveyor.speed);

        // Reset manufacturing values
        manufacturer.manufacturingTime = manufacturer.initialManuTime + (manufacturer.initialManuTime * manufacturerDecreasePercent);
        Debug.Log("New manufacturing speed: " + manufacturer.manufacturingTime);

        // Reset and adjust total costs
        for (int i = 0; i < upgradeSys.Count; i++)
        {
            upgradeSys[i].SendMessage("ResetAndAdjustPrices", priceIncreasePercent);
        }

        for (int i = 0; i < manufacturer.objectsList.Count; i++)
        {
            Destroy(manufacturer.objectsList[i]);
        }

        Debug.Log("Upgraded Prestige to " + sys.prestigeLvl);
    }

    private void PromptWindow()
    {
        promptParent.SetActive(true);
        for (int i = 0; i < promptUIToDisable.Count; i++)
        {
            promptUIToDisable[i].SetActive(false);
        }
        for (int i = 0; i < PromptUIToEnable.Count; i++)
        {
            PromptUIToEnable[i].SetActive(true);
        }
    }

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
