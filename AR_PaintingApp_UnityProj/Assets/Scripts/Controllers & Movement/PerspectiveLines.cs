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
    private Material LineMaterialDotted;

    [SerializeField]
    private Material LineMaterialSolid;

    public bool isDotted;


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

    [Tooltip("Set this to be the ui pannel for the perspective lines")]
    [SerializeField]
    private GameObject linesUI;

    [SerializeField]
    public bool isEnabled = false;

    private void Update()
    {
        //vector that will be rotated
        Vector3 end = new Vector3(0, 0, 0);

        //if no touch, don't need to run the rest of the code
        
        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }
        

         if (isEnabled)
          {
            RaycastHit hit;
            //if touch raycasts onto a plane (valid placement), we can continue with the code that places the lines
            if (Physics.Raycast(Camera.main.ScreenPointToRay(touchPosition), out hit) && linesUI.activeSelf)
            {
                if (hit.transform.gameObject.tag == "UserToolPlane")
                {

                    var hitPos = hit.transform;

                    end = (hitPos.right * LineLength) + hit.point;

                    for (int i = 0; i < NumberOfLines; i++)
                    {
                        //Loops based on number of lines selected by user, takes a vector and rotates it around a pivot
                        //Each line that gets drawn is a line defined by 2 vectors, the center one does not change, but the end
                        //gets rotated around the center and passed into the line drawing function
                        DrawLine(hit.point, end, Color.red);
                        end = RotatePointAroundPivot(end, hit.point, hitPos.forward * RotationAmount);
                    }
                }
            }
          }
    }


    //Function called when drawing a line
    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        //Creates a game object for the lines, and with settings from user/developers
        
        GameObject myLine = new GameObject();
        myLine.gameObject.tag = "PerspectiveLine";
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();

        if (isDotted)
            lr.material = LineMaterialDotted;
        else
            lr.material = LineMaterialSolid;

        lr.textureMode = LineTextureMode.Tile;
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = LineThickness;
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

    //Custom math function to rotate around the pivot
    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

    //Functions for ui controls
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

    public void ToggleEnable()
    {
        isEnabled = !isEnabled;
    }
}
