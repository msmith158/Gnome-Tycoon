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
    [SerializeField] private GameObject panelPlate;
    [SerializeField] private float sinkingPosAdditionY;
    [SerializeField] private float sinkingTime;
    [SerializeField] private float risingTime;
    private float risingPosY;
    
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

        risingPosY = manufactureButton.transform.position.y;
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
                manufactureButton.GetComponent<Button>().interactable = false;
                StartCoroutine(DoButtonAnimation(manufactureButton, detonateButton));
                break;
            case 1:
                detonateButton.GetComponent<Button>().interactable = true;
                StartCoroutine(DoButtonAnimation(detonateButton, manufactureButton));
                break;
        }
    }

    private IEnumerator DoButtonAnimation(GameObject oldButton, GameObject newButton)
    {
        Vector3 oldButtonNewPos = new Vector3(oldButton.transform.position.x, oldButton.transform.position.y + sinkingPosAdditionY, oldButton.transform.position.z);
        float timeElapsed = 0;
        
        while (timeElapsed < sinkingTime)
        {
            oldButton.transform.position =
                Vector3.Lerp(oldButton.transform.position, oldButtonNewPos, timeElapsed / sinkingTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        manufactureButton.SetActive(false);
        detonateButton.SetActive(true);
        
        Vector3 newButtonNewPos = new Vector3(newButton.transform.position.x, risingPosY, newButton.transform.position.z);
        timeElapsed = 0;
        
        while (timeElapsed < risingTime)
        {
            newButton.transform.position =
                Vector3.Lerp(newButton.transform.position, newButtonNewPos, timeElapsed / sinkingTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
