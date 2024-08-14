using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPanels : MonoBehaviour
{
    private GameObject objectToDismiss;
    private GameObject objectToActivate;
    private float dismissalTime;
    private float activationTime;
    private float altDismissalTime;
    private float altActivationTime;
    private Vector3 dismissalLocation;
    private Vector3 altDismissalLocation;
    private Vector3 activationLocation;
    private Vector3 altActivationLocation;
    private int methodRunMode = 0;
    [SerializeField] private WidescreenUIFix uiFixScript;
    public bool useAltPositions;

    private void OnEnable()
    {
        useAltPositions = uiFixScript.isActivated;
    }

    public void SetAsDismissingPanelObject(GameObject panelToDismiss)
    {
        objectToDismiss = panelToDismiss;
    }

    public void SetDismissalTime(float time)
    {
        dismissalTime = time;
    }
    
    public void SetAltDismissalTime(float time)
    {
        altDismissalTime = time;
    }

    public void SetNewDismissalPos(GameObject newDismissalLocation)
    {
        dismissalLocation = newDismissalLocation.transform.position;
    }
    
    public void SetNewAltDismissalPos(GameObject newAltDismissalLocation)
    {
        altDismissalLocation = newAltDismissalLocation.transform.position;
    }

    public void SetAsActivatingPanelObject(GameObject panelToActivate)
    {
        objectToActivate = panelToActivate;
    }

    public void SetActivationTime(float time)
    {
        activationTime = time;
    }
    
    public void SetAltActivationTime(float time)
    {
        altActivationTime = time;
    }

    public void SetNewActivationPos(GameObject newActivationLocation)
    {
        activationLocation = newActivationLocation.transform.position;
    }
    
    public void SetNewAltActivationPos(GameObject newAltActivationLocation)
    {
        altActivationLocation = newAltActivationLocation.transform.position;
    }

    public void SetDismissalValuesThroughScript(GameObject panelToDismiss, float dismissTime, GameObject newDismissalLocation)
    {
        objectToDismiss = panelToDismiss;
        dismissalTime = dismissTime;
        dismissalLocation = newDismissalLocation.transform.position;
    }

    public void SetActivationValuesThroughScript(GameObject panelToActivate, float activeTime, GameObject newActivationLocation)
    {
        objectToActivate = panelToActivate;
        activationTime = activeTime;
        activationLocation = newActivationLocation.transform.position;
    }

    public void ExecuteSmooth(int mode)
    {
        methodRunMode = mode;
        switch (methodRunMode)
        {
            case 0:
                StartCoroutine(SmoothSwitchDismissal());
                StartCoroutine(SmoothSwitchActivation());
                break;
            case 1:
                StartCoroutine(SmoothSwitchDismissal());
                break;
            case 2:
                StartCoroutine(SmoothSwitchActivation());
                break;
        }
        
    }

    IEnumerator SmoothSwitchDismissal()
    {
        switch (useAltPositions)
        {
            case false:
                // Smooth switch operation for the dismissing object
                float dismissalTimeElapsed = 0;
                while (dismissalTimeElapsed < dismissalTime)
                {
                    objectToDismiss.transform.position = Vector3.Lerp(objectToDismiss.transform.position, dismissalLocation, dismissalTimeElapsed / dismissalTime);
                    dismissalTimeElapsed += Time.deltaTime;
                    yield return null;
                }
                objectToDismiss.transform.position = dismissalLocation;
                // Reset to 0 to allow Event systems to run without issues
                methodRunMode = 0;
                break;
            case true:
                // Smooth switch operation for the alt dismissing object
                float altDismissalTimeElapsed = 0;
                while (altDismissalTimeElapsed < altDismissalTime)
                {
                    objectToDismiss.transform.position = Vector3.Lerp(objectToDismiss.transform.position, altDismissalLocation, altDismissalTimeElapsed / altDismissalTime);
                    altDismissalTimeElapsed += Time.deltaTime;
                    yield return null;
                }
                objectToDismiss.transform.position = altDismissalLocation;
                // Reset to 0 to allow Event systems to run without issues
                methodRunMode = 0;
                break;
        }
    }

    IEnumerator SmoothSwitchActivation()
    {
        switch (useAltPositions)
        {
            case false:
                // Smooth switch operation for the activating object
                float activationTimeElapsed = 0;
                while (activationTimeElapsed < activationTime)
                {
                    objectToActivate.transform.position = Vector3.Lerp(objectToActivate.transform.position,
                        activationLocation, activationTimeElapsed / activationTime);
                    activationTimeElapsed += Time.deltaTime;
                    yield return null;
                }
                objectToActivate.transform.position = activationLocation;
                // Reset to 0 to allow Event systems to run without issues
                methodRunMode = 0;
                break;
            case true:
                // Smooth switch operation for the alt activating object
                float altActivationTimeElapsed = 0;
                while (altActivationTimeElapsed < altActivationTime)
                {
                    objectToActivate.transform.position = Vector3.Lerp(objectToActivate.transform.position,
                        altActivationLocation, altActivationTimeElapsed / altActivationTime);
                    altActivationTimeElapsed += Time.deltaTime;
                    yield return null;
                }
                objectToActivate.transform.position = altActivationLocation;
                // Reset to 0 to allow Event systems to run without issues
                methodRunMode = 0;
                break;
        }
    }
}
