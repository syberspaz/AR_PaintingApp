using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PaletteMenuSelection : MonoBehaviour
{


    [SerializeField]
    Text text;
   
    // Update is called once per frame
    void Update()
    {
        Touch touch = Input.GetTouch(0);

        RaycastHit hit;

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit))
            return;
        else
        {
            text.text = "Processing Touch";
            ProcessMenuTouch(hit.rigidbody.gameObject);
        }
    }

    private void ProcessMenuTouch(GameObject menuItem)
    {
        text.text = "Hit, going through to check";

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

            }
            if (item.ToolType == 3)
            {
                //image search

            }

        }
        else
        {
            Debug.LogWarning("Invalid Selection");
        }
    }

}
