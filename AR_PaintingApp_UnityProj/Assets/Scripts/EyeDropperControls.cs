using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeDropperControls : MonoBehaviour
{

    public void SetMove(System.Single x)
    {
        transform.position = new Vector3(x, -1, 9);
    }
}
