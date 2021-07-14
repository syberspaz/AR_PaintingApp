using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFromButton : MonoBehaviour
{
    public Vector3 newPosition;

    public void Move()
    {
        transform.position = newPosition;
    }

    public void Move(float newPos)
    {
        transform.position = new Vector3(newPos,0,515);
    }
}
