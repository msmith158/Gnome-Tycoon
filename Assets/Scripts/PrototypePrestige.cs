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

    [Header("Object References")]
    [SerializeField] private PrototypeFactorySystem sys;
    [SerializeField] private PrototypeFinalPrestigeSystem finalPrestigeSys;
    [SerializeField] private List<PrototypeUpgrades> upgradeSys = new List<PrototypeUpgrades>();
    [SerializeField] private PrototypeManufacturer manufacturer;
    [SerializeField] private PrototypeConveyor conveyor;
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

            switch (prestigeType)
            {
                case PrestigeType.Prestige1:
                    if (sys.prestigeLvl == PrototypeFactorySystem.PrestigeLevel.Prestige0)
                    {
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige1);
                    }
                    break;
                case PrestigeType.Prestige2:
                    if (sys.prestigeLvl == PrototypeFactorySystem.PrestigeLevel.Prestige1)
                    {
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige2);
                    }
                    break;
                case PrestigeType.Prestige3:
                    if (sys.prestigeLvl == PrototypeFactorySystem.PrestigeLevel.Prestige2)
                    {
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige3);
                    }
                    break;
                case PrestigeType.Prestige4:
                    if (sys.prestigeLvl == PrototypeFactorySystem.PrestigeLevel.Prestige3)
                    {
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige4);
                    }
                    break;
                case PrestigeType.Prestige5:
                    if (sys.prestigeLvl == PrototypeFactorySystem.PrestigeLevel.Prestige4)
                    {
                        ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel.Prestige5);
                    }
                    break;
                case PrestigeType.FinalPrestige: // The nuke sequence
                    //StartCoroutine(finalPrestigeSys.NukeSequence());
                    finalPrestigeSys.SendMessage("StartNukeSequence", null);
                    break;
            }
        }
    }

    private void ChangeValuesBasedOnPrestige(PrototypeFactorySystem.PrestigeLevel newPrestLvl)
    {
        // Reset money to 0
        sys.prestigeLvl = newPrestLvl;
        sys.pointScore = 0;
        sys.moneyText.text = "Profit: $" + sys.pointScore;

        // Reset gnome values
        switch (newPrestLvl)
        {
            // Dev note: Yes, I know now that this isn't the best method to do this. I'll change it if I can.
            case PrototypeFactorySystem.PrestigeLevel.Prestige1:
                sys.lvl2InitialValue = sys.lvl1InitialValue + unitMultiplication;
                sys.lvl2Value = sys.lvl2InitialValue;
                Debug.Log("New gnome value: " + sys.lvl2Value);
                break;
            case PrototypeFactorySystem.PrestigeLevel.Prestige2:
                sys.lvl3InitialValue = sys.lvl1InitialValue + unitMultiplication;
                sys.lvl3Value = sys.lvl3InitialValue;
                Debug.Log("New gnome value: " + sys.lvl3Value);
                break;
            case PrototypeFactorySystem.PrestigeLevel.Prestige3:
                sys.lvl4InitialValue = sys.lvl1InitialValue + unitMultiplication;
                sys.lvl4Value = sys.lvl4InitialValue;
                Debug.Log("New gnome value: " + sys.lvl4Value);
                break;
            case PrototypeFactorySystem.PrestigeLevel.Prestige4:
                sys.lvl5InitialValue = sys.lvl1InitialValue + unitMultiplication;
                sys.lvl5Value = sys.lvl5InitialValue;
                Debug.Log("New gnome value: " + sys.lvl5Value);
                break;
            case PrototypeFactorySystem.PrestigeLevel.Prestige5:
                sys.lvl6InitialValue = sys.lvl1InitialValue + unitMultiplication;
                sys.lvl6Value = sys.lvl6InitialValue;
                Debug.Log("New gnome value: " + sys.lvl6Value);
                break;
        }

        // Reset conveyor values
        conveyor.speed = conveyor.initialSpeed;
        Debug.Log("Conveyor belt speed reset to " + conveyor.speed);

        // Reset manufacturing values
        manufacturer.manufacturingTime = manufacturer.initialManuTime - (manufacturer.initialManuTime * manufacturerDecreasePercent);
        Debug.Log("New manufacturing speed: " + manufacturer.manufacturingTime);

        // Reset and adjust total costs
        for (int i = 0; i < upgradeSys.Count; i++)
        {
            upgradeSys[i].SendMessage("ResetAndAdjustPrices", priceIncreasePercent);
        }

        Debug.Log("Upgraded Prestige to " + sys.prestigeLvl);
    }

    public void PromptWindow(PrototypeFactorySystem.PrestigeLevel currentPrestige)
    {
        switch (currentPrestige)
        {

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
