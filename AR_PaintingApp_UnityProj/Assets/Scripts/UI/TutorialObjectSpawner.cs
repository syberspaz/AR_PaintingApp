using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObjectSpawner : MonoBehaviour
{
    //Tutorials in this app will often take place in AR space
    //We need a way to spawn those objects slighly in front of the camera and keep them in frame
    //thats what this script is for

    public float distanceFromCamera;

    public bool shouldParent;

    public float TimeBeforeDestroy;

    public string NewMessage;

    public bool isBubble;

    public bool isMovementControllerSpawner;

    public void SpawnTutorialObject(GameObject Gameobj)
    {
        //handles spawning objects in the tutorial menu

        Matrix4x4 viewMatrix = Camera.main.worldToCameraMatrix;

        //Get forward vec
        Vector3 forward;
        forward.x = viewMatrix.m20;
        forward.y = viewMatrix.m21;
        forward.z = viewMatrix.m22;

        forward.Normalize();
        forward *= distanceFromCamera; //apply our distance

        Vector3 PlacingLocation;
        PlacingLocation = Camera.main.transform.position;
        PlacingLocation -= forward; //calculate final spawning position

        if (isBubble)
        {

            PlacingLocation.y -= 2;
        }

        GameObject newSpawnedObj = Instantiate(Gameobj, PlacingLocation, Quaternion.identity); //spawn object

        if (shouldParent)
        {
            newSpawnedObj.transform.parent = Camera.main.transform;
        }

        if (isMovementControllerSpawner)
        {
            GameObject controllerManager = GameObject.Find("Controller Manager");
            controllerManager.GetComponent<ControllerManager>().movementControllers.Add(Gameobj.GetComponent<MovementController>());
        }
        if(isBubble)
        {
            newSpawnedObj.GetComponent<PopUp>().PopupText.text = NewMessage;
        }
        if (TimeBeforeDestroy == 0)
        {
            return;
        }
        else
        GameObject.Destroy(newSpawnedObj, TimeBeforeDestroy);

    }

}
