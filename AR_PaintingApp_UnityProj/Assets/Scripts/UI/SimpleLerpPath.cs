using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLerpPath : MonoBehaviour
{
    [SerializeField]
    private Transform[] KeyPoints;

    public float T = 0;

    public float speedMultiplier;

    public int CurrentPreviousPointIndex = 0; //default value
    public int CurrentNextPointIndex = 1;

    public int ArraySizeDisplay;

    public bool AffectPosition;
    public bool AffectRotation;
    public bool AffectScale;

    // Update is called once per frame
    void Update()
    {
        int ArraySize = KeyPoints.Length;
        ArraySizeDisplay = ArraySize;

        T += Time.deltaTime;

        if (T >= 1f)
        {
            T = 0f; //reset T after getting to the end of a path

            if (CurrentNextPointIndex < ArraySize) //check if Next point is last in the path, if not move forward the indices by 1
            {
                CurrentPreviousPointIndex++;
                CurrentNextPointIndex++;
            }
             if (CurrentNextPointIndex == ArraySize)
            {
                CurrentPreviousPointIndex = CurrentNextPointIndex;
                CurrentNextPointIndex = 0;
                //CurrentPreviousPointIndex = ArraySize;
            }
            if (CurrentPreviousPointIndex == ArraySize)
            {
                CurrentPreviousPointIndex = 0;
                CurrentNextPointIndex = 1;
            }
        }

        if (AffectPosition)
        transform.position = Vector3.Lerp(KeyPoints[CurrentPreviousPointIndex].position, KeyPoints[CurrentNextPointIndex].position, T);
        if (AffectRotation)
        transform.rotation = Quaternion.Euler(Vector3.Lerp(KeyPoints[CurrentPreviousPointIndex].rotation.eulerAngles, KeyPoints[CurrentNextPointIndex].rotation.eulerAngles, T));
        if (AffectScale)
        transform.localScale = Vector3.Lerp(KeyPoints[CurrentPreviousPointIndex].lossyScale, KeyPoints[CurrentNextPointIndex].lossyScale, T);

    }
}
