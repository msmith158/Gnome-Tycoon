using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class NotificationSystem : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private int flashAmount;
    [SerializeField] private float flashLength;

    [Header("Object References")]
    [SerializeField] private GameObject adNotification;
    [SerializeField] private GameObject mailNotification;
    [SerializeField] private GameObject sabotageNotification;
    [SerializeField] private GameObject listArea;
    [SerializeField] private Image greenVignette;
    private List<GameObject> activeNotifications = new List<GameObject>();

    public void AddNotification(int type)
    {
        switch (type)
        {
            case 0:
                GameObject newAdNotif = Instantiate(adNotification, listArea.transform);
                activeNotifications.Add(newAdNotif);
                StartCoroutine(PushAlert());
                break;
            case 1:
                break;
            case 2:
                break;
        }

    }

    private IEnumerator PushAlert()
    {
        greenVignette.gameObject.SetActive(true);
        for (int i = 0; i < flashAmount; i++)
        {
            greenVignette.enabled = true;
            yield return new WaitForSeconds(flashLength);
            greenVignette.enabled = false;
            yield return new WaitForSeconds(flashLength);
        }
        greenVignette.gameObject.SetActive(false);
    }
}
