using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARMovementControllerManager : MonoBehaviour
{
    [SerializeField]
    private List<ARTransformController> transformControllers;

    public void SetAll(int movementType)
    {
        for (int i = 0; i < transformControllers.Count; i++)
        {
            transformControllers[i].activeMovementType = movementType;
        }
    }
}
