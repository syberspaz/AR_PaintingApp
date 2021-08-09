using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceUserPlane : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject userPlane;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlacePlane()
    {
        userPlane.SetActive(true);
    }

    public void DestroyPlane()
    {
        userPlane.SetActive(false);
    }



}
