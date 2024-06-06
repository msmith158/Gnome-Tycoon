using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeObject : MonoBehaviour
{
    public float value;
    public float initialValue;

    private void Start()
    {
        initialValue = value;
    }
}
