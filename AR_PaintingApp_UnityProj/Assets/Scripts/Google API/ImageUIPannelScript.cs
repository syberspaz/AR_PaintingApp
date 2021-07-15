using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageUIPannelScript : MonoBehaviour
{
    //Script to manage the UI for the window used for reference image search

    //This is the large pannel to display what image is selected
    [SerializeField]
    private RawImage selectionDisplay;

    [SerializeField]
    private ImageSearch imageSearchObject; //To get information about images and buttons, we need this

    [SerializeField]
    private GameObject referenceImageQuadPrefab;

    [SerializeField]
    private ControllerManager transformController;

    public void Start()
    {
        selectionDisplay.texture = null;   
    }

    public void OnImageButtonPress(int ButtonNumber)
    {
        selectionDisplay.texture = imageSearchObject.imagePannels[ButtonNumber].texture;
    }

    public void PlaceInScene()
    {
        if (selectionDisplay.texture != null) //Check to make sure there is a valid selection
        {
            GameObject newPlane = Instantiate(referenceImageQuadPrefab, new Vector3(-1,-1,0), Quaternion.identity); //spawns at 0,0,0
            newPlane.tag = "ReferenceImageQuad";
            newPlane.GetComponent<Renderer>().material.mainTexture = selectionDisplay.texture;
            transformController.movementControllers.Add(newPlane.GetComponent<MovementController>());
        }
        else
        {
            Debug.Log("No Image Selected");
        }
    }

    public void RemoveAllReferenceImages()
    {
        GameObject[] ImagePannels;
        ImagePannels = GameObject.FindGameObjectsWithTag("ReferenceImageQuad");

        for (int i = 0; i < ImagePannels.Length; i ++)
        {
            transformController.movementControllers.Remove(ImagePannels[i].GetComponent<MovementController>());
           GameObject.Destroy(ImagePannels[i]);
        }
    }



}
