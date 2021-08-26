using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ResetARSession : MonoBehaviour
{
    [SerializeField]
    ARSession arSession;

    public void ResetSession()
    {
        arSession.Reset();
    }
}
