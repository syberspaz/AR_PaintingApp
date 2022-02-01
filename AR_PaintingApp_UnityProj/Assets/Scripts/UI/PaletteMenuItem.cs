using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteMenuItem : MonoBehaviour
{

    [Tooltip("1 is calipers, 2 is lines, 3 is image search")]
    public int ToolType; //this value will be used to tell what tool was just hit across functions
    public List<GameObject> toolGO; //some tools need a direct call to 1 or multiple game objects
}
