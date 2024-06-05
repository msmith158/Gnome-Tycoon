using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PrototypeFactory;

public class PrototypeFactorySettings : MonoBehaviour
{
    [Header("Values")]
    public int pointScore;
    [Header("Object References")]
    public Collider basketTrigger;
    public TextMeshProUGUI moneyText;

    public void AddScore(int amount)
    {
        pointScore = pointScore + amount;
        moneyText.text = "Profit: $" + pointScore.ToString();
    }


}