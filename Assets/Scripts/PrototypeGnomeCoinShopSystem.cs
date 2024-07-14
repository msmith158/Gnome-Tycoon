using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrototypeGnomeCoinShopSystem : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float spinnerTime = 2f;
    private bool isReadyToDestroy = false;

    [Header("Object References")]
    [SerializeField] private PrototypeGnomeCoinSystem gnomeCoinSys;
    public GameObject promptBackground;
    public GameObject spinnerBackground;

    public void BuyGnomeCoins(int amount)
    {
        StartCoroutine(GnomeCoinPurchaseProcess(amount));
    }

    public void DestroyListing(GameObject objectToDestroy)
    {
        StartCoroutine(DestroyListingDelay(objectToDestroy));
    }

    IEnumerator GnomeCoinPurchaseProcess(int amountToBuy)
    {
        spinnerBackground.SetActive(true);
        yield return new WaitForSeconds(spinnerTime);
        gnomeCoinSys.AddCoins(amountToBuy);
        spinnerBackground.SetActive(false);
        isReadyToDestroy = true;
    }

    IEnumerator DestroyListingDelay(GameObject obj)
    {
        while (!isReadyToDestroy)
        {
            yield return null;
        }
        Destroy(obj);
        isReadyToDestroy = false;
    }
}
