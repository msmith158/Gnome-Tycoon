using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ModeSwitch : MonoBehaviour
{
    [Header("Values")]
    [Tooltip("0 = disabled, 1 = enabled")][Range(0, 1)] public int mode;
    [SerializeField] private float sinkingPosAdditionY;
    [SerializeField] private float sinkingTime;
    [SerializeField] private float sinkingTimePostDelay;
    [SerializeField] private float risingTime;
    private float risingPosY;

    [Header("Object References")]
    [SerializeField] private Button modeButton;
    [SerializeField] private GameObject manufactureButton;
    [SerializeField] private GameObject detonateButton;
    [SerializeField] private GameObject panelPlate;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip machineWhirr1;
    [SerializeField] private AudioClip machineWhirr2;
    [SerializeField] private AudioClip machineThud;
    
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
                StartCoroutine(DoButtonAnimation(manufactureButton, detonateButton));
                StartCoroutine(DoPanelPlateAnimation());
                mode = 1;
                break;
            case 1:
                StartCoroutine(DoButtonAnimation(detonateButton, manufactureButton));
                StartCoroutine(DoPanelPlateAnimation());
                mode = 0;
                break;
        }
    }

    private IEnumerator DoButtonAnimation(GameObject oldButton, GameObject newButton)
    {
        modeButton.GetComponent<Button>().interactable = false;
        oldButton.GetComponent<Button>().interactable = false;

        Vector3 buttonOldPos = oldButton.transform.position;
        Vector3 buttonNewPos = new Vector3(oldButton.transform.position.x, oldButton.transform.position.y + sinkingPosAdditionY, oldButton.transform.position.z);
        float timeElapsed = 0;

        sfxSource.clip = machineWhirr1;
        sfxSource.Play();

        while (timeElapsed < sinkingTime)
        {
            float t = timeElapsed / sinkingTime;
            Vector3 currentPosition = Vector3.Lerp(buttonOldPos, buttonNewPos, t);
            oldButton.transform.position = currentPosition;
            timeElapsed += Time.deltaTime;
            yield return null;

            if (!sfxSource.isPlaying && sfxSource.clip == machineWhirr1)
            {
                sfxSource.clip = machineWhirr2;
                sfxSource.loop = true;
                sfxSource.Play();
            }
        }
        oldButton.transform.position = buttonNewPos;
        sfxSource.loop = false;
        sfxSource.Stop();
        sfxSource.PlayOneShot(machineThud);

        yield return new WaitForSeconds(sinkingTimePostDelay);

        oldButton.SetActive(false);
        newButton.SetActive(true);
        newButton.GetComponent<Button>().interactable = false;

        //buttonOldPos = newButton.transform.position;
        //buttonNewPos = new Vector3(newButton.transform.position.x, newButton.transform.position.y + sinkingPosAdditionY, newButton.transform.position.z);
        timeElapsed = 0;

        sfxSource.clip = machineWhirr1;
        sfxSource.Play();
        
        while (timeElapsed < risingTime)
        {
            float t = timeElapsed / risingTime;
            Vector3 currentPosition = Vector3.Lerp(buttonNewPos, buttonOldPos, t);
            newButton.transform.position = currentPosition;
            timeElapsed += Time.deltaTime;
            yield return null;

            if (!sfxSource.isPlaying && sfxSource.clip == machineWhirr1)
            {
                sfxSource.clip = machineWhirr2;
                sfxSource.loop = true;
                sfxSource.Play();
            }
        }
        newButton.transform.position = buttonOldPos;
        sfxSource.loop = false;
        sfxSource.Stop();
        sfxSource.PlayOneShot(machineThud);

        newButton.GetComponent<Button>().interactable = true;
        modeButton.GetComponent<Button>().interactable = true;
    }

    private IEnumerator DoPanelPlateAnimation()
    {
        Vector3 panelPlateOldPos = panelPlate.transform.position;
        Vector3 panelPlateNewPos = new Vector3(panelPlate.transform.position.x, panelPlate.transform.position.y + sinkingPosAdditionY, panelPlate.transform.position.z);
        float timeElapsed = 0;

        while (timeElapsed < sinkingTime)
        {
            float t = timeElapsed / sinkingTime;
            Vector3 currentPosition = Vector3.Lerp(panelPlateOldPos, panelPlateNewPos, t);
            panelPlate.transform.position = currentPosition;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        panelPlate.transform.position = panelPlateNewPos;

        yield return new WaitForSeconds(sinkingTimePostDelay);

        //panelPlateOldPos = panelPlate.transform.position;
        //panelPlateNewPos = new Vector3(panelPlate.transform.position.x, panelPlate.transform.position.y + sinkingPosAdditionY, panelPlate.transform.position.z);
        timeElapsed = 0;

        while (timeElapsed < risingTime)
        {
            float t = timeElapsed / risingTime;
            Vector3 currentPosition = Vector3.Lerp(panelPlateNewPos, panelPlateOldPos, t);
            panelPlate.transform.position = currentPosition;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        panelPlate.transform.position = panelPlateOldPos;
    }
}
