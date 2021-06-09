using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeDropperColor : MonoBehaviour
{
    private Renderer renderer;

    [SerializeField]
    private GameObject tip;

    private RaycastHit touch;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

        //Temporary test/proof of concept, the block of .SetColor functions is useful for the future however
        if (Physics.Raycast(tip.transform.position, new Vector3(0,-1,0), out touch, 10f))
        {
            Renderer hitObjectRenderer = touch.collider.GetComponent<Renderer>();
            Color newCol = hitObjectRenderer.material.color;
            renderer.material.SetColor("_LiquidColor", newCol);
            renderer.material.SetColor("_SurfaceColor", newCol);
            renderer.material.SetColor("_FresnelColor", newCol);
        }

       
    }
}
