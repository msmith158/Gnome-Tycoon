using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypePrestige : MonoBehaviour
{
    public PrestigeType prestigeType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum PrestigeType
    {
        Prestige1,
        Prestige2,
        Prestige3,
        Prestige4,
        Prestige5,
        FinalPrestige
    }
}
