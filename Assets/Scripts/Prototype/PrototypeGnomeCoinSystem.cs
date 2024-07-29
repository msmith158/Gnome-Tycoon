using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PrototypeGnomeCoinSystem : MonoBehaviour
{
    [Header("Object References")]
    public TextMeshProUGUI gnomeCoinText;
    public List<GameObject> oneTimeObjects = new List<GameObject>();
    public List<string> oneTimeObjectNames = new List<string>();

    [Header("Values")]
    public int coinCount;
    public float permanentValue;
    public float permanentSpeed;
    public float permanentTime;
    public float permanentCooldown;

    public void Initialise()
    {
        if (gnomeCoinText != null)
        {
            gnomeCoinText.text = "�" + coinCount;
        }
    }

    // Update is called once per frame
    public void AddCoins(int amountToAdd)
    {
        coinCount += amountToAdd;
        gnomeCoinText.text = "�" + coinCount;
    }
}