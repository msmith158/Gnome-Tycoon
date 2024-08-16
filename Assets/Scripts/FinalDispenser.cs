using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalDispenser : MonoBehaviour
{
    [SerializeField] private GameObject spawnTrigger;
    [SerializeField] private GameObject newPrefab;
    [HideInInspector] public GnomeCoinSystem gnomeCoinSys;
    [SerializeField] private FinalFactorySystem sys;
    public float manufacturingTime;
    public float manufacturingCooldown;
    [HideInInspector] public float initialManuTime;
    [HideInInspector] public float initialManuCool;
    public GameObject timeSlider;
    public bool isAutomated;
    public List<GameObject> objectsList = new List<GameObject>();
    private bool isActivated = false;
    private Scrollbar slider;

    void OnEnable()
    {
        slider = timeSlider.GetComponent<Scrollbar>();
        initialManuTime = manufacturingTime;
        
        // Make sure to load the game via the loading level, otherwise these objects won't exist
        gnomeCoinSys = GameObject.Find("ddolManager").GetComponent<GnomeCoinSystem>();
    }
    
    public void SpawnObject() // This function is to be called if trying to use the DelayedSpawn method
    {
        if (!isActivated)
        {
            isActivated = true;
            StartCoroutine(DelayedSpawn());
        }
    }
    
    private IEnumerator DelayedSpawn()
    {
        // Work the manufacturing delay timer
        timeSlider.SetActive(true);
        timeSlider.transform.Find("timerText").GetComponent<TextMeshProUGUI>().text = "Manufacturing...";
        //manufacturingTime += (manufacturingTime * gnomeCoinSys.permanentTime);
        float manuT = manufacturingTime + (manufacturingTime * gnomeCoinSys.permanentTime);
        manufacturingCooldown += (manufacturingCooldown * gnomeCoinSys.permanentCooldown);

        for (float timer = manufacturingTime; timer > 0; timer -= Time.deltaTime)
        {
            timer = Mathf.Clamp(timer, 0f, manufacturingTime);
            float progress = Mathf.InverseLerp(0f, manufacturingTime, timer);
            slider.size = progress;
            yield return null;
        }

        // Instantiate the new raw materials from the dispenser
        GameObject newObject = Instantiate(newPrefab, spawnTrigger.transform.position, Quaternion.identity);
        newObject.tag = "gnome";
        objectsList.Add(newObject);

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
        sys.FinishDispensing();
        isActivated = false;
    }

    public IEnumerator AutomatedSpawn()
    {
        yield return null;
    }
}
