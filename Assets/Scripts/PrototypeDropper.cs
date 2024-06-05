using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrototypeDropper : MonoBehaviour
{
    private PrototypeFactorySettings gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("proto_gameManager").GetComponent<PrototypeFactorySettings>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            gameManager.AddScore(1);
            Destroy(other);
        }
    }
}
