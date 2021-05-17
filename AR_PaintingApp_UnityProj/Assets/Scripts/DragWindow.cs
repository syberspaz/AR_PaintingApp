using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragWindow : MonoBehaviour, IDragHandler
{
    //Handles dragging the windows around in AR World Space

    [SerializeField] private float ScaleFactor;

    //Our main transform for the windows
    [SerializeField] private RectTransform rect;

    [SerializeField] private GameObject canvas;

    [SerializeField]
    private Camera camera;


    public void OnDrag(PointerEventData eventData)
    {

        Vector3 up;
        up.x = camera.worldToCameraMatrix.m10;
        up.y = camera.worldToCameraMatrix.m11;
        up.z = camera.worldToCameraMatrix.m12;

        //get the values for camera forward (Taken from the view matrix)
        Vector3 forward;
        forward.x = camera.worldToCameraMatrix.m20;
        forward.y = camera.worldToCameraMatrix.m21;
        forward.z = camera.worldToCameraMatrix.m22;

        Vector3 sideways = Vector3.Cross(up, forward);

        Vector2 touchInput = eventData.delta;

        touchInput.x = touchInput.x / Screen.width;
        touchInput.y = touchInput.y / Screen.height;

        Vector3 Movement = new Vector3(0, 0, 0);

        Movement += up * touchInput.y;
        Movement += sideways * touchInput.x;

    
            rect.position += Movement;
        




    }

}
