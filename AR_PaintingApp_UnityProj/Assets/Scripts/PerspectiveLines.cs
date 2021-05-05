using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveLines : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 start = new Vector3(0, 0, 0);
        Vector3 end = new Vector3(10, 0, 0);
        Color col = Color.blue;



        for (int i = 0; i < 100; i++)
        {
            DrawLine(start, end, col, 100.0f);
            end = RotatePointAroundPivot(end, start, new Vector3(10, 10, 10));
        }
      
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Standard"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }


}
