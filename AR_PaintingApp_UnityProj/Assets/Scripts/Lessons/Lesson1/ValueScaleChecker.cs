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
    public class ValueScaleChecker : MonoBehaviour
    {
        Texture2D m_CameraTexture;

        Color thisCol;
        Texture2D CamTex;
        Texture2D ScannedCamTex;
        Texture2D ResizedCamTex;
        Color currentColor;

        public ARCameraManager cameraManager;

        public RawImage rawColourViewer;

        [SerializeField]
        List<RawImage> ValueImages = new List<RawImage>();

        private Mat hFactor;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                TakePicture();
                /*UpdateCameraImage();
                thisCol = CamTex.GetPixel((int)TextureScale.Map(Input.GetTouch(0).position.x, 0, Screen.currentResolution.width, 0, CamTex.width), (int)TextureScale.Map(Input.GetTouch(0).position.y, 0, Screen.currentResolution.height, 0, CamTex.height));
                currentColor = thisCol;
                float hue, sat, val;
                Color.RGBToHSV(thisCol, out hue, out sat, out val);
                //ValueImages[0].color = thisCol;
                if (val >= 0.0f && val <= 0.1f)
                {
                    ValueImages[0].color = thisCol;
                }
                else if (val >= 0.1f && val <= 0.2f)
                {
                    ValueImages[1].color = thisCol;
                }
                else if (val >= 0.2f && val <= 0.3f)
                {
                    ValueImages[2].color = thisCol;
                }
                else if (val >= 0.3f && val <= 0.4f)
                {
                    ValueImages[3].color = thisCol;
                }
                else if (val >= 0.4f && val <= 0.5f)
                {
                    ValueImages[4].color = thisCol;
                }
                else if (val >= 0.5f && val <= 0.6f)
                {
                    ValueImages[5].color = thisCol;
                }
                else if (val >= 0.6f && val <= 0.7f)
                {
                    ValueImages[6].color = thisCol;
                }
                else if (val >= 0.7f && val <= 0.8f)
                {
                    ValueImages[6].color = thisCol;
                }
                else if (val >= 0.8f && val <= 0.9f)
                {
                    ValueImages[7].color = thisCol;
                }
                else if (val >= 0.9f && val <= 1.0f)
                {
                    ValueImages[8].color = thisCol;
                }*/
            }
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

        void CheckValues()
        {
            bool check1 = false;
            bool check2 = false;
            bool check3 = false;
            bool check4 = false;
            bool check5 = false;
            bool check6 = false;
            bool check7 = false;
            bool check8 = false;
            bool check9 = false;
            bool check10 = false;

            for (int i = 0; i < ScannedCamTex.width; i++)
            {
                for (int j = 0; j < ScannedCamTex.height; j++)
                {
                    thisCol = ScannedCamTex.GetPixel(i, j);
                    float hue, sat, val;
                    Color.RGBToHSV(thisCol, out hue, out sat, out val);
                    if (val >= 0.0f && val <= 0.1f && check1 == false)
                    {
                        ValueImages[0].color = thisCol;
                        check1 = true;
                    }
                    else if (val >= 0.1f && val <= 0.2f && check2 == false)
                    {
                        ValueImages[1].color = thisCol;
                        check2 = true;
                    }
                    else if (val >= 0.2f && val <= 0.3f && check3 == false)
                    {
                        ValueImages[2].color = thisCol;
                        check3 = true;
                    }
                    else if (val >= 0.3f && val <= 0.4f && check4 == false)
                    {
                        ValueImages[3].color = thisCol;
                        check4 = true;
                    }
                    else if (val >= 0.4f && val <= 0.5f && check5 == false)
                    {
                        ValueImages[4].color = thisCol;
                        check5 = true;
                    }
                    else if (val >= 0.5f && val <= 0.6f && check6 == false)
                    {
                        ValueImages[5].color = thisCol;
                        check6 = true;
                    }
                    else if (val >= 0.6f && val <= 0.7f && check7 == false)
                    {
                        ValueImages[6].color = thisCol;
                        check7 = true;
                    }
                    else if (val >= 0.7f && val <= 0.8f && check8 == false)
                    {
                        ValueImages[6].color = thisCol;
                        check8 = true;
                    }
                    else if (val >= 0.8f && val <= 0.9f && check9 == false)
                    {
                        ValueImages[7].color = thisCol;
                        check9 = true;
                    }
                    else if (val >= 0.9f && val <= 1.0f && check10 == false)
                    {
                        ValueImages[8].color = thisCol;
                        check10 = true;
                    }
                }
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

        public void TakePicture()
        {
            UpdateCameraImage();
            ProcessImage();
            CheckValues();
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
    }
}
