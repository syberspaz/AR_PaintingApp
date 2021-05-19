using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPinchScaling : MonoBehaviour
{
    [SerializeField]
    private Text debugText;

    [SerializeField]
    EventSystem eventSystem;

    private GraphicRaycaster graphicRaycaster;

    private PointerEventData m_PointerEventData;

    private void Start()
    {
        graphicRaycaster = gameObject.GetComponent<GraphicRaycaster>();
    }



    private void Update()
    {


        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touch0PrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touch1PrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;

            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            m_PointerEventData = new PointerEventData(eventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = touchZero.position;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            graphicRaycaster.Raycast(m_PointerEventData, results);

            foreach (RaycastResult result in results)
            {
                if(result.gameObject.GetComponent<Pinchable>() != null)
                {
                    Vector3 newScale = result.gameObject.GetComponent<RectTransform>().localScale;
                    newScale.x += difference / Screen.width;
                    newScale.y += difference / Screen.height;
                    result.gameObject.GetComponent<RectTransform>().localScale = newScale;
                }
            }


        }
    }
}
