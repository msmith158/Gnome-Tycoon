using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalConveyor : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 2.0f;
    [HideInInspector] public float initialSpeed;
    [HideInInspector] public GnomeCoinSystem gnomeCoinSys;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialSpeed = speed;
    }

    private void OnEnable()
    {
        gnomeCoinSys = GameObject.Find("ddolManager").GetComponent<GnomeCoinSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.position -= transform.forward * (speed + (speed * gnomeCoinSys.permanentSpeed)) * Time.deltaTime;
        rb.MovePosition(rb.position + transform.forward * (speed + (speed * gnomeCoinSys.permanentSpeed)) * Time.deltaTime);
    }
}
