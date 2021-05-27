using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using UnityEngine.UI;

namespace OpenCvSharp
{

    public class High_Low_Key_Grading : MonoBehaviour
    {
        private List<Rect> rectsGrid;

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
            System.DateTime before = System.DateTime.Now;
            Mat inputMat = Unity.TextureToMat(inputTexture);
            inputImageVisual.texture = inputTexture;
            //Divide the image into 10 equal sized pieces
            //Create the grid of Rects that will split up the image
            for (int y = 0; y < GridSizeY ; y++)
                for(int x = 0; x < GridSizeX; x++)
                {
                    Size rectSize = new Size(inputMat.Width / GridSizeX, inputMat.Height / GridSizeY);
                    Rect tempRect = new Rect(new Point(x * inputMat.Width / GridSizeX, y * inputMat.Height / GridSizeY), rectSize);
                    Cv2.Rectangle(inputMat, tempRect, new Scalar(255, 0, 0));
                    //Debug.Log(tempRect.ToString());

                }
           
            Debug.Log(System.DateTime.Now);
            outputImageVisual.texture = Unity.MatToTexture(inputMat);
            System.DateTime after = System.DateTime.Now;

            System.TimeSpan duration = after.Subtract(before);
            Debug.Log("Duration in milliseconds: " + duration.Milliseconds);

        }
    }

}