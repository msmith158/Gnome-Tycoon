using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrototypeGnomeCoinSystem : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI gnomeCoinText;
    public List<GameObject> oneTimeObjects = new List<GameObject>();

    public int coinCount;

    private bool isStarted;

    // Start is called before the first frame update
    void Start()
    {
        switch (isStarted)
        {
            case true:
                break;
            case false:
                DontDestroyOnLoad(this.gameObject);
                isStarted = true;
                break;
        }
    }

    // Update is called once per frame
    public void AddCoins(int amountToAdd)
    {
        coinCount += amountToAdd;
        gnomeCoinText.text = "¢" + coinCount;
    }
}
