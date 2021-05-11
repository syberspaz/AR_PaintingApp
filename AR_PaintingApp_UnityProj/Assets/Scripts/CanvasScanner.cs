namespace OpenCvSharp
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using OpenCvSharp;
    using OpenCvSharp.Util;

   
    using UnityEngine.UI;
    using System;

    public class CanvasScanner : MonoBehaviour
    {
        public Texture2D SourceImage;
        public RawImage rawImage;
        public RawImage outputImage;


        // Start is called before the first frame update
        void Start()
        {

            Mat sourceMat = Unity.TextureToMat(SourceImage);
            Mat ResultMat = Unity.TextureToMat(SourceImage);

            //grey scale
            Cv2.CvtColor(sourceMat, sourceMat, ColorConversionCodes.BGR2GRAY);
            //Blurs
            sourceMat.GaussianBlur(new Size(5,5), 0);
            //threshold for b&w
            Cv2.Threshold(sourceMat,sourceMat, 0.0, 255.0, ThresholdTypes.Otsu);
            //canny edge detection
            Cv2.Canny(sourceMat, sourceMat, 50.0, 50.0);
            
            
            HierarchyIndex[] hierarchies;
            Point[][] contours;
            Cv2.FindContours(sourceMat, out contours, out hierarchies, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            Cv2.DrawContours(ResultMat, contours, -1, new OpenCvSharp.Scalar(255,0,0), 4, LineTypes.Link8);



           //  Cv2.WarpPerspective(sourceMat, SourceImage,  );

            



            rawImage.texture = SourceImage;
            outputImage.texture = Unity.MatToTexture(ResultMat);

        }

    }
}
