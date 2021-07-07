using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObjectSpawner : MonoBehaviour
{
    //Tutorials in this app will often take place in AR space
    //We need a way to spawn those objects slighly in front of the camera and keep them in frame
    //thats what this script is for

    public float distanceFromCamera;

    private ControllerManager controllerManager;

    [SerializeField]
    GameObject GreenBubble;

    [SerializeField]
    GameObject RedBubble;

    public bool isRedBubble;

    private void Start()
    {
        controllerManager = GameObject.FindGameObjectWithTag("ControllerManager").GetComponent<ControllerManager>();
    }

    //this is the blank one
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

        GameObject newSpawnedObj = Instantiate(Gameobj, PlacingLocation, Quaternion.identity); //spawn object
    }

    //For spawning tutorial bubbles
    public void SpawnTutorialBubble(string Message)
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

        GameObject newSpawnedObj;

        if (isRedBubble)
        {
             newSpawnedObj = Instantiate(RedBubble, PlacingLocation, Quaternion.identity); //spawn object
        }
        else
        {
             newSpawnedObj = Instantiate(GreenBubble, PlacingLocation, Quaternion.identity); //spawn object
        }


        PopUp pop = newSpawnedObj.GetComponent<PopUp>();
        pop.ChangeText(Message);
    }

    public void SpawnMovementController(GameObject Gameobj)
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

        GameObject newSpawnedObj = Instantiate(Gameobj, PlacingLocation, Quaternion.identity); //spawn object

        MovementController controller = newSpawnedObj.GetComponent<MovementController>();
        
        if (controller != null)
        {
            controllerManager.movementControllers.Add(controller);
        }
        
    }

}
