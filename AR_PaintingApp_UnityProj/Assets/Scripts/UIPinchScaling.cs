using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPinchScaling : MonoBehaviour
{
    [SerializeField]
    private Text testText;

    private void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touch0PrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touch1PrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;

            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            testText.text = difference.ToString();


        }
    }
}
