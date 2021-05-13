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
    using UnityEngine.UIElements;

    public class CanvasScanner : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The ARCameraManager which will produce frame events.")]
        ARCameraManager m_CameraManager;

        public new Camera camera;

        private Texture2D m_CameraTexture;
        private Texture2D SourceImage;
        public RawImage outputImage;

        //Gameobjects used for user corner position
        [SerializeField]
        private GameObject TopLeft;
        [SerializeField]
        private GameObject TopRight;
        [SerializeField]
        private GameObject BotLeft;
        [SerializeField]
        private GameObject BotRight;

        public bool UsingTestImage = false;

        [SerializeField]
        private Texture2D testImage;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private Text canvasDebugText;

        [SerializeField]
        private Text TopLeftDebugText;

        public ARCameraManager cameraManager
        {
            get => m_CameraManager;
            set => m_CameraManager = value;
        }

        void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
        {
            


            UpdateCameraImage();
            // UpdateHumanDepthImage();
            //   UpdateHumanStencilImage();
            //   UpdateEnvironmentDepthImage();
            //  UpdateEnvironmentDepthConfidenceImage();
        }

     

        void OnEnable()
        {
            if (m_CameraManager != null)
            {
                m_CameraManager.frameReceived += OnCameraFrameReceived;
            }
        }

        void OnDisable()
        {
            if (m_CameraManager != null)
            {
                m_CameraManager.frameReceived -= OnCameraFrameReceived;
            }
        }

        unsafe void UpdateCameraImage()
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
            var conversionParams = new XRCpuImage.ConversionParams(image, format);

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

            // Set the RawImage's texture so we can visualize it.
            SourceImage = m_CameraTexture;

          

           

        }

       


        public bool Contains<T>( T[] array, T obj)
        {
            return ((IList<T>)array).Contains(obj);
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
            if (!Contains(lefts,tops[0]))
                Swap(output, 0, 1);
            if (!Contains(rights,bottoms[0]))
               Swap(output,2, 3);

            // done
            return output;
        }
        public static void Swap<T>(T[] array, int i1, int i2)
        {
            T v = array[i2];
            array[i2] = array[i1];
            array[i1] = v;
        }

        // Start is called before the first frame update
        void Start()
        {


        }

        private bool TryGetTouchPosition(out Vector2 touchPosition)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touchPosition = Input.GetTouch(0).position;
                return true;
            }
            touchPosition = Input.GetTouch(0).position;
            return false;


        }

   
        

        public void CropImageFromUserProvidedCorners()
        {

            Mat ResultMat;
            Mat WarpedMat;
            Point2f[] corners = new Point2f[4];
            if (!UsingTestImage)
            {

             
                 ResultMat = Unity.TextureToMat(SourceImage);
                 WarpedMat = Unity.TextureToMat(SourceImage);

                corners[0].X = TopLeft.GetComponent<RectTransform>().position.x / canvas.pixelRect.size.x * WarpedMat.Width;
                corners[0].Y = TopLeft.GetComponent<RectTransform>().position.y / canvas.pixelRect.size.y * WarpedMat.Height;

                corners[1].X = TopRight.GetComponent<RectTransform>().position.x / canvas.pixelRect.size.x * WarpedMat.Width;
                corners[1].Y = TopRight.GetComponent<RectTransform>().position.y / canvas.pixelRect.size.y * WarpedMat.Height;

                corners[2].X = BotRight.GetComponent<RectTransform>().position.x / canvas.pixelRect.size.x * WarpedMat.Width;
                corners[2].Y = BotRight.GetComponent<RectTransform>().position.y / canvas.pixelRect.size.y * WarpedMat.Height;

                corners[3].X = BotLeft.GetComponent<RectTransform>().position.x / canvas.pixelRect.size.x * WarpedMat.Width;
                corners[3].Y = BotLeft.GetComponent<RectTransform>().position.y / canvas.pixelRect.size.y * WarpedMat.Height;

            }
            else
            {
                 ResultMat = Unity.TextureToMat(testImage);
                 WarpedMat = Unity.TextureToMat(testImage);



                corners[0].X = TopLeft.transform.position.x / canvas.pixelRect.size.x * WarpedMat.Width;
                corners[0].Y = TopLeft.transform.position.y / canvas.pixelRect.size.y * WarpedMat.Height;

                corners[1].X = TopRight.transform.position.x / canvas.pixelRect.size.x * WarpedMat.Width;
                corners[1].Y = TopRight.transform.position.y / canvas.pixelRect.size.y * WarpedMat.Height;

                corners[2].X = BotRight.transform.position.x / canvas.pixelRect.size.x * WarpedMat.Width;
                corners[2].Y = BotRight.transform.position.y / canvas.pixelRect.size.y * WarpedMat.Height;

                corners[3].X = BotLeft.transform.position.x / canvas.pixelRect.size.x * WarpedMat.Width;
                corners[3].Y = BotLeft.transform.position.y / canvas.pixelRect.size.y * WarpedMat.Height;
            }

            //TopLeftDebugText.text = WarpedMat.Width.ToString() + WarpedMat.Height.ToString();
            // outputImage.texture = Unity.MatToTexture(ResultMat);



           


            if (!UsingTestImage)
            WarpedMat = UnwrapShape(ResultMat, corners);
            else
            {
            WarpedMat = UnwrapShape(Unity.TextureToMat(testImage), corners);
            }
            outputImage.texture = Unity.MatToTexture(WarpedMat);
        }


        public void ProcessContoursFromCamera()
        {

            Mat sourceMat = Unity.TextureToMat(SourceImage);
            Mat ResultMat = Unity.TextureToMat(SourceImage);

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

            //Cv2.DrawContours(ResultMat, contours, -1, new OpenCvSharp.Scalar(255,0,0), 4, LineTypes.Link8);

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
                //dirty_ = false;
                ResultMat = Unity.TextureToMat(SourceImage);
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

            //  Cv2.WarpPerspective(sourceMat, SourceImage,  );





            //rawImage.texture = Unity.MatToTexture(sourceMat);
            outputImage.texture = Unity.MatToTexture(matUnwrapped);
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
        public Mat UnwrapShape( Mat img, Point2f[] corners, int maxSize = 0)
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


    }
}
