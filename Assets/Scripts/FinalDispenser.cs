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
    public bool isAutoActivated;
    public List<GameObject> objectsList = new List<GameObject>();
    private bool isActivated = false;
    private Scrollbar slider;
    [SerializeField] private float objectXOffset;
    private DDOLManager ddolManager;
    [HideInInspector] public bool isAutoRunning = false;

    void OnEnable()
    {
        slider = timeSlider.GetComponent<Scrollbar>();
        initialManuTime = manufacturingTime;
        
        // Make sure to load the game via the loading level, otherwise these objects won't exist
        gnomeCoinSys = GameObject.Find("ddolManager").GetComponent<GnomeCoinSystem>();
        ddolManager = GameObject.Find("ddolManager").GetComponent<DDOLManager>();
    }
    
    public void SpawnObject() // This function is to be called if trying to use the DelayedSpawn method
    {
        if (!isActivated)
        {
            isActivated = true;
            StartCoroutine(DelayedSpawn());
        }
    }

    public void SpawnAuto()
    {
        if (!isAutoActivated)
        {
            isAutoActivated = true;
            StartCoroutine(AutomatedSpawn());
        }
    }
    
    private IEnumerator DelayedSpawn()
    {
        // Work the manufacturing delay timer
        timeSlider.SetActive(true);
        timeSlider.transform.Find("timerText").GetComponent<TextMeshProUGUI>().text = "Manufacturing...";
        sys.manufacturingButtonImage.sprite = sys.manufacturingButtonPressed;
        float manuT = manufacturingTime + gnomeCoinSys.permanentTime;

        for (float timer = manuT; timer > 0; timer -= Time.deltaTime)
        {
            timer = Mathf.Clamp(timer, 0f, manuT);
            float progress = Mathf.InverseLerp(0f, manuT, timer);
            slider.size = progress;
            yield return null;
        }

        // Instantiate the new raw materials from the dispenser
        Vector3 newPos = new Vector3(spawnTrigger.transform.position.x + objectXOffset, spawnTrigger.transform.position.y,
            spawnTrigger.transform.position.z);
        GameObject newObject = Instantiate(newPrefab, newPos, Quaternion.identity);
        newObject.tag = "gnome";
        objectsList.Add(newObject);
        ddolManager.totalGnomesMade++;

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
        sys.manufacturingButtonImage.sprite = sys.manufacturingButtonUnpressed;
        sys.FinishDispensing();
        isActivated = false;
    }

    public IEnumerator AutomatedSpawn()
    {
        while (isAutoRunning)
        {
            float manuT = manufacturingTime + gnomeCoinSys.permanentTime;
            yield return new WaitForSeconds(manuT);
            
            Vector3 newPos = new Vector3(spawnTrigger.transform.position.x + objectXOffset, spawnTrigger.transform.position.y,
                spawnTrigger.transform.position.z);
            GameObject newObject = Instantiate(newPrefab, newPos, Quaternion.identity);
            newObject.tag = "gnome";
            objectsList.Add(newObject);
            ddolManager.totalGnomesMade++;
        }
        yield return null;
    }
}
