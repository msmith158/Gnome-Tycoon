using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrototypeGnomeCoinShopSystem : MonoBehaviour
{
    [Header("Values")]
    public float spinnerTime = 2f;

    [Header("Object References")]
    public TextMeshProUGUI gnomeCoinText;
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
