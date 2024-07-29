using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FinalMachineSystems : MonoBehaviour
{
    [SerializeField] private MachineType machineType;
    [SerializeField] private GameObject spawnTrigger;
    [SerializeField] private GameObject newPrefab;
    [SerializeField] private PrototypeManufacturer manufacturer;
    private bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("gnome"))
        {
            switch (machineType)
            {
                case MachineType.Sprayer:
                    Destroy(other);
                    GameObject newSprayedObject = Instantiate(newPrefab, spawnTrigger.transform.position, Quaternion.identity);
                    newSprayedObject.tag = "gnome";
                    manufacturer.objectsList.Add(newSprayedObject);
                    break;
                case MachineType.Moulder:
                    Destroy(other);
                    GameObject newObject = Instantiate(newPrefab, spawnTrigger.transform.position, Quaternion.identity);
                    newObject.tag = "gnome";
                    manufacturer.objectsList.Add(newObject);
                    break;
            }
            /* COPY + PASTE BELOW
            Destroy(other);
            GameObject newObject = Instantiate(newPrefab, spawnTrigger.transform.position, Quaternion.identity);
            newObject.tag = "gnome";
            manufacturer.objectsList.Add(newObject);
            */
        }
    }

    public void SpawnObject()
    {
        if (!isActivated)
        {
            isActivated = true;
            StartCoroutine(DelayedSpawn());
        }
    }

    private IEnumerator DelayedSpawn()
    {
        yield return null;
    }

    private enum MachineType
    {
        Dispenser,
        Sprayer,
        Moulder,
        Painter,
        Packager
    }
}