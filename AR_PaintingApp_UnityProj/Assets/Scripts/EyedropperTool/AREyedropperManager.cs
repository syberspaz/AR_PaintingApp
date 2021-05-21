using System;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    public class AREyedropperManager : MonoBehaviour
    {
        Texture2D m_CameraTexture;

        Color thisCol;
        Texture2D CamTex;
        Texture2D ResizedCamTex;
        Button currentButton;
        Color currentColor;

        public ARCameraManager cameraManager;

        public RawImage rawColourViewer;

        public GameObject coloursGameObject;

        public RawImage Complementary;

        public RawImage Monochromatic;

        public RawImage Analogous1;
        public RawImage Analogous2;

        public RawImage Triadic1;
        public RawImage Triadic2;

        public RawImage Tetradic1;
        public RawImage Tetradic2;
        public RawImage Tetradic3;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                thisCol = ResizedCamTex.GetPixel((int)TextureScale.Map(Input.GetTouch(0).position.x, 0, Screen.currentResolution.width, 0, ResizedCamTex.width), (int)TextureScale.Map(Input.GetTouch(0).position.y, 0, Screen.currentResolution.height, 0, ResizedCamTex.height));
                currentButton.image.color = thisCol;
                currentColor = thisCol;
                currentButton.GetComponentInChildren<Text>().text = "Hexcode: #" + ColorUtility.ToHtmlStringRGB(thisCol);
                currentButton = null;
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
            CamTex = newTex;
            ResizedCamTex = newTex;
            TextureScale.Point(ResizedCamTex, m_CameraTexture.width / 5, m_CameraTexture.height / 5);
            newTex.Apply();
        }

        void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
        {
            UpdateCameraImage();
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
            rawColourViewer.texture = CamTex;
            if (rawColourViewer.gameObject.activeSelf == false)
            {
                rawColourViewer.gameObject.SetActive(true);
            }
        }

        public void UpdateColour(GameObject thisBtn)
        {
            UpdateCameraImage();
            currentButton = thisBtn.GetComponent<Button>();
            currentButton.image.color = Color.black;
        }

        public void ShowColourOptions()
        {
            if (coloursGameObject.activeSelf == true)
            {
                coloursGameObject.SetActive(false);
            }
            else if (coloursGameObject.activeSelf == false)
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
