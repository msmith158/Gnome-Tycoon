using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalMachineSystems : MonoBehaviour
{
    [Tooltip("It's ideal that you set these regardless of machine type.")][Header("General/Shared Properties")]
    [SerializeField] private MachineType machineType;
    [SerializeField] private GameObject spawnTrigger;
    [SerializeField] private FinalMachineDespawnTrigger despawnTrigger;
    [SerializeField] private GameObject newPrefab;
    [SerializeField] private FinalDispenser dispenser;
    [HideInInspector] public DDOLManager initSys;
    public PrototypeFactorySystem sys;

    [Header("Painter Properties")]
    public List<Material> gnomeMaterialList = new List<Material>();
    
    [Header("Packager Properties")]
    [SerializeField] private GameObject jimboPrefab;
    [Tooltip("Put the max odds in whole numbers. For example, for a 1 in 10,000 chance, put 10000. This number CANNOT be 0.")] [SerializeField] private int chanceOfJimbo;
    private int jimboNumber = 0;
    private bool wasJimboSpawned;
    
    private void OnEnable()
    {
        // Make sure to load the game via the loading level, otherwise these objects won't exist
        initSys = GameObject.Find("ddolManager").GetComponent<DDOLManager>();
    }

    public void MachineFunctions(GameObject other, Vector3 objectVelocity)
    {
        switch (machineType)
        {
            case MachineType.Sprayer: // The functions for the sprayer machine
                Vector3 newSprayerObjectVelocity = objectVelocity;
                Destroy(other);
                GameObject newSprayedObject = Instantiate(newPrefab, spawnTrigger.transform.position, Quaternion.identity);
                newSprayedObject.tag = "gnome";
                dispenser.objectsList.Add(newSprayedObject);
                newSprayedObject.GetComponent<Rigidbody>().velocity = newSprayerObjectVelocity;
                break;
            case MachineType.Moulder: // The functions for the moulder machine
                Vector3 newMoulderObjectVelocity = objectVelocity;
                Destroy(other);
                GameObject newMouldedObject = Instantiate(newPrefab, spawnTrigger.transform.position, Quaternion.identity);
                newMouldedObject.tag = "gnome";
                dispenser.objectsList.Add(newMouldedObject);
                newMouldedObject.GetComponent<Rigidbody>().velocity = newMoulderObjectVelocity;
                break;
            case MachineType.Painter: // The functions for the painter machine
                // Apply the prestige-relevant material to the newly-spawned gnome
                if (initSys.resetTimes == 0)
                {
                    switch (sys.prestigeLvl)
                    {
                        case PrototypeFactorySystem.PrestigeLevel.Prestige0: // Dirty red gnome material
                            for (int i = 0; i < other.transform.childCount; i++)
                            {
                                other.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[0];
                            }
                            break;
                        case PrototypeFactorySystem.PrestigeLevel.Prestige1: // Red gnome material
                            for (int i = 0; i < other.transform.childCount; i++)
                            {
                                other.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[1];
                            }
                            break;
                        case PrototypeFactorySystem.PrestigeLevel.Prestige2: // Yellow gnome material
                            for (int i = 0; i < other.transform.childCount; i++)
                            {
                                other.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[2];
                            }
                            break;
                        case PrototypeFactorySystem.PrestigeLevel.Prestige3: // Green gnome material
                            for (int i = 0; i < other.transform.childCount; i++)
                            {
                                other.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[3];
                            }
                            break;
                        case PrototypeFactorySystem.PrestigeLevel.Prestige4: // Cyan gnome material
                            for (int i = 0; i < other.transform.childCount; i++)
                            {
                                other.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[4];
                            }
                            break;
                        case PrototypeFactorySystem.PrestigeLevel.Prestige5: // Purple gnome material
                            for (int i = 0; i < other.transform.childCount; i++)
                            {
                                other.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[5];
                            }
                            break;
                    }
                }
                else if (initSys.resetTimes >= 1) // If the game has been reset at least once, golden gnome material
                {
                    for (int i = 0; i < other.transform.childCount; i++)
                    {
                        other.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[6];
                    }
                }
                other.transform.position = spawnTrigger.transform.position;
                other.GetComponent<Rigidbody>().velocity = objectVelocity;
                break;
            case MachineType.Packager:
                Destroy(other);
                // Do a roll between a gnome and Jimbo, and then instantiate
                jimboNumber = Random.Range(0, chanceOfJimbo);
                Debug.Log(jimboNumber);
                //if (jimboNumber == 0 && sys.prestigeLvl != PrototypeFactorySystem.PrestigeLevel.Prestige0) // This is to spawn Jimbo
                if (jimboNumber == 0)
                {
                    GameObject newJimbo = Instantiate(jimboPrefab, spawnTrigger.transform.position, Quaternion.identity);
                    newJimbo.tag = "gnome";
                    dispenser.objectsList.Add(newJimbo);
                    newJimbo.GetComponent<Rigidbody>().velocity = objectVelocity;
                }
                else // This is to spawn the raw materials out of the dispenser
                {
                    GameObject newPackagedObject = Instantiate(newPrefab, spawnTrigger.transform.position, Quaternion.identity);
                    newPackagedObject.tag = "gnome";
                    dispenser.objectsList.Add(newPackagedObject);
                    newPackagedObject.GetComponent<Rigidbody>().velocity = objectVelocity;
                }
                break;
        }
    }

    private enum MachineType // YOU MUST SET YOUR MACHINES TO ONE OF THESE IN ORDER TO MAKE THIS SCRIPT WORK!
    {
        Sprayer,
        Moulder,
        Painter,
        Packager
    }
}