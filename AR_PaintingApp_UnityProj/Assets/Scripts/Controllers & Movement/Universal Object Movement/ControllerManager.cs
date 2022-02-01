using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{

    private Camera cameraObject;

    [SerializeField]
    public List<MovementController> movementControllers;

    public bool isSelectionMode; //We will only check for selections if this is on
    //This avoids unwanted selection (1 input that can do 2 things does NOT work well)

    public void Start()
    {
        cameraObject = Camera.main;
    }

    public void Update()
    {


        //Code to select specific objects
        if (isSelectionMode)
        {
            if (!TryGetTouchPosition(out Vector2 touchPos))
            {
                return;
            }
            else
            {
                Ray ray = cameraObject.ScreenPointToRay(touchPos);
                RaycastHit hitObject;

                MovementController hitController;

                if (Physics.Raycast(ray, out hitObject))
                {
                    if (hitObject.collider.gameObject.TryGetComponent<MovementController>(out hitController))
                    {
                        hitController.isSelected = true;


                    }
                }
            }
        }


        //Anything that all movementControllers need to all be updated
        for(int i = 0; i < movementControllers.Count; i++)
        {
            
            movementControllers[i].cameraTransform = cameraObject.transform;
            movementControllers[i].viewMatrix = cameraObject.worldToCameraMatrix;
        
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

    public void DeselectAll()
    {
        for (int i = 0; i < movementControllers.Count; i++)
        {
            movementControllers[i].isSelected = false;
        }
    }

    public void ToggleSelectionMode()
    {
        isSelectionMode = !isSelectionMode;
    }

    public void ToggleAllGyroEnabled()
    {
        for (int i = 0; i < movementControllers.Count; i++)
        {
            movementControllers[i].gyroEnabled = !movementControllers[i].gyroEnabled;
        }

    }
}
