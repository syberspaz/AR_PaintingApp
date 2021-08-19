using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;

public class PaletteMenuSelection : MonoBehaviour
{

    [SerializeField]
    private GameObject menuObject;

    [SerializeField]
    private TouchAndHoldMenu menuManager;



    // Update is called once per frame
    void Update()
    {

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began && menuObject.activeSelf)
        {
            Debug.Log("Clicked");
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit))
            {
              
               // Debug.Log(hit.transform.gameObject);
                ProcessMenuTouch(hit.transform.gameObject);
            }
            else
            {

                return;
            }
        }
    }

    private void ProcessMenuTouch(GameObject menuItem)
    {
        
        
        PaletteMenuItem item;

        if (menuItem.TryGetComponent<PaletteMenuItem>(out item))
        {
           
            //Palette Menu Item contains the information about what item was selected
            //if this returns false, the raycast isn't hitting the menu

            if (item.ToolType == 1)
            {


                //Caliper Tools
                item.toolGO[0].SetActive(true);
                item.toolGO[1].GetComponent<ValueToggler>().ToggleValue();

                //For the caliper, item 0 is the menu that needs to open for the calipers
                //item 1 is the 3d object for the calipers in the scene

            }
            if (item.ToolType == 2)
            {
              

                //perspective lines
                item.toolGO[0].GetComponent<TogglePlaneDetection>().PlaneDetectionToggle();
                item.toolGO[1].GetComponent<PerspectiveLines>().ToggleEnable();
                item.toolGO[2].SetActive(true);




            }
            if (item.ToolType == 3)
            {
                //image search
                item.toolGO[0].GetComponent<ValueToggler>().ToggleValue();

            }

            if (item.ToolType == 4)
            {
                item.toolGO[0].SetActive(true);
                item.toolGO[1].SetActive(false);
                item.toolGO[2].SetActive(false);
                item.toolGO[3].SetActive(false);



            }

            if (item.ToolType == 5)
            {
                SceneManager.LoadScene("2D_Image_Editor"); //scene 3 is the 2d image editing scene
            }

            if (item.ToolType == 6)
            {
                SceneManager.LoadScene("EyedropperScene");
            }

            menuManager.DisableMenu();
            // menuObject.SetActive(false);




        }
        else
        {
           
        }
        
    }
        
}
