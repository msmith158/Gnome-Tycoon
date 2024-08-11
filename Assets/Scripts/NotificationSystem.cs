using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationSystem : MonoBehaviour
{
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
                break;
            case 1:
                break;
            case 2:
                break;
        }

    }

    public void DestroyNotification(GameObject notification)
    {

    }
}
