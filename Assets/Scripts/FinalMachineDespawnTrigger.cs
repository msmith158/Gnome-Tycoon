using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMachineDespawnTrigger : MonoBehaviour
{
    [SerializeField] private FinalMachineSystems parentToCallBackTo;
    [HideInInspector] public Vector3 oldObjectVelocity;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("gnome")) // Only call the function for the manufactured objects to stop accidentally processing other objects
        {
            parentToCallBackTo.MachineFunctions(other.gameObject, other.GetComponent<Rigidbody>().velocity);
        }
    }
}
