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
    public List<GameObject> oneTimeObjects = new List<GameObject>();
    public List<string> oneTimeObjectNames = new List<string>();
    private Image vignette;

    [Header("Values")]
    public int coinCount;
    public float permanentValue;
    public float permanentSpeed;
    public float permanentTime;
    public float permanentCooldown;
    [SerializeField] private int flashAmount;
    [SerializeField] private float flashLength;

    public void Initialise()
    {
        if (gnomeCoinText != null)
        {
            gnomeCoinText.text = "Coins: ¢" + coinCount;
        }
        AddReferences();
    }

    // Update is called once per frame
    public void AddCoins(int amountToAdd)
    {
        coinCount += amountToAdd;
        gnomeCoinText.text = "Coins: ¢" + coinCount;
        StartCoroutine(FlashScreenGreen());
    }

    private void AddReferences()
    {
        sys = GameObject.Find("gameManager").GetComponent<FinalFactorySystem>();
        vignette = sys.gnomeCoinVignetteReference;
        Debug.Log(vignette);
    }

    private IEnumerator FlashScreenGreen()
    {
        vignette.gameObject.SetActive(true);
        for (int i = 0; i < flashAmount; i++)
        {
            vignette.enabled = true;
            yield return new WaitForSeconds(flashLength);
            vignette.enabled = false;
            yield return new WaitForSeconds(flashLength);
        }
        vignette.gameObject.SetActive(false);
    }
}
