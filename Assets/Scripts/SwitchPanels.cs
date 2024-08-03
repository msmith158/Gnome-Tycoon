using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPanels : MonoBehaviour
{
    private GameObject objectToDismiss;
    private GameObject objectToActivate;
    private float dismissalTime;
    private float activationTime;
    private Vector3 dismissalLocation;
    private Vector3 activationLocation;

    public void SetAsDismissingPanelObject(GameObject panelToDismiss)
    {
        objectToDismiss = panelToDismiss;
    }

    public void SetDismissalTime(float time)
    {
        dismissalTime = time;
    }

    public void AdjustDismissalPositionY(float locationY)
    {
        dismissalLocation = new Vector3(objectToDismiss.transform.position.x, objectToDismiss.transform.position.y + locationY, objectToDismiss.transform.position.z);
    }

    public void SetAsActivatingPanelObject(GameObject panelToActivate)
    {
        objectToActivate = panelToActivate;
    }

    public void SetActivationTime(float time)
    {
        activationTime = time;
    }

    public void AdjustActivationPositionY(float locationY)
    {
        activationLocation = new Vector3(objectToActivate.transform.position.x, objectToActivate.transform.position.y + locationY, objectToActivate.transform.position.z);
    }

    public void ExecuteSmooth()
    {
        StartCoroutine(SmoothSwitchDismissal());
        StartCoroutine(SmoothSwitchActivation());
    }

    IEnumerator SmoothSwitchDismissal()
    {
        // Smooth switch operation for the dismissing object
        float dismissalTimeElapsed = 0;
        while (dismissalTimeElapsed < dismissalTime)
        {
            objectToDismiss.transform.position = Vector3.Lerp(objectToDismiss.transform.position, dismissalLocation, dismissalTimeElapsed / dismissalTime);
            dismissalTimeElapsed += Time.deltaTime;
            yield return null;
        }
        objectToDismiss.transform.position = dismissalLocation;
    }

    IEnumerator SmoothSwitchActivation()
    {
        // Smooth switch operation for the activating object
        float activationTimeElapsed = 0;
        while (activationTimeElapsed < activationTime)
        {
            objectToActivate.transform.position = Vector3.Lerp(objectToActivate.transform.position, activationLocation, activationTimeElapsed / activationTime);
            activationTimeElapsed += Time.deltaTime;
            yield return null;
        }
        objectToActivate.transform.position = activationLocation;
    }
}
