using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMachineDespawnTrigger : MonoBehaviour
{
    [Header("General/Shared Properties")]
    public TriggerType triggerType;

    [Header("Machine Trigger Properties")]
    [SerializeField] private FinalMachineSystems parentToCallBackTo;
    [HideInInspector] public Vector3 oldObjectVelocity;

    [Header("Exit Trigger Properties")]
    private PrototypeFactorySystem gameManager;
    private float value;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("proto_gameManager").GetComponent<PrototypeFactorySystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("gnome")) // Only call the function for the manufactured objects to stop accidentally processing other objects
        {
            switch (triggerType)
            {
                case TriggerType.MachineTrigger:
                    parentToCallBackTo.MachineFunctions(other.gameObject, other.GetComponent<Rigidbody>().velocity);
                    break;
                case TriggerType.ExitTrigger:
                    switch (gameManager.prestigeLvl)
                    {
                        case PrototypeFactorySystem.PrestigeLevel.Prestige0:
                            value = gameManager.lvl1Value;
                            break;
                        case PrototypeFactorySystem.PrestigeLevel.Prestige1:
                            value = gameManager.lvl2Value;
                            break;
                        case PrototypeFactorySystem.PrestigeLevel.Prestige2:
                            value = gameManager.lvl3Value;
                            break;
                        case PrototypeFactorySystem.PrestigeLevel.Prestige3:
                            value = gameManager.lvl4Value;
                            break;
                        case PrototypeFactorySystem.PrestigeLevel.Prestige4:
                            value = gameManager.lvl5Value;
                            break;
                        case PrototypeFactorySystem.PrestigeLevel.Prestige5:
                            value = gameManager.lvl6Value;
                            break;
                    }
                    gameManager.AddScore(value);
                    Destroy(other.gameObject);
                    break;
            }
        }
    }

    public enum TriggerType
    {
        MachineTrigger,
        ExitTrigger
    }
}
