using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGBtoCIE : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float r = 1.0f;
    [Range(0.0f, 1.0f)]
    public float g = 1.0f;
    [Range(0.0f, 1.0f)]
    public float b = 1.0f;
    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    float gammaConvert(float rgbValue){
        if(rgbValue <= 0.04045f){
            return (rgbValue / 12.92f);
        }

        float temp = (rgbValue + 0.055f);
        temp = temp / 1.055f;

        return Mathf.Pow(temp, 2.4f);
    }
    // Update is called once per frame
    void Update()
    {
        float currentR = gammaConvert(r);
        float currentG = gammaConvert(g);
        float currentB = gammaConvert(b);
        float x = 0.4124f * r + 0.3576f * g + 0.1805f * b;
        float y = 0.2126f * r + 0.7152f * g + 0.0722f * b;
        float z = 0.0193f * r + 0.1192f * g + 0.9505f * b;
        rend.material.color = new Color(r, g, b);
    }
}
