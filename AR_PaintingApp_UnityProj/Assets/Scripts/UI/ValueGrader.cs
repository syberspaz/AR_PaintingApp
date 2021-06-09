using Unity.Collections.LowLevel.Unsafe;
namespace OpenCvSharp
{
    //looks at a series of value boxes, creates an array of them and gets the color inside them
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

        [SerializeField]
        private SceneCameraPassthrough cameraInfo;

        //test source image
        [SerializeField]
        private Texture2D sourceImage;


        private Texture2D m_CameraTexture;

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
            sourceImage = cameraInfo.camOutput;
            //Set up Mat for opencv and display the source image
            Mat input = Unity.TextureToMat(sourceImage);
            outputImages[0].texture = Unity.MatToTexture(input);

            //This entire block finds and draws the contours
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

            //shows the contours in the unity canvas
            outputImages[1].texture = Unity.MatToTexture(input);

            //Create our bounding boxes and add them to a list for more processing
            List<Rect> boundingBoxes = new List<Rect>();
            for (int i = 0; i < contours.Length; i++)
                boundingBoxes.Add(Cv2.BoundingRect(contours[i]));
            
            //sort bounding boxes
            for (int i = 0; i < boundingBoxes.Count; i++)
            {
                for (int j = 0; j < boundingBoxes.Count; j++)
                {
                    if (boundingBoxes[i].Center.X < boundingBoxes[j].Center.X)
                    {
                        var temp = boundingBoxes[i];
                        boundingBoxes[i] = boundingBoxes[j];
                        boundingBoxes[j] = temp;
                    }
                }
            }

            Mat colorAnalysisMat;
            colorAnalysisMat = Unity.TextureToMat(sourceImage);
           
            //Cv2.CvtColor(colorAnalysisMat, colorAnalysisMat, ColorConversionCodes.BGR2HSV);

            //Draws the circles at the center of the bounding boxes (for testing)
            for (int i = 0; i < boundingBoxes.Count; i++)
            {
                Debug.Log(boundingBoxes[i].Center);

                float h, s, v;
                Color.RGBToHSV(sourceImage.GetPixel((int)boundingBoxes[i].Center.X, (int)boundingBoxes[i].Center.Y),out h,out s,out v);

                Debug.Log("H: " + h + " S: " + s + " V: " + v);


                if (i == 5)
                    Cv2.Rectangle(colorAnalysisMat, boundingBoxes[i], new Scalar(100, 0, 20), 4);
            }
            //Shows the image with the circles in the unity canvas
            outputImages[2].texture = Unity.MatToTexture(colorAnalysisMat);




        }

        private void Update()
        {
            /*
            Mat input = Unity.TextureToMat(sourceImage);


            var indexer = input.GetGenericIndexer<Vec3b>();

            Vec3b color = indexer[(int)debugTextLoc.x, (int)debugTextLoc.y];
            Vector3 Col;
            Col.x = color.Item0;
            Col.y = color.Item1;
            Col.z = color.Item2;

            debugText.text = Col.ToString();
            */

        }

    }
}
