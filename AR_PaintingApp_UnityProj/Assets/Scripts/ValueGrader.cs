using Unity.Collections.LowLevel.Unsafe;
namespace OpenCvSharp
{
    using System;

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using OpenCvSharp;
    using OpenCvSharp.Util;
    using UnityEngine.XR.ARFoundation;
    using UnityEngine.XR.ARSubsystems;
    using UnityEngine.UI;
    public class ValueGrader : MonoBehaviour
    {
        //test source image
        [SerializeField]
        private Texture2D sourceImage;

        //element 0 is top left, element 1 is top right, element 2 is bottom left, element 3 is bottom right
        [SerializeField]
        private List<RawImage> outputImages;

        [SerializeField]
        private Text debugText;

        [SerializeField]
        private Vector2 debugTextLoc;


        //Later this will be turned into a seperate function
        void Start()
        {
            Mat input = Unity.TextureToMat(sourceImage);
            //Just shows the sourceImage before processing
            outputImages[0].texture = Unity.MatToTexture(input);

           

            Mat ContoursMat = Unity.TextureToMat(sourceImage);

            Cv2.CvtColor(ContoursMat, ContoursMat, ColorConversionCodes.BGR2GRAY);

            ContoursMat.GaussianBlur(new Size(5, 5), 0);
            //threshold for b&w
            Cv2.Threshold(ContoursMat, ContoursMat, 0.0, 255.0, ThresholdTypes.Otsu);
            //canny edge detection
            Cv2.Canny(ContoursMat, ContoursMat, 50.0, 50.0);

            HierarchyIndex[] hierarchies;
            Point[][] contours;
            Cv2.FindContours(ContoursMat, out contours, out hierarchies, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            Cv2.DrawContours(input, contours, -1, new OpenCvSharp.Scalar(255,0,0), 4, LineTypes.Link8);

            outputImages[1].texture = Unity.MatToTexture(input);


            

        }

        private void Update()
        {
            Mat input = Unity.TextureToMat(sourceImage);


            var indexer = input.GetGenericIndexer<Vec3b>();

            Vec3b color = indexer[(int)debugTextLoc.x, (int)debugTextLoc.y];
            Vector3 Col;
            Col.x = color.Item0;
            Col.y = color.Item1;
            Col.z = color.Item2;

            debugText.text = Col.ToString();


        }


    }
}
