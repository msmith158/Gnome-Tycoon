using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeCoinShopSystem : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float spinnerTime = 2f;
    private bool isReadyToDestroy = false;

    [Header("Object References")]
    private GnomeCoinSystem gnomeCoinSys;
    public GameObject promptBackground;
    public GameObject spinnerBackground;

    public void OnEnable()
    {
        gnomeCoinSys = GameObject.Find("ddolManager").GetComponent<GnomeCoinSystem>();
    }
    public void BuyGnomeCoins(int amount)
    {
        StartCoroutine(GnomeCoinPurchaseProcess(amount));
    }

    public void DisableListing(GameObject objectToDestroy)
    {
        StartCoroutine(DisableListingDelay(objectToDestroy));
    }

    IEnumerator GnomeCoinPurchaseProcess(int amountToBuy)
    {
        spinnerBackground.SetActive(true);
        yield return new WaitForSeconds(spinnerTime);
        gnomeCoinSys.AddCoins(amountToBuy, false);
        spinnerBackground.SetActive(false);
        isReadyToDestroy = true;
    }

    IEnumerator DisableListingDelay(GameObject obj)
    {
        while (!isReadyToDestroy)
        {
            yield return null;
        }
        obj.SetActive(false);
        isReadyToDestroy = false;
        gnomeCoinSys.oneTimeObjects.Add(obj);
    }
}
