using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;

using OpenCvSharp;
using OpenCvSharp.Util;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using UnityEngine.UI;

namespace OpenCvSharp
{
    public class AREyedropperManager : MonoBehaviour
    {
        Texture2D m_CameraTexture;

        Color thisCol;
        Texture2D CamTex;
        Texture2D ScannedCamTex;
        Texture2D ResizedCamTex;
        Button currentButton;
        Color currentColor;

        public ARCameraManager cameraManager;

        public RawImage rawColourViewer;

        public GameObject coloursGameObject;
        public GameObject colourAnalysisObject;
        public GameObject colourModiferObject;
        public GameObject palettesGameObject;

        public Slider hueSlider;
        public Slider satSlider;
        public Slider valSlider;

        public RawImage Complementary;

        public RawImage Monochromatic;

        public RawImage Analogous1;
        public RawImage Analogous2;

        public RawImage Triadic1;
        public RawImage Triadic2;

        public RawImage Tetradic1;
        public RawImage Tetradic2;
        public RawImage Tetradic3;

        List<List<Color>> buckets = new List<List<Color>>();
        List<List<Color>> biggestBuckets = new List<List<Color>>();

        List<Color> Bucket1 = new List<Color>();
        List<Color> Bucket2 = new List<Color>();
        List<Color> Bucket3 = new List<Color>();
        List<Color> Bucket4 = new List<Color>();
        List<Color> Bucket5 = new List<Color>();
        List<Color> Bucket6 = new List<Color>();
        List<Color> Bucket7 = new List<Color>();
        List<Color> Bucket8 = new List<Color>();
        List<Color> Bucket9 = new List<Color>();
        List<Color> Bucket10 = new List<Color>();
        List<Color> Bucket11 = new List<Color>();
        List<Color> Bucket12 = new List<Color>();
        List<Color> Bucket13 = new List<Color>();
        List<Color> Bucket14 = new List<Color>();
        List<Color> Bucket15 = new List<Color>();
        List<Color> Bucket16 = new List<Color>();
        List<Color> Bucket17 = new List<Color>();
        List<Color> Bucket18 = new List<Color>();
        List<Color> Bucket19 = new List<Color>();
        List<Color> Bucket20 = new List<Color>();
        List<Color> Bucket21 = new List<Color>();
        List<Color> Bucket22 = new List<Color>();
        List<Color> Bucket23 = new List<Color>();
        List<Color> Bucket24 = new List<Color>();
        List<Color> Bucket25 = new List<Color>();
        List<Color> Bucket26 = new List<Color>();
        List<Color> Bucket27 = new List<Color>();

        [SerializeField]
        List<RawImage> palettes = new List<RawImage>();

        List<Dictionary<string, object>> colourList;

        [SerializeField]
        Color monoColor;

        [SerializeField]
        bool checkColourKey;

        [SerializeField]
        bool isHighKey;

        [SerializeField]
        bool isLowKey;

        [SerializeField]
        bool isMidKey;

        [SerializeField]
        bool useSatRange;

        [SerializeField]
        float startRange;

        [SerializeField]
        float endRange;

        [SerializeField]
        Text ErrPercentageText;

        private Mat hFactor;

        private void Start()
        {
            colourList = CSVReader.Read("example");
            HomographyCalculation();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                UpdateCameraImage();
                thisCol = CamTex.GetPixel((int)TextureScale.Map(Input.GetTouch(0).position.x, 0, Screen.currentResolution.width, 0, CamTex.width), (int)TextureScale.Map(Input.GetTouch(0).position.y, 0, Screen.currentResolution.height, 0, CamTex.height));
                currentButton.image.color = thisCol;
                currentColor = thisCol;
                monoColor = currentColor;
                currentButton.GetComponentInChildren<Text>().text = "Hexcode: #" + ColorUtility.ToHtmlStringRGB(thisCol);
                /*string colourName = FindColour(ColorUtility.ToHtmlStringRGB(thisCol));
                if (colourName == "null")
                {
                    currentButton.GetComponentInChildren<Text>().text = "Hexcode: #" + ColorUtility.ToHtmlStringRGB(thisCol);
                }
                else
                {
                    currentButton.GetComponentInChildren<Text>().text = colourName;
                }*/
                currentButton = null;
            }

            if (colourModiferObject.activeSelf == true)
            {
                
            }
        }

