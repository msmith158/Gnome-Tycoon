using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Above an aspect ratio of 2, Unity's UI seems to cause issues with the placements of UI elements.
// UI elements do not anchor correctly with wider displays and become closer to the centre as a result.
// This script allows you to fix that problem by allowing for a UI element offset when the screen is above a certain aspect ratio.
public class WidescreenUIFix : MonoBehaviour
{
    [SerializeField] private bool runOnEnable = true;
    [Tooltip("Any aspect ratio above this number will be affected.")][SerializeField] private float aspectRatioThreshold;
    [SerializeField] private List<GameObject> uiToMove = new List<GameObject>();
    [Tooltip("This will ADD to the current UI element position.")][SerializeField] private List<Vector3> offsetAdjustment = new List<Vector3>();

    private void OnEnable()
    {
        switch (runOnEnable)
        {
            case true:
                FixUI();
                break;
        }
    }

    private void FixUI()
    {
        if (Camera.main.aspect <= aspectRatioThreshold)
        {
            Debug.Log("Your aspect ratio is below the threshold. Current aspect ratio: " + Camera.main.aspect + ".");
        }
        else if (Camera.main.aspect > aspectRatioThreshold)
        {
            if (uiToMove.Count != offsetAdjustment.Count)
            {
                Debug.Log("You have not set up the widescreen fix script correctly. Please ensure the lists are of equal length.");
            }
            else
            {
                for (int i = 0; i < uiToMove.Count; i++)
                {
                    uiToMove[i].transform.position += offsetAdjustment[i];
                }
                Debug.Log("The widescreen fix script has completed.");
            }
        }
    }
}
