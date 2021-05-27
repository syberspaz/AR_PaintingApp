using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuOptions : MonoBehaviour
{
    public GameObject[] menus;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick(int menuNumber){
        if(menus[menuNumber] != null){
            Instantiate(menus[menuNumber]);
        }else{
            Debug.Log("Error: Menu item hasn't been assigned yet");
        }
        
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.25f).setEase(LeanTweenType.easeInBack).setOnComplete(destroyMe);
    }

    private void destroyMe(){
        Destroy(gameObject);
    }
}