        void HomographyCalculation()
        {
            // CAMERA Colours
            Color camRedRGB;
            ColorUtility.TryParseHtmlString("d05546", out camRedRGB);
            ConvertToXYZ.CIEXYZ camRed = ConvertToXYZ.ColortoXYZ(camRedRGB);

            Color camGreenRGB;
            ColorUtility.TryParseHtmlString("378d5c", out camGreenRGB);
            ConvertToXYZ.CIEXYZ camGreen = ConvertToXYZ.ColortoXYZ(camGreenRGB);

            Color camBlueRGB;
            ColorUtility.TryParseHtmlString("395585", out camBlueRGB);
            ConvertToXYZ.CIEXYZ camBlue = ConvertToXYZ.ColortoXYZ(camBlueRGB);

            Color camYellowRGB;
            ColorUtility.TryParseHtmlString("f0bd26", out camYellowRGB);
            ConvertToXYZ.CIEXYZ camYellow = ConvertToXYZ.ColortoXYZ(camYellowRGB);
            //print("X: " + newConv.X + " | Y: " + newConv.Y + " | Z: " + newConv.Z);

            Point3d[] src = {
                new Point3d(camRed.X, camRed.Y, camRed.Z),
                new Point3d(camGreen.X, camGreen.Y, camGreen.Z),
                new Point3d(camBlue.X, camBlue.Y, camBlue.Z),
                new Point3d(camYellow.X, camYellow.Y, camYellow.Z)
        };

            // IDEAL Colours
            ConvertToXYZ.CIEXYZ red = ConvertToXYZ.ColortoXYZ(Color.red);
            ConvertToXYZ.CIEXYZ green = ConvertToXYZ.ColortoXYZ(Color.green);
            ConvertToXYZ.CIEXYZ blue = ConvertToXYZ.ColortoXYZ(Color.blue);
            ConvertToXYZ.CIEXYZ yellow = ConvertToXYZ.ColortoXYZ(Color.yellow);

            Point3d[] dst = {
                new Point3d(red.X, red.Y, red.Z),
                new Point3d(green.X, green.Y, green.Z),
                new Point3d(blue.X, blue.Y, blue.Z),
                new Point3d(yellow.X, yellow.Y, yellow.Z)
        };

            InputArray src_pts = InputArray.Create(src);
            InputArray dst_pts = InputArray.Create(dst);

            // 1. Calculate Homography
            hFactor = Cv2.FindHomography(src_pts, dst_pts);
            /*for (var rowIndex = 0; rowIndex < h.Rows; rowIndex++)
            {
                for (var colIndex = 0; colIndex < h.Cols; colIndex++)
                {
                    print($"{h.At<double>(rowIndex, colIndex)} ");
                }
            }*/


        }

        ConvertToXYZ.CIEXYZ CalibColor(ConvertToXYZ.CIEXYZ Pixel)
        {
            return new ConvertToXYZ.CIEXYZ(
                    (Pixel.X * hFactor.At<double>(0, 0) + Pixel.Y * hFactor.At<double>(0, 1) + Pixel.Z * hFactor.At<double>(0, 2)),
                    (Pixel.X * hFactor.At<double>(1, 0) + Pixel.Y * hFactor.At<double>(1, 1) + Pixel.Z * hFactor.At<double>(1, 2)),
                    (Pixel.X * hFactor.At<double>(2, 0) + Pixel.Y * hFactor.At<double>(2, 1) + Pixel.Z * hFactor.At<double>(2, 2))
                    );
        }

        public unsafe void UpdateCameraImage()
        {
            // Attempt to get the latest camera image. If this method succeeds,
            // it acquires a native resource that must be disposed (see below).
            if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            {
                return;
            }

            // Once we have a valid XRCpuImage, we can access the individual image "planes"
            // (the separate channels in the image). XRCpuImage.GetPlane provides
            // low-overhead access to this data. This could then be passed to a
            // computer vision algorithm. Here, we will convert the camera image
            // to an RGBA texture and draw it on the screen.

            // Choose an RGBA format.
            // See XRCpuImage.FormatSupported for a complete list of supported formats.
            var format = TextureFormat.RGBA32;

            if (m_CameraTexture == null || m_CameraTexture.width != image.width || m_CameraTexture.height != image.height)
            {
                m_CameraTexture = new Texture2D(image.width, image.height, format, false);
            }

            // Convert the image to format, flipping the image across the Y axis.
            // We can also get a sub rectangle, but we'll get the full image here.
            var conversionParams = new XRCpuImage.ConversionParams(image, format, XRCpuImage.Transformation.MirrorY);

            // Texture2D allows us write directly to the raw texture data
            // This allows us to do the conversion in-place without making any copies.
            var rawTextureData = m_CameraTexture.GetRawTextureData<byte>();
            try
            {
                image.Convert(conversionParams, new IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);
            }
            finally
            {
                // We must dispose of the XRCpuImage after we're finished
                // with it to avoid leaking native resources.
                image.Dispose();
            }

            // Apply the updated texture data to our texture
            m_CameraTexture.Apply();
            Texture2D newTex = RotateTexture(m_CameraTexture);
            newTex.Apply();
            //rawColourViewer.texture = newTex;
            CamTex = newTex;
            /*ResizedCamTex = newTex;
            TextureScale.Bilinear(ResizedCamTex, m_CameraTexture.width / 5, m_CameraTexture.height / 5);*/
        }

