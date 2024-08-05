using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrestigeSystem : MonoBehaviour
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
    [SerializeField] private FinalFactorySystem sys;
    [SerializeField] private PrestigeSequenceSystem finalPrestigeSys;
    [SerializeField] private List<FinalUpgrades> upgradeSys = new List<FinalUpgrades>();
    [SerializeField] private List<FinalDispenser> dispensers = new List<FinalDispenser>();
    [SerializeField] private List<FinalConveyor> conveyors = new List<FinalConveyor>();
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI currentPrestigeText;
    [SerializeField] private TextMeshProUGUI prestigeBodyText;
    [SerializeField] private GameObject promptParent;
    [SerializeField] private List<GameObject> promptUIToDisable = new List<GameObject>();
    [SerializeField] private List<GameObject> PromptUIToEnable = new List<GameObject>();

    void OnEnable()
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
                    if (sys.prestigeLvl == FinalFactorySystem.PrestigeLevel.Prestige0 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige1);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige1);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.Prestige2:
                    if (sys.prestigeLvl == FinalFactorySystem.PrestigeLevel.Prestige1 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige2);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige2);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.Prestige3:
                    if (sys.prestigeLvl == FinalFactorySystem.PrestigeLevel.Prestige2 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige3);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige3);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.Prestige4:
                    if (sys.prestigeLvl == FinalFactorySystem.PrestigeLevel.Prestige3 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige4);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige4);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.Prestige5:
                    if (sys.prestigeLvl == FinalFactorySystem.PrestigeLevel.Prestige4 && !sys.debugMode)
                    {
                        sys.pointScore -= price;
                        sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige5);
                    }
                    else if (sys.debugMode)
                    {
                        ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel.Prestige5);
                    }
                    else PromptWindow();
                    break;
                case PrestigeType.FinalPrestige: // The nuke sequence
                    if (sys.prestigeLvl == FinalFactorySystem.PrestigeLevel.Prestige5 && !sys.debugMode)
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

    private void ChangeValuesBasedOnPrestige(FinalFactorySystem.PrestigeLevel newPrestLvl)
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
        sys.moneyText.text = "Profit: $" + sys.pointScore.ToString("F2");

        // Reset gnome values (and update current prestige text)
        switch (newPrestLvl)
        {
            case FinalFactorySystem.PrestigeLevel.Prestige0:
                displayablePrestigeLevel = 0;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case FinalFactorySystem.PrestigeLevel.Prestige1:
                sys.lvl2InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl2Value = sys.lvl2InitialValue;
                Debug.Log("New gnome value: " + sys.lvl2Value);
                displayablePrestigeLevel = 1;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case FinalFactorySystem.PrestigeLevel.Prestige2:
                sys.lvl3InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl3Value = sys.lvl3InitialValue;
                Debug.Log("New gnome value: " + sys.lvl3Value);
                displayablePrestigeLevel = 2;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case FinalFactorySystem.PrestigeLevel.Prestige3:
                sys.lvl4InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl4Value = sys.lvl4InitialValue;
                Debug.Log("New gnome value: " + sys.lvl4Value);
                displayablePrestigeLevel = 3;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case FinalFactorySystem.PrestigeLevel.Prestige4:
                sys.lvl5InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl5Value = sys.lvl5InitialValue;
                Debug.Log("New gnome value: " + sys.lvl5Value);
                displayablePrestigeLevel = 4;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
            case FinalFactorySystem.PrestigeLevel.Prestige5:
                sys.lvl6InitialValue = sys.lvl1InitialValue * unitMultiplication;
                sys.lvl6Value = sys.lvl6InitialValue;
                Debug.Log("New gnome value: " + sys.lvl6Value);
                displayablePrestigeLevel = 5;
                currentPrestigeText.text = "Current Prestige: " + displayablePrestigeLevel;
                break;
        }

        // Reset conveyor values
        for (int i = 0; i < conveyors.Count; i++)
        {
            switch (conveyors[i].isActiveAndEnabled)
            {
                case true: 
                    conveyors[i].speed = conveyors[i].initialSpeed;
                    break;
                case false:
                    break;
            }
        }

        // Reset manufacturing values
        for (int i = 0; i < dispensers.Count; i++)
        {
            switch (dispensers[i].isActiveAndEnabled)
            {
                case true:
                    dispensers[i].manufacturingTime = dispensers[i].initialManuTime + (dispensers[i].initialManuTime * manufacturerDecreasePercent);
                    break;
                case false:
                    break;
            }
        }

        // Reset and adjust total costs
        for (int i = 0; i < upgradeSys.Count; i++)
        {
            upgradeSys[i].SendMessage("ResetAndAdjustPrices", priceIncreasePercent);
        }

        for (int i = 0; i < dispensers.Count; i++)
        {
            switch (dispensers[i].isActiveAndEnabled)
            {
                case true:
                    for (int j = 0; j < dispensers[i].objectsList.Count; j++)
                    {
                        Destroy(dispensers[i].objectsList[j]);
                    }
                    break;
                case false:
                    break;
            }
        }

        StartCoroutine(finalPrestigeSys.DoPrestigePhase());

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
