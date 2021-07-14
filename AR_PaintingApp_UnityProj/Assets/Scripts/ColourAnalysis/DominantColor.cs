using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
using System.Linq;
using System;

namespace OpenCvSharp
{

    public class DominantColor : MonoBehaviour
    {

        [SerializeField]
        private Image wedgePrefab;

        [SerializeField]
        private Transform pieGraphOrigin;

        private int PixelCount; //needed for % calcs

        [SerializeField]
        private Renderer ImageObject;

        //debug
     

        public int jump;

        public int k;


        public static void Kmeans(Mat input, Mat output, int k)
        {
            using (Mat points = new Mat())
            {
                using (Mat labels = new Mat())
                {
                    using (Mat centers = new Mat())
                    {
                        int width = input.Cols;
                        int height = input.Rows;

                        points.Create(width * height, 1, MatType.CV_32FC3);
                        centers.Create(k, 1, points.Type());
                        output.Create(height, width, input.Type());

                        // Input Image Data
                        int i = 0;
                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++, i++)
                            {
                                Vec3f vec3f = new Vec3f
                                {
                                    Item0 = input.At<Vec3b>(y, x).Item0,
                                    Item1 = input.At<Vec3b>(y, x).Item1,
                                    Item2 = input.At<Vec3b>(y, x).Item2
                                };

                                points.Set<Vec3f>(i, vec3f);
                            }
                        }

                        // Criteria:
                        // – Stop the algorithm iteration if specified accuracy, epsilon, is reached.
                        // – Stop the algorithm after the specified number of iterations, MaxIter.
                        var criteria = new TermCriteria(type: CriteriaType.Eps | CriteriaType.MaxIter, maxCount: 10, epsilon: 1.0);

                        // Finds centers of clusters and groups input samples around the clusters.
                        Cv2.Kmeans(data: points, k: k, bestLabels: labels, criteria: criteria, attempts: 3, flags: KMeansFlags.PpCenters, centers: centers);

                        // Output Image Data
                        i = 0;
                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++, i++)
                            {
                                int index = labels.Get<int>(i);

                                Vec3b vec3b = new Vec3b();

                                int firstComponent = Convert.ToInt32(Math.Round(centers.At<Vec3f>(index).Item0));
                                firstComponent = firstComponent > 255 ? 255 : firstComponent < 0 ? 0 : firstComponent;
                                vec3b.Item0 = Convert.ToByte(firstComponent);

                                int secondComponent = Convert.ToInt32(Math.Round(centers.At<Vec3f>(index).Item1));
                                secondComponent = secondComponent > 255 ? 255 : secondComponent < 0 ? 0 : secondComponent;
                                vec3b.Item1 = Convert.ToByte(secondComponent);

                                int thirdComponent = Convert.ToInt32(Math.Round(centers.At<Vec3f>(index).Item2));
                                thirdComponent = thirdComponent > 255 ? 255 : thirdComponent < 0 ? 0 : thirdComponent;
                                vec3b.Item2 = Convert.ToByte(thirdComponent);

                                output.Set<Vec3b>(y, x, vec3b);
                            }
                        }
                    }
                }
            }
        }


        private void CreateListFromImage()
        {

        

            Texture CanvasTexture = ImageObject.material.mainTexture;

            Texture2D inputTexture = CanvasTexture as Texture2D;

            TextureScale.Bilinear(inputTexture, 300, 300);

            inputTexture.Apply();

            Debug.Log("Starting Kmeans"); //this prints

            Mat InputMat = new Mat();

            Mat outputMat = new Mat();

            //first reduce colors in the image
            Kmeans(Unity.TextureToMat(inputTexture), outputMat, k);

            Debug.Log("Finished kmeans"); //this doesn't print

            Texture2D inputTextureQuantized = Unity.MatToTexture(outputMat);

            PixelCount = 0;

            List<Color> allSampledPixels = new List<Color>();

            //samples every nth pixel (n being whatever the value of jump is set to in the inspector) and stores them in a list
            for (int i = 0; i < inputTextureQuantized.width; i += jump)
            {
                for (int j = 0; j < inputTextureQuantized.height; j += jump)
                {
                    PixelCount++;
                    allSampledPixels.Add(inputTextureQuantized.GetPixel(i, j));
                }
            }

            //Debug.Log(PixelCount);

            //Dictionary stores unique colors, and the amount of times they show up
            Dictionary<Color, int> colorDictionary = new Dictionary<Color, int>();

            for (int i = 0; i < allSampledPixels.Count; i++)
            {
                if (colorDictionary.ContainsKey(allSampledPixels[i]))
                {
                    colorDictionary[allSampledPixels[i]] = colorDictionary[allSampledPixels[i]] + 1;
                }
                else
                {
                    colorDictionary.Add(allSampledPixels[i], 1);
                }
            }



            List<KeyValuePair<Color, int>> myList = colorDictionary.ToList();

            //sort the list from lowest count to highest count
            myList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

            MakeGraph(myList);

        }


        public void CreateColorPallete()
        {
            Debug.Log("Generating Pallete");
            CreateListFromImage();

        }

        private void MakeGraph(List<KeyValuePair<Color, int>> colorsAndValues)
        {
            float total = 0f;
            float zRotation = 0f;

            for (int i = 0; i < colorsAndValues.Count; i++)
            {
                Image newWedge = Instantiate(wedgePrefab) as Image;
                newWedge.transform.SetParent(pieGraphOrigin.transform, false);
                newWedge.color = colorsAndValues.ElementAt(i).Key;

                //calculate %
                float percentage = colorsAndValues.ElementAt(i).Value / (float)PixelCount;
                total += percentage;

             


                newWedge.fillAmount = percentage;

                newWedge.transform.rotation = Quaternion.Euler(new Vector3(0, 0, zRotation));
                zRotation -= newWedge.fillAmount * 360f;


            }
        }



    }
}
