using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARMovementControllerManager : MonoBehaviour
{
    [SerializeField]
    private List<ARTransformController> transformControllers;

    //Takes input from slider, and applies to all ARTransformControllers given to this script
    public void SetAll(System.Single movementType)
    {
        for (int i = 0; i < transformControllers.Count; i++)
        {
            transformControllers[i].activeMovementType = (int)movementType;
        }
    }
}
