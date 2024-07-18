using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestReference : MonoBehaviour
{
    public NewBehaviourScript script;
    public TextMeshProUGUI text;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            script.value = 2f;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            script.value = 0f;
        }

        text.text = script.value.ToString();
    }
}
