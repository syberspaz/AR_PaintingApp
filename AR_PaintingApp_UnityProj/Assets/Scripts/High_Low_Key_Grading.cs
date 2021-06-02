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

        [SerializeField]
        RawImage inputImageVisual;
        [SerializeField]
        RawImage outputImageVisual;

        public void Start()
        { 
            Mat inputMat = Unity.TextureToMat(inputTexture);
            inputImageVisual.texture = inputTexture;

            //Divide the image into 10 equal sized pieces
            //Create the grid of Rects that will split up the image
            for (int y = 0; y < GridSizeY ; y++)
                for(int x = 0; x < GridSizeX; x++)
                {
                    Size rectSize = new Size(inputMat.Width / GridSizeX, inputMat.Height / GridSizeY);
                    Rect tempRect = new Rect(new Point(x * inputMat.Width / GridSizeX, y * inputMat.Height / GridSizeY), rectSize);
                    rectsGrid.Add(tempRect);
                    //
                    //Debug.Log(tempRect.ToString());

                }

            //setup an indexer
            var indexer = inputMat.GetGenericIndexer<Vec3b>();

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

                //Debug.Log(startX + " " + endX + " " + startY + " " + endY);

                for (int y = startY; y < endY; y++)
                {
                    for (int x = startX; x < endX; x++)
                    {
                        pixelCount++;
                        colorsInBox.Add(inputTexture.GetPixel(x, y));
                    }
                }

                for (int j = 0; j < colorsInBox.Count; j++)
                {
                    //Debug.Log("Color in box: " + colorsInBox[j]);
                    average += colorsInBox[j];
                }
                Debug.Log(average / (float)colorsInBox.Count);

                //Debug.Log(pixelCount);
                //Debug.Log(average / pixelCount);
                
              }
            

            outputImageVisual.texture = Unity.MatToTexture(inputMat);
            

        }
    }

}