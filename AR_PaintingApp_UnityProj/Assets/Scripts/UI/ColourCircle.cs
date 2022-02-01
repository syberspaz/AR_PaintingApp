using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ColourCircle : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Canvas canvas;
    private GameObject circle;
    private Button button;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private bool isAnimated = false;
    public int Num_Steps = 10;
    private int stepCounter;
    private Vector3 newLocation;
    private Vector3 oldLocation;
    private float stepSize;
    private Vector2 marchDir;

    public float journeyTime = 1.0f;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        circle = this.gameObject;
        
        button = circle.GetComponent<Button>();
        rectTransform = circle.GetComponent<RectTransform>();
        button.onClick.AddListener(TaskOnClick);
        canvasGroup = circle.GetComponent<CanvasGroup>();
        
    }

    public void moveTo(Vector3 newLoc){
        isAnimated = true;
        stepCounter = 0;
        newLocation = newLoc;
        
        oldLocation = rectTransform.anchoredPosition;


        /*
        stepSize = Vector3.Distance(oldLocation, newLocation) / Num_Steps;
        Vector3 temp = Vector3.Normalize(newLocation - oldLocation);
        marchDir = new Vector2(temp.x, temp.y);
        */

        startTime = Time.time;
    }

    public void FixedUpdate(){
        float fractComplete = (Time.time - startTime) / journeyTime;
        if(isAnimated){
            /*
            rectTransform.anchoredPosition += (stepSize) * marchDir;
            stepCounter ++;*/
            
            rectTransform.anchoredPosition = Vector3.Slerp(oldLocation, newLocation, fractComplete);

        }
        /*
        if(stepCounter == Num_Steps){
            stepCounter = 0;
            isAnimated = false;
        }*/
        if(fractComplete >= 1.0){
            isAnimated = false;
        }
    }
    public void TaskOnClick(){
        Debug.Log("Button Clicked");
    }

    public void OnBeginDrag(PointerEventData eventData){
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData){
        if(!isAnimated){
            GameObject parent = this.gameObject.transform.parent.gameObject;

            //rectTransform.anchoredPosition += eventData.delta / (canvas.scaleFactor * parent.GetComponent<RectTransform>().localScale);
            
            Vector3 temp = rectTransform.anchoredPosition + eventData.delta / (canvas.scaleFactor * parent.GetComponent<RectTransform>().localScale);
            Vector3 slerpResult = Vector3.Slerp(rectTransform.anchoredPosition, temp, 0.9f);
            rectTransform.anchoredPosition = new Vector2(slerpResult.x, slerpResult.y);

            
        }
        
    }

    public void OnEndDrag(PointerEventData eventData){
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1.0f;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
