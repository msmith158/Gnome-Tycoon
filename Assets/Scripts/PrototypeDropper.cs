using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrototypeDropper : MonoBehaviour
{
    private PrototypeFactorySystem gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("proto_gameManager").GetComponent<PrototypeFactorySystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            float value = other.GetComponent<PrototypeObject>().value;
            gameManager.AddScore(value);
            Destroy(other);
        }
    }
}
