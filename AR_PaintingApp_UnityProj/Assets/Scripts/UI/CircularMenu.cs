using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class CircularMenu : MonoBehaviour, IDropHandler
{
    public float radius = 100.0f;
    static public int NumItems =  5;
    public int startSceneIndex;
    //public GameObject cube;
    public string[] scenesNames = new string[NumItems];
    private Color[] colours;
    private Vector2 cartesianToPolar(Vector3 pos){
        //We want atan2 instead of atan b/c atan2 handles when x = 0
        float theta = Mathf.Atan2(pos.y, pos.x);
        float radius = Mathf.Sqrt(pos.y * pos.y  + pos.x * pos.x);
        return new Vector2(theta, radius);
    }  

    private Vector3 polarToCartesian(float theta, float radius, float z) {
        float x = Mathf.Cos(theta) * radius;
        float y = Mathf.Sin(theta) * radius;
        return new Vector3(x, y, z);
    }

    void Awake(){
        DontDestroyOnLoad(this.gameObject.transform.parent.gameObject);
        // SceneManager.LoadScene(scenesNames[startSceneIndex], LoadSceneMode.Single);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        //cube.GetComponent<Renderer>().material.color = Color.red;
        colours = new Color[NumItems];
        colours[0] = Color.red;
        colours[1] = Color.blue;
        colours[2] = Color.green;
        colours[3] = Color.yellow;
        colours[4] = Color.magenta;
    }

    // Update is called once per frame
    void Update()
    {   
        float angle = (-Mathf.PI);
        float angleDelta = (2 * Mathf.PI) / NumItems;
        Vector3 center = this.transform.position;
        for(int i = 0; i < NumItems; i ++){
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Debug.DrawLine(center, center + new Vector3(x, y, 0.0f), Color.red, 5.0f);
            angle += angleDelta;
        }
    }

    public void OnDrop(PointerEventData eventData){
        Vector3 center = this.transform.position;
        Vector3 colourWheelPos = eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition;

        Vector2 polarCoord = cartesianToPolar(colourWheelPos);
        //We want atan2 instead of atan b/c atan2 handles when x = 0
        //float theta = Mathf.Atan2(colourWheelPos.y, colourWheelPos.x);
        Debug.Log(polarCoord.x);
    
        if(eventData.pointerDrag != null){
            float circleAngle = (-Mathf.PI);
            float angleDelta = (2 * Mathf.PI) / NumItems;
            for(int i = 0; i < NumItems; i ++){
                if(polarCoord.x >= circleAngle && polarCoord.x < circleAngle + angleDelta){
                    float midAngle = circleAngle + (angleDelta / 2.0f);
                    
                    Vector3 cartCoord = polarToCartesian(midAngle, radius, 0);
                    
                    eventData.pointerDrag.GetComponent<ColourCircle>().moveTo(cartCoord);
                    

                    Debug.Log(scenesNames[i]);
                    
                    //if going back to main scene, destroy this object
                    if (scenesNames[i] == "MainScreen")
                    {
                        Destroy((this.gameObject.transform.parent.gameObject));
                    }


                    SceneManager.LoadScene(scenesNames[i], LoadSceneMode.Single);

                }
                circleAngle += angleDelta;
            }
            
        }
        
    }
}
