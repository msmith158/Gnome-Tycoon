using UnityEngine;

public class FinalMachineDespawnTrigger : MonoBehaviour
{
    [Header("General/Shared Properties")]
    public TriggerType triggerType;

    [Header("Machine Trigger Properties")]
    [SerializeField] private FinalMachineSystems parentToCallBackTo;

    [Header("Exit Trigger Properties")]
    public FinalFactorySystem gameManager;
    private double value;

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
                        case FinalFactorySystem.PrestigeLevel.Prestige0:
                            value = gameManager.lvl1Value;
                            break;
                        case FinalFactorySystem.PrestigeLevel.Prestige1:
                            value = gameManager.lvl2Value;
                            break;
                        case FinalFactorySystem.PrestigeLevel.Prestige2:
                            value = gameManager.lvl3Value;
                            break;
                        case FinalFactorySystem.PrestigeLevel.Prestige3:
                            value = gameManager.lvl4Value;
                            break;
                        case FinalFactorySystem.PrestigeLevel.Prestige4:
                            value = gameManager.lvl5Value;
                            break;
                        case FinalFactorySystem.PrestigeLevel.Prestige5:
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
