using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PrototypeFactory;

public class PrototypeFactorySettings : MonoBehaviour
{
    [Header("Values")]
    public int pointScore;
    public float manufacturingTime;
    public float manufacturingCooldown;
    [Header("Object References")]
    public GameObject manufacturerSpawnPoint;
    public Collider basketTrigger;
    public GameObject objectPrefab;
    public Button spawnButton;
    public TextMeshProUGUI moneyText;
    public GameObject timeSlider;
    public Scrollbar slider;

    private bool isActivated = false;

    public void Start()
    {
        slider = timeSlider.GetComponent<Scrollbar>();
    }

    public void SpawnObject()
    {
        if (!isActivated)
        {
            isActivated = true;
            StartCoroutine(DelayedSpawn());
        }
    }

    public void AddScore(int amount)
    {
        pointScore = pointScore + amount;
        moneyText.text = "Profit: $" + pointScore.ToString();
    }

    IEnumerator DelayedSpawn()
    {
        timeSlider.SetActive(true);
        timeSlider.transform.Find("timerText").GetComponent<TextMeshProUGUI>().text = "Manufacturing...";

        for (float timer = manufacturingTime; timer > 0; timer -= Time.deltaTime)
        {
            timer = Mathf.Clamp(timer, 0f, manufacturingTime); // Ensure countdown stays within 0 and duration
            float normalizedCountdown = timer / manufacturingTime; // Normalize countdown to range 0 to 1
            slider.size = timer;
            yield return null;
        }

        Instantiate(objectPrefab, manufacturerSpawnPoint.transform.position, Quaternion.identity);
        timeSlider.transform.Find("timerText").GetComponent<TextMeshProUGUI>().text = "Cooling down...";

        for (float timer = 0; timer > manufacturingCooldown; timer += Time.deltaTime)
        {
            timer = Mathf.Clamp(timer, 0f, manufacturingCooldown); // Ensure countdown stays within 0 and duration
            float normalizedCountdown = timer / manufacturingCooldown; // Normalize countdown to range 0 to 1
            slider.size = timer;
            yield return null;
        }

        timeSlider.SetActive(false);
        isActivated = false;
    }
}