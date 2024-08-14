using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Above an aspect ratio of 2, Unity's UI seems to cause issues with the placements of UI elements.
// UI elements do not anchor correctly with wider displays and become closer to the centre as a result.
// This script allows you to fix that problem by allowing for a UI element offset when the screen is above a certain aspect ratio.
public class WidescreenUIFix : MonoBehaviour
{
    [SerializeField] private bool runOnEnable = true;
    [SerializeField] private ThresholdType thresholdType;
    [Tooltip("Any aspect ratio above this number will be affected.")][SerializeField] private float aspectRatioThreshold;
    [SerializeField] private List<GameObject> uiToMove = new List<GameObject>();
    [SerializeField] private AdjustmentType adjustmentType;
    [Tooltip("This will ADD to the current UI element position.")][SerializeField] private List<Vector3> newValue = new List<Vector3>();
    [Tooltip("Only assign this if you're using the ObjectAlignment option.")] [SerializeField] private List<GameObject> posPoints = new List<GameObject>();
    public bool isActivated = false;

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
        switch (thresholdType)
        {
            case ThresholdType.Below:
                if (Camera.main.aspect > aspectRatioThreshold)
                {
                    Debug.Log("Your aspect ratio is above the threshold. Current aspect ratio: " + Camera.main.aspect + ".");
                }
                else if (Camera.main.aspect <= aspectRatioThreshold)
                {
                    if (uiToMove.Count != newValue.Count || uiToMove.Count != posPoints.Count)
                    {
                        Debug.Log("You have not set up the widescreen fix script correctly. Please ensure the lists are of equal length.");
                    }
                    for (int i = 0; i < uiToMove.Count; i++)
                    {
                        switch (adjustmentType)
                        {
                            case AdjustmentType.Offset:
                                uiToMove[i].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                                uiToMove[i].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                                uiToMove[i].GetComponent<RectTransform>().position += newValue[i];
                                break;
                            case AdjustmentType.Position:
                                uiToMove[i].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                                uiToMove[i].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                                uiToMove[i].GetComponent<RectTransform>().position = new Vector3(newValue[i].x, newValue[i].y);
                                break;
                            case AdjustmentType.ObjectAlignment:
                                uiToMove[i].transform.position = posPoints[i].transform.position;
                                
                                break;
                        }
                    }
                    isActivated = true;
                    Debug.Log("The widescreen fix script has completed.");
                }
                break;
            case ThresholdType.Above:
                if (Camera.main.aspect <= aspectRatioThreshold)
                {
                    Debug.Log("Your aspect ratio is below the threshold. Current aspect ratio: " + Camera.main.aspect + ".");
                }
                else if (Camera.main.aspect > aspectRatioThreshold)
                {
                    if (uiToMove.Count != newValue.Count)
                    {
                        Debug.Log("You have not set up the widescreen fix script correctly. Please ensure the lists are of equal length.");
                    }
                    else
                    {
                        for (int i = 0; i < uiToMove.Count; i++)
                        {
                            uiToMove[i].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                            uiToMove[i].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                            Debug.Log(newValue[i]);
                            switch (adjustmentType)
                            {
                                case AdjustmentType.Offset:
                                    uiToMove[i].transform.position += newValue[i];
                                    break;
                                case AdjustmentType.Position:
                                    uiToMove[i].GetComponent<Transform>().position = new Vector3(newValue[i].x, newValue[i].y, newValue[i].z);
                                    break;
                            }
                        }
                        isActivated = true;
                        Debug.Log("The widescreen fix script has completed.");
                    }
                }
                break;
        }
    }

    private enum AdjustmentType
    {
        Offset,
        Position,
        ObjectAlignment
    }

    private enum ThresholdType
    {
        Above,
        Below
    }
}
