using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSwitch : MonoBehaviour
{
    [Tooltip("0 = disabled, 1 = enabled")] [Range(0, 1)] public int mode;
    [SerializeField] private Button modeButton;
    [SerializeField] private GameObject manufactureButton;
    [SerializeField] private GameObject detonateButton;
    
    void OnEnable()
    {
        switch (mode)
        {
            case 0:
                modeButton.interactable = false;
                manufactureButton.SetActive(true);
                detonateButton.SetActive(false);
                break;
            case 1:
                modeButton.interactable = true;
                manufactureButton.SetActive(false);
                detonateButton.SetActive(true);
                break;
        }
    }
    
    public void EnableModeButton()
    {
        modeButton.interactable = true;
    }

    public void SwitchButtons()
    {
        switch (mode)
        {
            case 0:
                manufactureButton.SetActive(false);
                detonateButton.SetActive(true);
                mode = 1;
                break;
            case 1:
                manufactureButton.SetActive(true);
                detonateButton.SetActive(false);
                mode = 0;
                break;
        }
    }
}
