using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PrototypeFactory;

public class PrototypeFactorySystem : MonoBehaviour
{
    [Header("Values")]
    public float pointScore;
    [Header("Object References")]
    public Collider basketTrigger;
    public TextMeshProUGUI moneyText;

    public void AddScore(float amount)
    {
        pointScore = pointScore + amount;
        moneyText.text = "Profit: $" + pointScore.ToString();
    }


}