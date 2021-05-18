using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialSpawn: MonoBehaviour
{
    public RectTransform holdingObject;
    public float holdDuration = 1.0f;
    public GameObject objectToSpawn;
    private float startTime;
    private bool isInstantiated = false;
    private int childCount;
    
    // Start is called before the first frame update
    void Start()
    {
        childCount = this.gameObject.transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0){
            Touch touchInfo = Input.GetTouch(0);
            if(touchInfo.phase == TouchPhase.Stationary && //You're touching & holding
                !isInstantiated){
                
                if(startTime == 0.0f){ //first frame of the hold
                    startTime = Time.time;
                }else{
                    float endTime = Time.time;
                    if(endTime - startTime >= holdDuration){
                        print("Spawn item");
                        startTime = 0.0f;
                        GameObject menu = Instantiate(objectToSpawn);
                        isInstantiated = true;
                        menu.transform.parent = this.gameObject.transform;
                        menu.transform.position = new Vector3(touchInfo.position.x, touchInfo.position.y);
                    }
                }
            }else if(this.gameObject.transform.childCount == childCount){
                startTime = 0.0f;
                isInstantiated = false;
            }else{
                startTime = 0.0f;
            }
        }
    }
}
