// ## THIS SCRIPT HAS BEEN DEPRECATED AS IT IS NO LONGER NEEDED.
// ## IF THIS SCRIPT NEEDS TO BE USED AGAIN, THE PRODUCTION LINE REFERENCE NEEDS TO BE FIXED.

/*using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// During testing, I found that a certain combination of upgrades caused the objects to clash and stop moving.
// This script will allow the player to clear all objects in the case that this happens.
// This script is to be attached to an Event System (e.g. a button).
public class ClearObjects : MonoBehaviour
{
    [SerializeField] private List<FinalDispenser> dispensers = new List<FinalDispenser>();
    [SerializeField] private FinalFactorySystem sys;
    
    // This will temporarily unfreeze time, clear all objects and then freeze it again
    public void ClearAllFrozenConveyors()
    {
        Time.timeScale = 1.0f;
        
        for (int i = 0; i < sys.productionLines.Count; i++)
        {
            for (int j = 0; j < dispensers[i].objectsList.Count; i++)
            {
                Destroy(dispensers[i].objectsList[j]);
            }
        }

        Time.timeScale = 0.0f;
    }

    // This will just clear all objects without attempting to unfreeze time
    public void ClearAllUnfrozenConveyors()
    {
        for (int i = 0; i < sys.productionLines.Count; i++)
        {
            for (int j = 0; j < dispensers[i].objectsList.Count; i++)
            {
                Destroy(dispensers[i].objectsList[j]);
            }
        }
    }
}*/
