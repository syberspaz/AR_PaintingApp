using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Require the object to have a recttransform, since the scripts that use this as a flag will require this
[RequireComponent(typeof(RectTransform))]

public class Pinchable : MonoBehaviour
{
    
    //This acts more as a flag than a script, give this to ui objects that should be scaled with a pinch

}
