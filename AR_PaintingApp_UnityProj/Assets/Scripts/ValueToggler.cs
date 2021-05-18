using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueToggler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject goToggler;

    private bool Value;
    public void ToggleValue()
    {
        Value = !Value;
        goToggler.SetActive(Value);
    }
}
