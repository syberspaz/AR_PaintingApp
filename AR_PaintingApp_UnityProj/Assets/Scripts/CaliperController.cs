using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaliperController : MonoBehaviour
{
    // Update is called once per frame

    private Animator animator;

    [SerializeField]
    Camera camera;

    //this float controls how open the calipers are, 0 is closed and the closer to 1 the more open
    public float Openness;
    public void Update()
    {
        animator = this.GetComponent<Animator>();

        animator.SetFloat("DistanceBetweenEnds", Openness);

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Stationary)
        {
            this.transform.parent = camera.transform;
        }
        else
        {
            this.transform.parent = null;
        }

    }

    public void SetOpenness(float value)
    {
        Openness = value;
    }

    public void SetScale(float scale)
    {
        this.transform.localScale = new Vector3(scale, scale, scale);

        


    }

}