        public void ModifyColours()
        {
            if (colourModiferObject.activeSelf == true)
            {
                colourModiferObject.SetActive(false);
            }
            else
            {
                UpdateCameraImage();
                ProcessImage();
                rawColourViewer.texture = CamTex;
                colourModiferObject.SetActive(true);
            }
        }

        public void ColourAnalysis()
        {
            if (colourAnalysisObject.activeSelf == true)
            {
                colourAnalysisObject.SetActive(false);
            }
            else
            {
                UpdateCameraImage();
                ProcessImage();
                Texture2D changedTexture2D = new Texture2D(ScannedCamTex.width, ScannedCamTex.height);
                float curHue, curSat, curVal, hue, sat, val;
                Color.RGBToHSV(monoColor, out hue, out sat, out val);
                Color.RGBToHSV(currentColor, out curHue, out curSat, out curVal);
                float hueColDiff = hue - curHue;
                float satColDiff = sat - curSat;
                float valColDiff = val - curVal;
                print("Hue: " + hue + " | Sat: " + sat + " | Val: " + val);
                int outsideColCounter = 0;
                float hueDiff = TextureScale.Map(10, 0.0f, 360.0f, 0.0f, 1.0f);
                float satDiff = TextureScale.Map(6, 0.0f, 100.0f, 0.0f, 1.0f);
                float valDiff = TextureScale.Map(6, 0.0f, 100.0f, 0.0f, 1.0f);

                float numPixels = ScannedCamTex.width * ScannedCamTex.height;
                for (int i = 0; i < ScannedCamTex.width; i++)
                {
                    for (int j = 0; j < ScannedCamTex.height; j++)
                    {
                        Color thisCol = ScannedCamTex.GetPixel(i, j);
                        //Color tempCol = ScannedCamTex.GetPixel(i, j);
                        float tempHue, tempSat, tempVal;
                        //Color.RGBToHSV(tempCol, out tempHue, out tempSat, out tempVal);
                        //Color thisCol = Color.HSVToRGB(tempHue, tempSat + satColDiff, tempVal + valColDiff);
                        float hue2, sat2, val2;
                        Color.RGBToHSV(thisCol, out hue2, out sat2, out val2);
                        if (!checkColourKey)
                        {
                            if (useSatRange)
                            {
                                if ((hue2 < hue - hueDiff || hue2 > hue + hueDiff) || (val2 < val - valDiff || val2 > val + valDiff) || (sat2 < startRange || sat2 > endRange))
                                {
                                    changedTexture2D.SetPixel(i, j, Color.HSVToRGB(hue2, sat2, 0.05f));
                                    outsideColCounter++;
                                }
                                else
                                {
                                    changedTexture2D.SetPixel(i, j, thisCol);
                                }
                            }
                            else
                            {
                                if ((hue2 < hue - hueDiff || hue2 > hue + hueDiff) || (val2 < val - valDiff || val2 > val + valDiff))
                                {
                                    changedTexture2D.SetPixel(i, j, Color.HSVToRGB(hue2, sat2, 0.08f));
                                    outsideColCounter++;
                                }
                                else
                                {
                                    changedTexture2D.SetPixel(i, j, thisCol);
                                }
                            }
                        }
                        else
                        {
                            if (isHighKey)
                            {
                                if (val2 < 0.5f)
                                {
                                    changedTexture2D.SetPixel(i, j, Color.HSVToRGB(hue2, sat2, 0.08f));
                                    outsideColCounter++;
                                }
                                else
                                {
                                    changedTexture2D.SetPixel(i, j, thisCol);
                                }
                            }
                            if (isMidKey)
                            {

                                if (val2 <= 0.4f || val2 >= 0.6f)
                                {
                                    changedTexture2D.SetPixel(i, j, Color.HSVToRGB(hue2, sat2, 0.08f));
                                    outsideColCounter++;
                                }
                                else
                                {
                                    changedTexture2D.SetPixel(i, j, thisCol);
                                }
                            }
                            if (isLowKey)
                            {

                                if (val2 >= 0.5f)
                                {
                                    changedTexture2D.SetPixel(i, j, Color.HSVToRGB(hue2, sat2, 0.08f));
                                    outsideColCounter++;
                                }
                                else
                                {
                                    changedTexture2D.SetPixel(i, j, thisCol);
                                }
                            }
                        }
                    }
                }
                changedTexture2D.Apply();
                float errPer = outsideColCounter / numPixels * 100.0f;
                ErrPercentageText.text = "Error: " + (int)errPer + "%";
                print("Outside Col Counter: " + outsideColCounter);
                colourAnalysisObject.SetActive(true);
                rawColourViewer.texture = changedTexture2D;
            }
        }

