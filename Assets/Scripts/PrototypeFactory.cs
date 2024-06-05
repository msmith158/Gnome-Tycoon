using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PrototypeFactory : MonoBehaviour
{
    /*public ProtoObjectType prototypeObjectType;
    private PrototypeFactorySettings gameManager;
    private GameObject objectPrefab;
    private Transform manufacturerSpawnPoint;
    private List<Collider> conveyor;
    private Button spawnButton;
    private float conveyorSpeed;
    private Collider basketTrigger;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("proto_gameManager").GetComponent<PrototypeFactorySettings>();
        spawnButton = gameManager.spawnButton;
        objectPrefab = gameManager.objectPrefab;
        basketTrigger = gameManager.basketTrigger;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider conveyor)
    {
        switch (prototypeObjectType)
        {
            case ProtoObjectType.Object:
                this.GetComponent<Rigidbody>().AddForce(0, 0, conveyorSpeed, ForceMode.Force);
                break;
        }
    }

    public enum ProtoObjectType
    {
        Manufacturer,
        Conveyor,
        Object,
        Basket
    }*/
}
