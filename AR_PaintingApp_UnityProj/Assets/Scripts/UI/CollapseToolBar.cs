using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseToolBar : MonoBehaviour
{
    //tutorial scene & explore mode both have a scrolling tool bar
    //this script just helps for collapsing it

    [SerializeField]
    GameObject ScrollBarObject;

    [SerializeField]
    Canvas canvas;

    bool isOpen = true;

   public void CollapseButtonPress()
    {
        isOpen = !isOpen; //toggle

        ScrollBarObject.SetActive(isOpen);

        if(isOpen)
        {
            RectTransform thisRect = gameObject.GetComponent<RectTransform>();
            thisRect.anchoredPosition = new Vector2(thisRect.rect.x, 300);
        }
        else
        {
           

            RectTransform thisRect = gameObject.GetComponent<RectTransform>();
            thisRect.anchoredPosition = new Vector2(thisRect.rect.x, 20);
        }


    }
}
