using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
public class ARMovementControllerManager : MonoBehaviour
{
    [SerializeField]
    public List<ARTransformController> transformControllers;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private GameObject testPlacePrefab;


    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    //Takes input from slider, and applies to all ARTransformControllers given to this script
    public void SetMovementTypeAll(System.Single movementType)
    {
        for (int i = 0; i < transformControllers.Count; i++)
        {
            transformControllers[i].activeMovementType = (int)movementType;
        }
    }

    public void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPos))
        {
            return;
        }
        else
        {
            Ray ray = camera.ScreenPointToRay(touchPos);
            RaycastHit hitObject;

            ARTransformController hitController;

            if(Physics.Raycast(ray, out hitObject))
            {
               if (hitObject.collider.gameObject.TryGetComponent<ARTransformController>(out hitController))
                {
                    hitController.isActive = !hitController.isActive;
                   

                }
            }
        }



    }

    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;


    }

}
