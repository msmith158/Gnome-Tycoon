using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrototypeDropper : MonoBehaviour
{
    private PrototypeFactorySystem gameManager;
    private float value;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("proto_gameManager").GetComponent<PrototypeFactorySystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            switch (gameManager.prestigeLvl)
            {
                case PrototypeFactorySystem.PrestigeLevel.Prestige1:
                    value = gameManager.lvl1Value;
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige2:
                    value = gameManager.lvl2Value;
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige3:
                    value = gameManager.lvl3Value;
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige4:
                    value = gameManager.lvl4Value;
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige5:
                    value = gameManager.lvl5Value;
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige6:
                    value = gameManager.lvl6Value;
                    break;
            }
            gameManager.AddScore(value);
            Destroy(other);
        }
    }
}