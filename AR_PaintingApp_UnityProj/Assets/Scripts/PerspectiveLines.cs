using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

//Class that goes onto the perspective lines prefab
public class PerspectiveLines : MonoBehaviour
{
    [SerializeField]
    private Material LineMaterial;
    [SerializeField]
    private ARRaycastManager raycastManager;

    [SerializeField]
    private float NumberOfLines;

    [SerializeField]
    private float LineLength;

    [SerializeField]
    private float LineThickness;

    [SerializeField]
    private float RotationAmount;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();



    private void Update()
    {
        //vector that will be rotated
        Vector3 end = new Vector3(0, 0, 0);

        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        if (raycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {


            var hitPos = s_Hits[0].pose;

            end = (hitPos.forward * LineLength) + hitPos.position;

            for (int i = 0; i < NumberOfLines; i++)
            {

                DrawLine(hitPos.position, end, Color.red);
                end = RotatePointAroundPivot(end, hitPos.position, hitPos.up * RotationAmount);
            }



        }
    }

    //Function called when drawing a line
    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.gameObject.tag = "PerspectiveLine";
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = LineMaterial;
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.0001f;
        lr.endWidth = LineThickness;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;


    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

    public void SetNumLines(System.Single num)
    {
        NumberOfLines = num;
    }

    public void SetLineThickness(System.Single thick)
    {
        LineThickness = thick;
    }

    public void SetLineRotation(System.Single rotInDeg)
    {
        RotationAmount = rotInDeg;
    }

    public void SetLineLength(System.Single length)
    {
        LineLength = length;
    }

    public void DestroyAllLines()
    {
        GameObject[] PerspectiveLines = GameObject.FindGameObjectsWithTag("PerspectiveLine");

        for (int i = 0; i < PerspectiveLines.Length; i++)
        {
            GameObject.Destroy(PerspectiveLines[i], 0.0f);
        }

    }
}
