using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GnomeCoinSystem : MonoBehaviour
{
    [Header("Object References")]
    public TextMeshProUGUI gnomeCoinText;
    private FinalFactorySystem sys;
    private MainMenuScript menuSys;
    public List<GameObject> oneTimeObjects = new List<GameObject>();
    public List<string> oneTimeObjectNames = new List<string>();
    private Image vignette;

    [Header("Values")]
    public int coinCount;
    public float permanentValue;
    public float permanentSpeed;
    public float permanentTime;
    public float permanentCooldown;
    [SerializeField] private int passiveIncomeAmount;
    [SerializeField] private float passiveIncomeTime;
    [SerializeField] private int flashAmount;
    [SerializeField] private float flashLength;
    [SerializeField] private int passiveFlashAmount;
    [SerializeField] private float passiveFlashLength;

    public void Initialise()
    {
        if (gnomeCoinText != null)
        {
            gnomeCoinText.text = "Coins: c" + coinCount;
        }
        AddReferences();
        switch (menuSys.isOver13)
        {
            case false:
                StartCoroutine(PassiveIncome());
                break;
            case true:
                break;
        }
    }

    // Update is called once per frame
    public void AddCoins(int amountToAdd, bool isPassive)
    {
        coinCount += amountToAdd;
        gnomeCoinText.text = "Coins: c" + coinCount;
        StartCoroutine(FlashScreenGreen(isPassive));
    }

    private void AddReferences()
    {
        sys = GameObject.Find("gameManager").GetComponent<FinalFactorySystem>();
        menuSys = GameObject.Find("mainMenuSystemHandler").GetComponent<MainMenuScript>();
        vignette = sys.gnomeCoinVignetteReference;
    }

    private IEnumerator PassiveIncome()
    {
        while (!menuSys.isOver13)
        {
            yield return new WaitForSeconds(passiveIncomeTime);
            AddCoins(passiveIncomeAmount, true);
        }
    }

    private IEnumerator FlashScreenGreen(bool isPassive)
    {
        vignette.gameObject.SetActive(true);
        switch (isPassive)
        {
            case true:
                for (int i = 0; i < passiveFlashAmount; i++)
                {
                    vignette.enabled = true;
                    yield return new WaitForSeconds(passiveFlashLength);
                    vignette.enabled = false;
                    yield return new WaitForSeconds(passiveFlashLength);
                }
                vignette.gameObject.SetActive(false);
                break;
            case false:
                for (int i = 0; i < flashAmount; i++)
                {
                    vignette.enabled = true;
                    yield return new WaitForSeconds(flashLength);
                    vignette.enabled = false;
                    yield return new WaitForSeconds(flashLength);
                }
                vignette.gameObject.SetActive(false);
                break;
        }
    }
}
