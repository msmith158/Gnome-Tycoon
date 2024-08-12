using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotificationSystem : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private int flashAmount;
    [SerializeField] private float flashLength;

    [Header("Object References")] 
    [SerializeField] private AdSystem adSys;
    [SerializeField] private GameObject adNotification;
    [SerializeField] private GameObject mailNotification;
    [SerializeField] private GameObject sabotageNotification;
    [SerializeField] private GameObject listArea;
    [SerializeField] private AudioSource uiSfxSource;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private Image greenVignette;
    [SerializeField] private Image bellNotifIcon;
    private List<GameObject> activeNotifications = new List<GameObject>();

    public void AddNotification(int type)
    {
        switch (type)
        {
            case 0:
                GameObject newAdNotif = Instantiate(adNotification, listArea.transform);
                activeNotifications.Add(newAdNotif);
                StartCoroutine(PushAlert());
                Button button = newAdNotif.transform.GetChild(0).GetComponent<Button>();
                button.onClick.AddListener(() => uiSfxSource.PlayOneShot(buttonSound));
                button.onClick.AddListener(() => adSys.PlayFakeAd());
                button.onClick.AddListener(() => DestroyNotification(newAdNotif));
                break;
            case 1:
                break;
            case 2:
                break;
        }

    }

    public void DestroyNotification(GameObject obj)
    {
        Destroy(obj);
    }

    private IEnumerator PushAlert()
    {
        greenVignette.gameObject.SetActive(true);
        bellNotifIcon.enabled = true;
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
