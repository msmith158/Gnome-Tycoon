using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrototypeUpgrades : MonoBehaviour
{
    [Header("Values")]
    public UpgradeType upgradeType;
    public float initialCost;
    private float currentPrice;


    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private PrototypeFactorySystem sys;
    [SerializeField] private PrototypeObject obj;

    // Start is called before the first frame update
    void Start()
    {
        currentPrice = initialCost;
        UpdatePrice(currentPrice);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNewValues(float percentage)
    {
        if (sys.pointScore >= currentPrice)
        {
            sys.pointScore -= currentPrice;

            switch (upgradeType)
            {
                case UpgradeType.GnomeValue:
                    obj.value = obj.initialValue + percentage;
                    UpdatePrice(obj.value);
                    break;
                case UpgradeType.ConveyorSpeed:
                    break;
                case UpgradeType.ManufactureTime:
                    break;
            }
        }
    }

    void UpdatePrice(float newPrice)
    {
        costText.text = "$" + newPrice;
    }

    public enum UpgradeType
    {
        GnomeValue,
        ConveyorSpeed,
        ManufactureTime
    }
}
