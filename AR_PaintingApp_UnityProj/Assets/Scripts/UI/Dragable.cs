using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Dragable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //This script exists to have 2d ui space objects be dragged around
    //Mostly just used for user selected corners but many possible applications

    [SerializeField]
    private Canvas canvas;
    private GameObject circle;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    void Start()
    {
        circle = this.gameObject;

        rectTransform = circle.GetComponent<RectTransform>();
       
        canvasGroup = circle.GetComponent<CanvasGroup>();

    }


    //These functions are all the eventsystem calls, they just move the ui element according to the drag delta position
    public void OnBeginDrag(PointerEventData eventData)
    {
       
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
      
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1.0f;
    }
  
}