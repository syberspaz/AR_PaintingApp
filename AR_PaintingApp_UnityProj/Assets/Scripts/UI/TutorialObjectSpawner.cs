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

    public GameObject spawningPrefab;

    public bool isMovementControllerSpawner;

    public void SpawnTutorialObject()
    {
      

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

        GameObject newSpawnedObj = Instantiate(spawningPrefab, PlacingLocation, Quaternion.identity); //spawn object

        if (shouldParent)
        {
            newSpawnedObj.transform.parent = Camera.main.transform;
        }

        if (isMovementControllerSpawner)
        {
            GameObject controllerManager = GameObject.Find("Controller Manager");
            controllerManager.GetComponent<ControllerManager>().movementControllers.Add(spawningPrefab.GetComponent<MovementController>());
        }

        GameObject.Destroy(newSpawnedObj, TimeBeforeDestroy);

    }

}
