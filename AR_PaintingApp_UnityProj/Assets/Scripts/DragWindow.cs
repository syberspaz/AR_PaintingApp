using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour, IDragHandler
{
    //Handles dragging the windows around in AR World Space

    [SerializeField] private float ScaleFactor;

    //Our main transform for the windows
    [SerializeField] private RectTransform rect;

    [SerializeField] private GameObject canvas;

    public void Start()
    {
        Debug.Log(canvas.GetComponent<RectTransform>().localScale.x);
        Debug.Log(canvas.GetComponent<RectTransform>().localScale.y);
    }

    public void OnDrag(PointerEventData eventData)
    {

        rect.anchoredPosition += eventData.delta.normalized * canvas.GetComponent<RectTransform>().localScale * Screen.width;
        Debug.Log(eventData.delta.normalized * canvas.GetComponent<RectTransform>().localScale.x * Screen.width);

    }

}
