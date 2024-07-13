using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrototypeGnomeCoinShopSystem : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float spinnerTime = 2f;

    [Header("Object References")]
    [SerializeField] private PrototypeGnomeCoinSystem gnomeCoinSys;
    public GameObject promptBackground;
    public GameObject spinnerBackground;

    public void BuyGnomeCoins(int amount)
    {
        StartCoroutine(GnomeCoinPurchaseProcess(amount));
    }

    IEnumerator GnomeCoinPurchaseProcess(int amountToBuy)
    {
        promptBackground.SetActive(true);
        spinnerBackground.SetActive(true);
        yield return new WaitForSeconds(spinnerTime);

    }
}
