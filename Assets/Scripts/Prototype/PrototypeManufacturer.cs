using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrototypeManufacturer : MonoBehaviour
{
    [Header("Values")]
    public float manufacturingTime;
    public float manufacturingCooldown;
    [HideInInspector] public float initialManuTime;
    [HideInInspector] public float initialManuCool;
    [Tooltip("Put the max odds in whole numbers. For example, for a 1 in 10,000 chance, put 10000. This number CANNOT be 0.")] [SerializeField] private int chanceOfJimbo;
    private int jimboNumber = 0;
    private bool wasJimboSpawned;

    [Header("Object References")]
    public GameObject timeSlider;
    public GameObject manufacturerSpawnPoint;
    public GameObject objectPrefab;
    [SerializeField] private GameObject jimboPrefab;
    public GameObject objectModel;
    public PrototypeFactorySystem sys;
    [HideInInspector] public PrototypeDDOLManager initSys;
    [HideInInspector] public PrototypeGnomeCoinSystem gnomeCoinSys;
    public List<GameObject> objectsList = new List<GameObject>();
    public List<Material> gnomeMaterialList = new List<Material>();
    private GameObject newObject;
    private GameObject newJimbo;

    // Private variables
    private bool isActivated = false;
    private Scrollbar slider;

    private void Start()
    {
        slider = timeSlider.GetComponent<Scrollbar>();
        initialManuTime = manufacturingTime;
    }

    private void OnEnable()
    {
        // Make sure to load the game via the loading level, otherwise these objects won't exist
        initSys = GameObject.Find("proto_ddolManager").GetComponent<PrototypeDDOLManager>();
        gnomeCoinSys = GameObject.Find("proto_ddolManager").GetComponent<PrototypeGnomeCoinSystem>();
    }

    public void SpawnObject()
    {
        if (!isActivated)
        {
            isActivated = true;
            StartCoroutine(DelayedSpawn());
        }
    }

    IEnumerator DelayedSpawn()
    {
        // Work the manufacturing delay timer
        timeSlider.SetActive(true);
        timeSlider.transform.Find("timerText").GetComponent<TextMeshProUGUI>().text = "Manufacturing...";
        manufacturingTime += (manufacturingTime * gnomeCoinSys.permanentTime);
        manufacturingCooldown += (manufacturingCooldown * gnomeCoinSys.permanentCooldown);

        for (float timer = manufacturingTime; timer > 0; timer -= Time.deltaTime)
        {
            timer = Mathf.Clamp(timer, 0f, manufacturingTime);
            float progress = Mathf.InverseLerp(0f, manufacturingTime, timer);
            slider.size = progress;
            yield return null;
        }

        // Do a roll between a gnome and Jimbo, and then instantiate
        jimboNumber = Random.Range(0, chanceOfJimbo);
        Debug.Log(jimboNumber);
        if (jimboNumber == 0 && sys.prestigeLvl != PrototypeFactorySystem.PrestigeLevel.Prestige0) // This is to spawn Jimbo
        {
            Debug.Log("Hey buddy");
            newJimbo = Instantiate(jimboPrefab, manufacturerSpawnPoint.transform.position, Quaternion.identity);
            wasJimboSpawned = true;
        }
        else // This is to spawn a gnome
        {
            newObject = Instantiate(objectPrefab, manufacturerSpawnPoint.transform.position, Quaternion.identity);
            wasJimboSpawned = false;
            Debug.Log("Gnome spawned");
        }

        // If not Jimbo, apply the prestige-relevant material to the newly-spawned gnome
        if (initSys.resetTimes == 0)
        {
            switch (sys.prestigeLvl)
            {
                case PrototypeFactorySystem.PrestigeLevel.Prestige0: // Dirty red gnome material
                    for (int i = 0; i < newObject.transform.childCount; i++)
                    {
                        newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[0];
                    }
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige1: // Red gnome material
                    switch (wasJimboSpawned)
                    {
                        case false:
                            for (int i = 0; i < newObject.transform.childCount; i++)
                            {
                                newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[1];
                            }
                            break;
                        case true:
                            break;
                    }
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige2: // Yellow gnome material
                    switch (wasJimboSpawned)
                    {
                        case false:
                            for (int i = 0; i < newObject.transform.childCount; i++)
                            {
                                newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[2];
                            }
                            break;
                        case true:
                            break;
                    }
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige3: // Green gnome material
                    switch (wasJimboSpawned)
                    {
                        case false:
                            for (int i = 0; i < newObject.transform.childCount; i++)
                            {
                                newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[3];
                            }
                            break;
                        case true:
                            break;
                    }
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige4: // Cyan gnome material
                    switch (wasJimboSpawned)
                    {
                        case false:
                            for (int i = 0; i < newObject.transform.childCount; i++)
                            {
                                newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[4];
                            }
                            break;
                        case true:
                            break;
                    }
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige5: // Purple gnome material
                    switch (wasJimboSpawned)
                    {
                        case false:
                            for (int i = 0; i < newObject.transform.childCount; i++)
                            {
                                newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[5];
                            }
                            break;
                        case true:
                            break;
                    }
                    break;
            }
        }
        else if (initSys.resetTimes >= 1) // If the game has been reset at least once, golden gnome material
        {
            for (int i = 0; i < newObject.transform.childCount; i++)
            {
                newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[6];
            }
        }

        // Tag and assign objects
        switch (wasJimboSpawned)
        {
            case false:
                newObject.tag = "gnome";
                objectsList.Add(newObject);
                break;
            case true:
                newJimbo.tag = "gnome";
                objectsList.Add(newJimbo);
                break;
        }

        // Work the manufacturer cooldown timer
        timeSlider.transform.Find("timerText").GetComponent<TextMeshProUGUI>().text = "Cooling down...";
        for (float timer = 0; timer < manufacturingCooldown; timer += Time.deltaTime)
        {
            timer = Mathf.Clamp(timer, 0f, manufacturingCooldown);
            float progress = Mathf.InverseLerp(0f, manufacturingCooldown, timer);
            slider.size = progress;
            yield return null;
        }

        timeSlider.SetActive(false);
        isActivated = false;
    }
}
