using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialSpawn: MonoBehaviour
{
    //These are the current UI objects that are on the screen.
    public GameObject invalidObjectOne;
    private RectTransform invalidHoldOne;
    public Canvas canvas;
    public GameObject invalidObjectTwo;
    private RectTransform invalidHoldTwo;
    public float holdDuration = 1.0f;
    public GameObject objectToSpawn;
    private float startTime;
    private bool isVisible = false;
    private int childCount;
    
    // Start is called before the first frame update
    void Start()
    {
        childCount = this.gameObject.transform.childCount;
        invalidHoldOne = invalidObjectOne.GetComponent<RectTransform>();
        invalidHoldTwo = invalidObjectTwo.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0){
            Touch touchInfo = Input.GetTouch(0);
            if(touchInfo.phase == TouchPhase.Stationary && //You're touching & holding
                //We don't want to spawn the radial menu on top of other UI elements
                ! RectTransformUtility.RectangleContainsScreenPoint(invalidHoldOne, touchInfo.position) &&
                ! RectTransformUtility.RectangleContainsScreenPoint(invalidHoldTwo, touchInfo.position)&& 
                //Only spawn when the radial menu doesn't exist
                !isVisible){
                
                if(startTime == 0.0f){ //first frame of the hold
                    startTime = Time.time;
                }else{
                    float endTime = Time.time;
                    if(endTime - startTime >= holdDuration){
                        print("Spawn item");
                        startTime = 0.0f;
                        objectToSpawn.SetActive(true);
                        isVisible = true;
                        objectToSpawn.transform.parent = canvas.gameObject.transform;
                        objectToSpawn.transform.position = new Vector3(touchInfo.position.x, touchInfo.position.y);
                        objectToSpawn.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                        LeanTween.scale(objectToSpawn, new Vector3(1.0f, 1.0f, 1.0f), 0.25f).setEase(LeanTweenType.easeOutBack);
                    }
                }
            }else if(this.gameObject.transform.childCount == childCount){
                startTime = 0.0f;
                isVisible = false;
            }else{
                startTime = 0.0f;
            }
        }
    }
}