        public void TakePicture()
        {
            UpdateCameraImage();
            ProcessImage();
            ColourAnalysis();
            m_CameraTexture.GetPixels();
        }

        // Use this for initialization
        void ProcessImage()
        {
            Mat sourceMat = Unity.TextureToMat(CamTex);
            Mat ResultMat = Unity.TextureToMat(CamTex);

            //grey scale
            Cv2.CvtColor(sourceMat, sourceMat, ColorConversionCodes.BGR2GRAY);
            //Blurs
            sourceMat.GaussianBlur(new Size(5, 5), 0);
            //threshold for b&w
            Cv2.Threshold(sourceMat, sourceMat, 0.0, 255.0, ThresholdTypes.Otsu);
            //canny edge detection
            Cv2.Canny(sourceMat, sourceMat, 50.0, 50.0);


            HierarchyIndex[] hierarchies;
            Point[][] contours;
            Cv2.FindContours(sourceMat, out contours, out hierarchies, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            // check contours and drop those we consider "noise", all others put into a single huge "key points" map
            // also, detect all almost-rectangular contours with big area and try to determine whether they're exact match
            List<Point> keyPoints = new List<Point>();
            List<Point[]> goodCandidates = new List<Point[]>();
            double referenceArea = ResultMat.Width * ResultMat.Height;
            foreach (Point[] contour in contours)
            {
                double length = Cv2.ArcLength(contour, true);

                // drop mini-contours
                if (length >= 25.0)
                {
                    Point[] approx = Cv2.ApproxPolyDP(contour, length * 0.01, true);
                    keyPoints.AddRange(approx);

                    if (approx.Length >= 4 && approx.Length <= 6)
                    {
                        double area = Cv2.ContourArea(approx);
                        if (area / referenceArea >= 0.33)
                            goodCandidates.Add(approx);
                    }
                }
            }

            // compute convex hull, considering we presume having an image of a document on more or less
            // homogeneous background, this accumulated convex hull should be the document bounding contour
            Point[] hull = Cv2.ConvexHull(keyPoints);
            Point[] hullContour = Cv2.ApproxPolyDP(hull, Cv2.ArcLength(hull, true) * 0.01, true);

            // find best guess for our contour
            Point[] paperContour = GetBestMatchingContour(sourceMat.Width * sourceMat.Height, goodCandidates, hullContour);
            if (null == paperContour)
            {
                Debug.Log("No Contour");

                ResultMat = null;
                ResultMat = Unity.TextureToMat(CamTex);
                return;
            }

            if (paperContour.Length == 4)
            {
                Debug.Log("Sorting Contour");
                paperContour = SortCorners(paperContour);
            }
            // some hit: we either have 3 points or > 4 which we can try to make a 4-corner shape
            else if (paperContour.Length > 2)
            {
                Debug.Log("Finding Contour (3 or more than 4)");

                // yet contour might contain too much points: along with calculation inaccuracies we might face a
                // bended piece of paper, missing corner etc.
                // the solution is to use bounding box
                RotatedRect bounds = Cv2.MinAreaRect(paperContour);
                Point2f[] points = bounds.Points();
                Point[] intPoints = Array.ConvertAll(points, p => new Point(Math.Round(p.X), Math.Round(p.Y)));
                Point[] fourCorners = SortCorners(intPoints);

                // array.ClosestElement is not efficient but we can live with it since it's quite few
                // elements to search for
                System.Func<Point, Point, double> distance = (Point x, Point y) => Point.Distance(x, y);
                Point[] closest = new Point[4];
                for (int i = 0; i < fourCorners.Length; ++i)
                    closest[i] = ClosestElement(paperContour, fourCorners[i], distance);

                paperContour = closest;
            }

            var matUnwrapped = ResultMat;
            if (paperContour.Length == 4)
            {
                Debug.Log("Unwraping Shape");

                matUnwrapped = UnwrapShape(ResultMat, Array.ConvertAll(paperContour, p => new Point2f(p.X, p.Y)));
                // automatic color converter


            }
            ScannedCamTex = Unity.MatToTexture(matUnwrapped);
            rawColourViewer.texture = ScannedCamTex;
        }

        public bool Contains<T>(T[] array, T obj)
        {
            return ((IList<T>)array).Contains(obj);
        }

        public static void Swap<T>(T[] array, int i1, int i2)
        {
            T v = array[i2];
            array[i2] = array[i1];
            array[i1] = v;
        }

        private Point[] SortCorners(Point[] corners)
        {
            if (corners.Length != 4)
                throw new OpenCvSharpException("\"corners\" must be an array of 4 elements");

            // divide vertically
            System.Array.Sort<Point>(corners, (a, b) => a.Y.CompareTo(b.Y));
            Point[] tops = new Point[] { corners[0], corners[1] }, bottoms = new Point[] { corners[2], corners[3] };

            // divide horizontally
            System.Array.Sort<Point>(corners, (a, b) => a.X.CompareTo(b.X));
            Point[] lefts = new Point[] { corners[0], corners[1] }, rights = new Point[] { corners[2], corners[3] };

            // fetch final array
            Point[] output = new Point[] {
                tops[0],
                tops[1],
                bottoms[0],
                bottoms[1]
            };
            if (!Contains(lefts, tops[0]))
                Swap(output, 0, 1);
            if (!Contains(rights, bottoms[0]))
                Swap(output, 2, 3);

            // done
            return output;
        }

        public static T ClosestElement<T>(T[] array, T referenceObject, System.Func<T, T, double> distanceMeasurer)
        {
            if (array.Length == 0)
                throw new OpenCvSharpException("Array can not be empty.");

            // LINQ would work best here, but I read about issues with sorting (OrderBy) on Unity/iOS, so I'll pass and
            // make my own sorting
            T[] copy = (T[])array.Clone();
            System.Array.Sort(copy, (a, b) => distanceMeasurer(a, referenceObject).CompareTo(distanceMeasurer(b, referenceObject)));
            return copy[0];
        }

        private Point[] GetBestMatchingContour(double areaSize, List<Point[]> candidates, Point[] hull)
        {
            Point[] result = hull;
            if (candidates.Count == 1)
                result = candidates[0];
            else if (candidates.Count > 1)
            {
                List<Point> keys = new List<Point>();
                foreach (var c in candidates)
                    keys.AddRange(c);

                Point[] joinedCandidates = Cv2.ConvexHull(keys);
                Point[] joinedHull = Cv2.ApproxPolyDP(joinedCandidates, Cv2.ArcLength(joinedCandidates, true) * 0.01, true);
                result = joinedHull;
            }

            // check further
            if (true)
            {
                double area = Cv2.ContourArea(result);
                if (area / areaSize < 0.33 * 0.75)
                    result = null;
            }

            return result;
        }

        //public Mat UnwrapShape(this Mat img, Point2f[] corners, int maxSize = 0)
        public Mat UnwrapShape(Mat img, Point2f[] corners, int maxSize = 0)
        {
            if (corners.Length != 4)
                throw new OpenCvSharpException("argument 'points' must be of length = 4");

            // grab bounds corners, sort them in correct order and
            // get width/height of the shape
            float width = (float)Point2f.Distance(corners[0], corners[1]);  // lt -> rt
            float height = (float)Point2f.Distance(corners[0], corners[3]); // lt -> lb

            // downscaling
            if (maxSize > 0 && (width > maxSize || height > maxSize))
            {
                if (width > height)
                {
                    var s = maxSize / width;
                    width = maxSize;
                    height = height * s;
                }
                else
                {
                    var s = maxSize / height;
                    height = maxSize;
                    width = width * s;
                }
            }

            // compute transform
            Point2f[] destination = new Point2f[]
            {
                new Point2f(0,     0),
                new Point2f(width, 0),
                new Point2f(width, height),
                new Point2f(0,     height)
            };
            var transform = Cv2.GetPerspectiveTransform(corners, destination);

            // un-warp
            return img.WarpPerspective(transform, new Size(width, height), InterpolationFlags.Cubic);
        }

        private string FindColour(string hexcode)
        {
            for (int i = 0; i < colourList.Count; i++)
            {
                if (hexcode == (string)colourList[i]["Hex"])
                {
                    return (string)colourList[i]["Name"];
                }
            }
            return "null";
        }

        public Texture2D RotateTexture(Texture2D t)
        {
            Texture2D newTexture = new Texture2D(t.height, t.width, t.format, false);

            for (int i = 0; i < t.height; i++)
            {
                for (int j = 0; j < t.width; j++)
                {
                    newTexture.SetPixel(i - (t.width - t.height), j, t.GetPixel(j, t.width - i));
                }
            }

            newTexture.Apply();
            return newTexture;
        }

        public void Screenshot()
        {
            UpdateCameraImage();
        }

        private void GeneratePalette()
        {
            UpdateCameraImage();
            for (int i = 0; i < CamTex.height; i++)
            {
                for (int j = 0; j < CamTex.width; j++)
                {
                    Color thisCol = CamTex.GetPixel(i, j);
                    if (thisCol.r <= 0.33f)
                    {
                        if (thisCol.g <= 0.33f)
                        {
                            if (thisCol.b <= 0.33f)
                            {
                                Bucket1.Add(thisCol);
                            }
                            else if (thisCol.b <= 0.66f)
                            {
                                Bucket10.Add(thisCol);
                            }
                            else if (thisCol.b <= 1.0f)
                            {
                                Bucket19.Add(thisCol);
                            }
                        }
                        else if (thisCol.g <= 0.66f)
                        {
                            if (thisCol.b <= 0.33f)
                            {
                                Bucket2.Add(thisCol);
                            }
                            else if (thisCol.b <= 0.66f)
                            {
                                Bucket11.Add(thisCol);
                            }
                            else if (thisCol.b <= 1.0f)
                            {
                                Bucket20.Add(thisCol);
                            }
                        }
                        else if (thisCol.g <= 1.0f)
                        {
                            if (thisCol.b <= 0.33f)
                            {
                                Bucket3.Add(thisCol);
                            }
                            else if (thisCol.b <= 0.66f)
                            {
                                Bucket12.Add(thisCol);
                            }
                            else if (thisCol.b <= 1.0f)
                            {
                                Bucket21.Add(thisCol);
                            }
                        }
                    }
                    else if (thisCol.r <= 0.66f)
                    {
                        if (thisCol.g <= 0.33f)
                        {
                            if (thisCol.b <= 0.33f)
                            {
                                Bucket4.Add(thisCol);
                            }
                            else if (thisCol.b <= 0.66f)
                            {
                                Bucket13.Add(thisCol);
                            }
                            else if (thisCol.b <= 1.0f)
                            {
                                Bucket22.Add(thisCol);
                            }
                        }
                        else if (thisCol.g <= 0.66f)
                        {
                            if (thisCol.b <= 0.33f)
                            {
                                Bucket5.Add(thisCol);
                            }
                            else if (thisCol.b <= 0.66f)
                            {
                                Bucket14.Add(thisCol);
                            }
                            else if (thisCol.b <= 1.0f)
                            {
                                Bucket23.Add(thisCol);
                            }
                        }
                        else if (thisCol.g <= 1.0f)
                        {
                            if (thisCol.b <= 0.33f)
                            {
                                Bucket6.Add(thisCol);
                            }
                            else if (thisCol.b <= 0.66f)
                            {
                                Bucket15.Add(thisCol);
                            }
                            else if (thisCol.b <= 1.0f)
                            {
                                Bucket24.Add(thisCol);
                            }
                        }
                    }
                    else if (thisCol.r <= 1.0f)
                    {
                        if (thisCol.g <= 0.33f)
                        {
                            if (thisCol.b <= 0.33f)
                            {
                                Bucket7.Add(thisCol);
                            }
                            else if (thisCol.b <= 0.66f)
                            {
                                Bucket16.Add(thisCol);
                            }
                            else if (thisCol.b <= 1.0f)
                            {
                                Bucket25.Add(thisCol);
                            }
                        }
                        else if (thisCol.g <= 0.66f)
                        {
                            if (thisCol.b <= 0.33f)
                            {
                                Bucket8.Add(thisCol);
                            }
                            else if (thisCol.b <= 0.66f)
                            {
                                Bucket17.Add(thisCol);
                            }
                            else if (thisCol.b <= 1.0f)
                            {
                                Bucket26.Add(thisCol);
                            }
                        }
                        else if (thisCol.g <= 1.0f)
                        {
                            if (thisCol.b <= 0.33f)
                            {
                                Bucket9.Add(thisCol);
                            }
                            else if (thisCol.b <= 0.66f)
                            {
                                Bucket18.Add(thisCol);
                            }
                            else if (thisCol.b <= 1.0f)
                            {
                                Bucket27.Add(thisCol);
                            }
                        }
                    }
                }
            }

            buckets.Add(Bucket1);
            buckets.Add(Bucket2);
            buckets.Add(Bucket3);
            buckets.Add(Bucket4);
            buckets.Add(Bucket5);
            buckets.Add(Bucket6);
            buckets.Add(Bucket7);
            buckets.Add(Bucket8);
            buckets.Add(Bucket9);
            buckets.Add(Bucket10);
            buckets.Add(Bucket11);
            buckets.Add(Bucket12);
            buckets.Add(Bucket13);
            buckets.Add(Bucket14);
            buckets.Add(Bucket15);
            buckets.Add(Bucket16);
            buckets.Add(Bucket17);
            buckets.Add(Bucket18);
            buckets.Add(Bucket19);
            buckets.Add(Bucket20);
            buckets.Add(Bucket21);
            buckets.Add(Bucket22);
            buckets.Add(Bucket23);
            buckets.Add(Bucket24);
            buckets.Add(Bucket25);
            buckets.Add(Bucket26);
            buckets.Add(Bucket27);

            for (int i = 0; i < 5; i++)
            {
                SortValues();
            }

            for (int i = 0; i < biggestBuckets.Count; i++)
            {
                Color tempColor = AverageColorFromTexture(biggestBuckets[i]);
                palettes[i].color = tempColor;
                palettes[i].gameObject.GetComponentInChildren<Text>().text = "Hexcode: #" + ColorUtility.ToHtmlStringRGB(ComplementaryColour(tempColor));
            }

            buckets.Clear();
            biggestBuckets.Clear();
            Bucket1.Clear();
            Bucket2.Clear();
            Bucket3.Clear();
            Bucket4.Clear();
            Bucket5.Clear();
            Bucket6.Clear();
            Bucket7.Clear();
            Bucket8.Clear();
            Bucket9.Clear();
            Bucket10.Clear();
            Bucket11.Clear();
            Bucket12.Clear();
            Bucket13.Clear();
            Bucket14.Clear();
            Bucket15.Clear();
            Bucket16.Clear();
            Bucket17.Clear();
            Bucket18.Clear();
            Bucket19.Clear();
            Bucket20.Clear();
            Bucket21.Clear();
            Bucket22.Clear();
            Bucket23.Clear();
            Bucket24.Clear();
            Bucket25.Clear();
            Bucket26.Clear();
            Bucket27.Clear();
        }

        private void SortValues()
        {
            List<Color> greatestColor = new List<Color>();
            int highestCount = -1;
            int removedIndex = -1;
            for (int i = 0; i < buckets.Count; i++)
            {
                if (buckets[i].Count > highestCount)
                {
                    highestCount = buckets[i].Count;
                    removedIndex = i;
                }
            }
            biggestBuckets.Add(buckets[removedIndex]);
            buckets.RemoveAt(removedIndex);
        }

        Color AverageColorFromTexture(List<Color> bucket)
        {
            int total = bucket.Count;

            float r = 0;
            float g = 0;
            float b = 0;
            float a = 0;

            for (int i = 0; i < total; i++)
            {

                r += bucket[i].r;
                g += bucket[i].g;
                b += bucket[i].b;
                a += bucket[i].a;

            }

            return new Color((r / total), (g / total), (b / total), (a / total));
        }

        public void UpdateColour(GameObject thisBtn)
        {
            currentButton = thisBtn.GetComponent<Button>();
            currentButton.image.color = Color.black;
        }

        public void ShowColourOptions()
        {
            if (coloursGameObject.activeSelf == true)
            {
                coloursGameObject.SetActive(false);
            }
            else
            {
                Complementary.color = ComplementaryColour(currentColor);
                Complementary.gameObject.GetComponentInChildren<Text>().text = "Hexcode: #" + ColorUtility.ToHtmlStringRGB(ComplementaryColour(currentColor));
                Monochromatic.color = MonochromaticColour(currentColor);
                Monochromatic.gameObject.GetComponentInChildren<Text>().text = "Hexcode: #" + ColorUtility.ToHtmlStringRGB(MonochromaticColour(currentColor));
                Analogous1.color = AnalogousColours(currentColor)[0];
                Analogous1.gameObject.GetComponentInChildren<Text>().text = "Hexcode: #" + ColorUtility.ToHtmlStringRGB(AnalogousColours(currentColor)[0]);
                Analogous2.color = AnalogousColours(currentColor)[1];
                Analogous2.gameObject.GetComponentInChildren<Text>().text = "Hexcode: #" + ColorUtility.ToHtmlStringRGB(AnalogousColours(currentColor)[1]);
                Triadic1.color = TriadicColours(currentColor)[0];
                Triadic1.gameObject.GetComponentInChildren<Text>().text = "Hexcode: #" + ColorUtility.ToHtmlStringRGB(TriadicColours(currentColor)[0]);
                Triadic2.color = TriadicColours(currentColor)[1];
                Triadic2.gameObject.GetComponentInChildren<Text>().text = "Hexcode: #" + ColorUtility.ToHtmlStringRGB(TriadicColours(currentColor)[1]);
                Tetradic1.color = TetradicColours(currentColor)[0];
                Tetradic1.gameObject.GetComponentInChildren<Text>().text = "Hexcode: #" + ColorUtility.ToHtmlStringRGB(TetradicColours(currentColor)[0]);
                Tetradic2.color = TetradicColours(currentColor)[1];
                Tetradic2.gameObject.GetComponentInChildren<Text>().text = "Hexcode: #" + ColorUtility.ToHtmlStringRGB(TetradicColours(currentColor)[1]);
                Tetradic3.color = TetradicColours(currentColor)[2];
                Tetradic3.gameObject.GetComponentInChildren<Text>().text = "Hexcode: #" + ColorUtility.ToHtmlStringRGB(TetradicColours(currentColor)[2]);
                coloursGameObject.SetActive(true);
            }
        }

        public void ShowPalettes()
        {
            GeneratePalette();
            palettesGameObject.SetActive(true);
        }

        Color ComplementaryColour(Color _color)
        {
            Color.RGBToHSV(_color, out float H, out float S, out float V);
            float negativeH = (H + 0.5f) % 1f;
            Color negativeColor = Color.HSVToRGB(negativeH, S, V);
            return negativeColor;
        }

        Color MonochromaticColour(Color _color)
        {
            Color.RGBToHSV(_color, out float H, out float S, out float V);
            float monoH = (H - 0.002777778f) % 1f;
            Color monoColor = Color.HSVToRGB(monoH, S, V);
            return monoColor;
        }

        List<Color> AnalogousColours(Color _color)
        {
            //0.08333334f
            List<Color> values = new List<Color>();
            Color.RGBToHSV(_color, out float H, out float S, out float V);
            float anaOneH = (H + 0.083f) % 1f;
            Color anaOneColor = Color.HSVToRGB(anaOneH, S, V);
            values.Add(anaOneColor);

            Color.RGBToHSV(_color, out float H2, out float S2, out float V2);
            float anaTwoH = (H2 - 0.083f) % 1f;
            Color anaTwoColor = Color.HSVToRGB(anaTwoH, S2, V2);
            values.Add(anaTwoColor);
            return values;
        }

        List<Color> TriadicColours(Color _color)
        {
            //0.3333333f
            List<Color> values = new List<Color>();
            Color.RGBToHSV(_color, out float H, out float S, out float V);
            float triOneH = (H + 0.33f) % 1f;
            Color triOneColor = Color.HSVToRGB(triOneH, S, V);
            values.Add(triOneColor);

            Color.RGBToHSV(_color, out float H2, out float S2, out float V2);
            float triTwoH = (H2 + (0.33f * 2)) % 1f;
            Color triTwoColor = Color.HSVToRGB(triTwoH, S2, V2);
            values.Add(triTwoColor);
            return values;
        }

        List<Color> TetradicColours(Color _color)
        {
            //0.25f
            List<Color> values = new List<Color>();
            Color.RGBToHSV(_color, out float H, out float S, out float V);
            float triOneH = (H + 0.25f) % 1f;
            Color triOneColor = Color.HSVToRGB(triOneH, S, V);
            values.Add(triOneColor);

            Color.RGBToHSV(_color, out float H2, out float S2, out float V2);
            float triTwoH = (H2 + (0.25f * 2)) % 1f;
            Color triTwoColor = Color.HSVToRGB(triTwoH, S2, V2);
            values.Add(triTwoColor);

            Color.RGBToHSV(_color, out float H3, out float S3, out float V3);
            float triThreeH = (H3 + (0.25f * 3)) % 1f;
            Color triThreeColor = Color.HSVToRGB(triThreeH, S3, V3);
            values.Add(triThreeColor);
            return values;
        }
    }
}
