namespace OpenCvSharp.Demo
{
	using UnityEngine;
	using OpenCvSharp;
	using UnityEngine.UI;
	public class CanvasScanner : WebCamera
	{
		[SerializeField]
		private RawImage rawCamTex;
		[SerializeField]
		private RawImage Processed;


		protected override void Awake()
		{
			base.Awake();
			//this.forceFrontalCamera = false;
		}

		// Our sketch generation function
		protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
		{
			

			Mat img = Unity.TextureToMat(input, TextureParameters);
			Texture2D myTexture = new Texture2D(input.width, input.height);
			//Convert image to grayscale
			Mat imgGray = new Mat ();
			Cv2.CvtColor (img, imgGray, ColorConversionCodes.BGR2GRAY);
			
			// Clean up image using Gaussian Blur
			Mat imgGrayBlur = new Mat ();
			Cv2.GaussianBlur (imgGray, imgGrayBlur, new Size (5, 5), 0);

			//Extract edges
			Mat cannyEdges = new Mat ();
			Cv2.Canny (imgGrayBlur, cannyEdges, 10.0, 70.0);

			//Do an invert binarize the image
			Mat mask = new Mat ();
			Cv2.Threshold (cannyEdges, mask, 70.0, 255.0, ThresholdTypes.BinaryInv);

			// result, passing output texture as parameter allows to re-use it's buffer
			// should output texture be null a new texture will be created
			output = Unity.MatToTexture(mask, output);

			rawCamTex.texture = Unity.MatToTexture(img, myTexture);
			Processed.texture = Unity.MatToTexture(mask, output);

			return true;
		}
	}
}