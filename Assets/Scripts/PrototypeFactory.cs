using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrototypeFactory : MonoBehaviour
{
    public ProtoObjectType prototypeObjectType;
    private Transform manufacturerSpawnPoint;
    private BoxCollider conveyor;

    // Start is called before the first frame update
    void Start()
    {
        manufacturerSpawnPoint = transform.Find("manufacturerSpawnPoint");
        conveyor = GameObject.Find("conveyorBelt1").GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (prototypeObjectType)
        {
            case ProtoObjectType.Conveyor:

                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (prototypeObjectType)
        {
            case ProtoObjectType.Object:
                break;
        }
    }

    public enum ProtoObjectType
    {
        Manufacturer,
        Conveyor,
        Object,
        Basket
    }
}
