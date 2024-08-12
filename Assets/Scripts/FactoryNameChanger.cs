using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Assign this script to the Factory Name Text object.
public class FactoryNameChanger : MonoBehaviour
{
    private MainMenuScript menuSys;
    
    void OnEnable()
    {
        menuSys = GameObject.Find("mainMenuSystemHandler").GetComponent<MainMenuScript>();
        if (menuSys.adjective == "")
        {
            GetComponent<TextMeshPro>().text = "Mom & Pop's\n" + "Amazing Gnomes";
        }
        else
        {
            GetComponent<TextMeshPro>().text = "Mom & Pop's\n" + menuSys.adjective + " Gnomes";
        }
    }
}
