using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrototypeGnomeCoinSystem : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI gnomeCoinText;
    [HideInInspector] public int coinCount;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    public void AddCoins(int amountToAdd)
    {
        coinCount += amountToAdd;
        gnomeCoinText.text = "¢" + coinCount;
    }
}
