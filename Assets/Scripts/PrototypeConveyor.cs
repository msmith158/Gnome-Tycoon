using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeConveyor : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 2.0f;
    [HideInInspector] public float initialSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialSpeed = speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.position -= transform.forward * speed * Time.deltaTime;
        rb.MovePosition(rb.position + transform.forward * speed * Time.deltaTime);
    }
}
