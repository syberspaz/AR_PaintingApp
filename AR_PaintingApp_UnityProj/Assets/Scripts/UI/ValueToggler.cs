using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueToggler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject goToggler;

    private bool Value = false; //assume false, if object is active it will get set to active in Start

    private void Start()
    {
        Value = goToggler.activeSelf;
    }

    public void ToggleValue()
    {
        Value = !Value;
        goToggler.SetActive(Value);
    }
}
