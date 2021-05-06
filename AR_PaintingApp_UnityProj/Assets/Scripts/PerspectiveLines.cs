using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveLines : MonoBehaviour
{
    [SerializeField]
    private Material LineMaterial;

    [SerializeField]
    public Vector3 start = new Vector3(0, 0, 0);
    [SerializeField]
    public Vector3 end = new Vector3(10, 0, 0);
    [SerializeField]
    public Color col = Color.blue;
    [SerializeField]
    public int NumOfLines;
    [SerializeField]
    public Vector3 Rotation = new Vector3(10,10,10);

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
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);


    }

    public void CreateLines(int NumOfLines)
    {
        for (int i = 0; i < NumOfLines; i++)
        {
            DrawLine(start, end, col);
            end = RotatePointAroundPivot(end, start, Rotation);
        }
    }

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    public void DestroyAllLines()
    {
       GameObject[] PerspectiveLines = GameObject.FindGameObjectsWithTag("PerspectiveLine");

        for(int i = 0; i < PerspectiveLines.Length; i++)
        {
            GameObject.Destroy(PerspectiveLines[i], 0.0f);
        }

    }

    public void SetRotX(float rot)
    {
        Rotation.x = rot;
    }
    public void SetRotY(float rot)
    {
        Rotation.y = rot;
    }
    public void SetRotZ(float rot)
    {
        Rotation.z = rot;
    }
    public void SetStartPosX(float pos)
    {
        start.x = pos;
    }
    public void SetStartPosY(float pos)
    {
        start.y = pos;
    }
    public void SetStartPosZ(float pos)
    {
        start.z = pos;
    }
    public void SetEndPosX(float pos)
    {
        end.x = pos;
    }
    public void SetEndPosY(float pos)
    {
        end.y = pos;
    }
    public void SetEndPosZ(float pos)
    {
        end.z = pos;
    }

}
