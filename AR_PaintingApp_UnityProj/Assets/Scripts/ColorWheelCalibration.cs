using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using UnityEngine.UI;

namespace OpenCvSharp
{
    public class ColorWheelCalibration : MonoBehaviour
    {
        //Image stuff that needs to be Input/Output
        [SerializeField]
        Texture2D inputTexture;

        [SerializeField]
        RawImage inputVis;
        [SerializeField]
        RawImage outputVis;

        public void Start()
        {
            inputVis.texture = inputTexture;

            Mat inMat;
            inMat = Unity.TextureToMat(inputTexture);

            Mat ContoursMat = new Mat();

           
            

            
          

        }
    }
}
