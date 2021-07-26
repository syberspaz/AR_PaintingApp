using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItemBob : MonoBehaviour
{
    [SerializeField]
    public Vector3 startingPosition;
    [SerializeField]
    public Vector3 endingPosition;

    [SerializeField]
    public Vector3 startingRotation;
    [SerializeField]
    public Vector3 endingRotation;


    private bool isForwardsMove;
    private bool isForwardsRotation;

    [SerializeField]
    public float moveT = 0;

    [SerializeField]
    public float rotationT = 0;

    [SerializeField]
    public bool isRotationEndless;

    [Tooltip("lower number is slower higher number is faster")]
    [SerializeField]
    public float moveSpeed;

    [Tooltip("lower number is slower higher number is faster")]
    [SerializeField]
    public float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        if (moveT >= 1)
        {
            isForwardsMove = false;
            
        }
        if (moveT <= 0)
        {
            isForwardsMove = true;
        }
      
            
        if (isForwardsMove)
        {
            moveT += Time.deltaTime * moveSpeed;
        }
        else
        {
            moveT -= Time.deltaTime * moveSpeed;
        }



        if (rotationT >= 1)
        {
            if (isRotationEndless)
            {
                rotationT = 0;
            }
            isForwardsRotation = false;
        }
        if (rotationT <= 0)
        {
            isForwardsRotation = true;
        }


        if (isForwardsRotation)
        {
            rotationT += Time.deltaTime * rotationSpeed;
        }
        else
        {
            rotationT -= Time.deltaTime * rotationSpeed;


        }

        transform.localPosition = Vector3.Lerp(startingPosition, endingPosition, moveT);
        transform.rotation = Quaternion.Euler(Vector3.Lerp(startingRotation, endingRotation, rotationT));



    }
}
