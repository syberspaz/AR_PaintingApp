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

        transform.position = Vector3.Lerp(KeyPoints[CurrentPreviousPointIndex].position, KeyPoints[CurrentNextPointIndex].position, T);
        transform.rotation = Quaternion.Euler(Vector3.Lerp(KeyPoints[CurrentPreviousPointIndex].rotation.eulerAngles, KeyPoints[CurrentNextPointIndex].rotation.eulerAngles, T));

    }
}
