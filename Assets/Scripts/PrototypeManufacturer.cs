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

    [Header("Object References")]
    public GameObject timeSlider;
    public GameObject manufacturerSpawnPoint;
    public GameObject objectPrefab;
    public GameObject objectModel;
    public PrototypeFactorySystem sys;
    [HideInInspector] public PrototypeInitialisation initSys;
    public List<GameObject> objectsList = new List<GameObject>();
    public List<Material> gnomeMaterialList = new List<Material>();

    // Private variables
    private bool isActivated = false;
    private Scrollbar slider;

    private void Start()
    {
        slider = timeSlider.GetComponent<Scrollbar>();
        initialManuTime = manufacturingTime;
        initialManuCool = manufacturingCooldown;
    }

    private void OnEnable()
    {
        initSys = GameObject.Find("proto_ddolManager").GetComponent<PrototypeInitialisation>();
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
        timeSlider.SetActive(true);
        timeSlider.transform.Find("timerText").GetComponent<TextMeshProUGUI>().text = "Manufacturing...";

        for (float timer = manufacturingTime; timer > 0; timer -= Time.deltaTime)
        {
            timer = Mathf.Clamp(timer, 0f, manufacturingTime);
            float progress = Mathf.InverseLerp(0f, manufacturingTime, timer);
            slider.size = progress;
            yield return null;
        }

        GameObject newObject = Instantiate(objectPrefab, manufacturerSpawnPoint.transform.position, Quaternion.identity);
        if (initSys.resetTimes == 0)
        {
            switch (sys.prestigeLvl)
            {
                case PrototypeFactorySystem.PrestigeLevel.Prestige0:
                    for (int i = 0; i < newObject.transform.childCount; i++)
                    {
                        newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[0];
                        Debug.Log("Le lolz");
                    }
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige1:
                    for (int i = 0; i < newObject.transform.childCount; i++)
                    {
                        newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[1];
                    }
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige2:
                    for (int i = 0; i < newObject.transform.childCount; i++)
                    {
                        newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[2];
                    }
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige3:
                    for (int i = 0; i < newObject.transform.childCount; i++)
                    {
                        newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[3];
                    }
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige4:
                    for (int i = 0; i < newObject.transform.childCount; i++)
                    {
                        newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[4];
                    }
                    break;
                case PrototypeFactorySystem.PrestigeLevel.Prestige5:
                    for (int i = 0; i < newObject.transform.childCount; i++)
                    {
                        newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[5];
                    }
                    break;
            }
        }
        else if (initSys.resetTimes >= 1) 
        {
            for (int i = 0; i < newObject.transform.childCount; i++)
            {
                newObject.transform.GetChild(i).GetComponent<Renderer>().material = gnomeMaterialList[6];
            }
        }
        objectsList.Add(newObject);
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
