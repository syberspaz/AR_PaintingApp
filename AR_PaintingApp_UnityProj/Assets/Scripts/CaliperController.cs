using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaliperController : MonoBehaviour
{
    //Animator used to control the motion of the calipers
    private Animator animator;

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
