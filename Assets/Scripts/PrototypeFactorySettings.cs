using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PrototypeFactory;

public class PrototypeFactorySettings : MonoBehaviour
{
    [Header("Values")]
    public int pointScore;
    [Header("Object References")]
    public GameObject manufacturerSpawnPoint;
    public Collider basketTrigger;
    public GameObject objectPrefab;
    public Button spawnButton;

    public void SpawnObject()
    {
        Instantiate(objectPrefab, manufacturerSpawnPoint.transform.position, Quaternion.identity);
    }
}
