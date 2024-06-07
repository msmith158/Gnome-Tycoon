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

    // Private variables
    private bool isActivated = false;
    private Scrollbar slider;

    private void Start()
    {
        slider = timeSlider.GetComponent<Scrollbar>();
        initialManuTime = manufacturingTime;
        initialManuCool = manufacturingCooldown;
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

        Instantiate(objectPrefab, manufacturerSpawnPoint.transform.position, Quaternion.identity);
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
