using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeConveyor : MonoBehaviour
{
    public float conveyorSpeed;
    private Vector3 trueConveyorSpeed;
    public Collider protoObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            protoObject.GetComponent<PrototypeObject>().UpdateVelocity(trueConveyorSpeed);
            Debug.Log("Hi");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            protoObject.GetComponent<PrototypeObject>().UpdateVelocity(new Vector3(0, 0, 0));
            Debug.Log("Bye");
        }
    }
}