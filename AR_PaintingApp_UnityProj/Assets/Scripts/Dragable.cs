using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Dragable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private Canvas canvas;
    private GameObject circle;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        circle = this.gameObject;

        rectTransform = circle.GetComponent<RectTransform>();
       
        canvasGroup = circle.GetComponent<CanvasGroup>();

    }
    public void TaskOnClick()
    {
        Debug.Log("Button Clicked");
    }

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
    // Update is called once per frame
    void Update()
    {

    }
}