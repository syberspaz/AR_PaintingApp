using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using UnityEngine.UI;

namespace OpenCvSharp
{

    public class High_Low_Key_Grading : MonoBehaviour
    {
        private List<Rect> rectsGrid = new List<Rect>();
        private List<Color> AverageColorInBoxes = new List<Color>();

        [SerializeField]
        private Texture2D inputTexture;

        [SerializeField]
        private int GridSizeX;
        [SerializeField]
        private int GridSizeY;

        //User sets the ui panels for visualization
        [SerializeField]
        RawImage inputImageVisual;
        [SerializeField]
        RawImage outputImageVisual;

        public void Start()
        { 
            Mat inputMat = Unity.TextureToMat(inputTexture);
            inputImageVisual.texture = inputTexture;

            //Divide the image into n pieces (n is inputted by user)
            //Create the grid of Rects that will split up the image
            for (int y = 0; y < GridSizeY ; y++)
                for(int x = 0; x < GridSizeX; x++)
                {
                    Size rectSize = new Size(inputMat.Width / GridSizeX, inputMat.Height / GridSizeY);
                    Rect tempRect = new Rect(new Point(x * inputMat.Width / GridSizeX, y * inputMat.Height / GridSizeY), rectSize);
                    rectsGrid.Add(tempRect);

                    //Grid visualization, this line can be removed later
                    Cv2.Rectangle(inputMat, tempRect, new Scalar(255, 0, 0));
                   

                }


            //needs to be outside of loop
            List<Color> colorsInBox = new List<Color>();
            Color average;


            //This is the function that goes through the grid and returns the average color of each square
            //This code is terrible but opencv has forced my hand due to a lack of overloaded functions
            for (int i = 0; i < rectsGrid.Count; i++)
             {
                average = Color.black;
                average.a = 0f;

                int pixelCount = 0;
                //Calculate what pixels to start and end for each square;
                int startX, endX, startY, endY;
                startX = rectsGrid[i].Left;
                endX = rectsGrid[i].Right;
                startY = rectsGrid[i].Top;
                endY = rectsGrid[i].Bottom;
                

                //goes through every pixel in a rect, and adds it to an array of colors (each element of this array is a pixel)
                for (int y = startY; y < endY; y++)
                {
                    for (int x = startX; x < endX; x++)
                    {
                        pixelCount++;
                        colorsInBox.Add(inputTexture.GetPixel(x, y));
                    }
                }

                //after all the colors are aquired for a box, we can average them out
                for (int j = 0; j < colorsInBox.Count; j++)
                {
                    average += colorsInBox[j];
                }
                Debug.Log(average / (float)colorsInBox.Count);


            }
            
            //just sets visualization
            outputImageVisual.texture = Unity.MatToTexture(inputMat);
            

        }
    }

}