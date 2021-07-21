using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
public class PaletteMenuSelection : MonoBehaviour
{


    [SerializeField]
    Text text;
   
    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clicked");
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                text.text = "Processing Touch";
               // Debug.Log(hit.transform.gameObject);
                ProcessMenuTouch(hit.transform.gameObject);
            }
            else
            {
                text.text = "No Hit";
                return;
            }
        }
    }

    private void ProcessMenuTouch(GameObject menuItem)
    {
        text.text = "ProcessMenuTouchFuncCalled";
        
        PaletteMenuItem item;

        if (menuItem.TryGetComponent<PaletteMenuItem>(out item))
        {
            //Palette Menu Item contains the information about what item was selected
            //if this returns false, the raycast isn't hitting the menu

            if (item.ToolType == 1)
            {
                text.text = "Caliper Menu";

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

            }

        }
        else
        {
            text.text = "Hit, but not a menu item, ignore";
        }
        
    }
        
}
