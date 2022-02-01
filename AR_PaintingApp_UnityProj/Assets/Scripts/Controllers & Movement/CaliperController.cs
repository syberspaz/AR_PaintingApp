using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CaliperController : MonoBehaviour
{
    //Animator used to control the motion of the calipers
    private Animator animator;

    public Text distanceText;

    public Transform tip1;
    public Transform tip2;

    //this float controls how open the calipers are, 0 is closed and the closer to 1 the more open
    public float Openness;

    //Automatically gets the animator component
    public void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    //Updates the animator every frame
    public void Update()
    {
        animator.SetFloat("DistanceBetweenEnds", Openness);

        //calculate distance between tips and update text


        float distance = Vector3.Distance(tip1.position, tip2.position);

        distance *= 100; //multiply by 100 to bring to cm from m

        
        distanceText.text = distance.ToString() + " cm";

        float pinchAmount = 0f;

        if (Mathf.Abs(DetectTouchMovement.pinchDistanceDelta) > 0)
        { // zoom
            pinchAmount = DetectTouchMovement.pinchDistanceDelta;
        }

       

        Openness += (pinchAmount * Time.deltaTime) / 20;


        if (Input.touchCount > 2)
        {
            transform.localScale += (pinchAmount * Vector3.one * (Time.deltaTime)) / 3;
            //makes sure the caliper doesn't become comicaly large

            Vector3 newSize;
            newSize.x = Mathf.Clamp(transform.localScale.x, 0.2f, 1);
            newSize.y = Mathf.Clamp(transform.localScale.y, 0.2f, 1);
            newSize.z = Mathf.Clamp(transform.localScale.z, 0.2f, 1);

            transform.localScale = newSize;

        }

    }


    //2 functions after this are for the sliders
    public void SetOpenness(float value)
    {
        Openness = value;
    }

    public void SetScale(float scale)
    {
        this.transform.localScale = new Vector3(scale, scale, scale);
    }

}
