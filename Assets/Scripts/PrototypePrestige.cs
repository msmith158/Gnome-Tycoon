using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using TMPro;
using UnityEngine;

public class PrototypePrestige : MonoBehaviour
{
    [Header("Values: General")]
    public PrestigeType prestigeType;
    public float price;

    [Header("Object References")]
    [SerializeField] private PrototypeFactorySystem sys;
    [SerializeField] private PrototypeFinalPrestigeSystem finalPrestigeSys;

    public void PrestigeEvents()
    {
        if (sys.pointScore >= price)
        {
            sys.pointScore -= price;
            sys.moneyText.text = "Profit: $" + sys.RoundToNearestHundredth(sys.pointScore).ToString("F2");

            switch (prestigeType)
            {
                case PrestigeType.Prestige1:
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
