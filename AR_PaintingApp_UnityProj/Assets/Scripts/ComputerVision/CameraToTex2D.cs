using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CameraToTex2D : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The ARCameraManager which will produce frame events.")]
    ARCameraManager m_CameraManager;

    public ARCameraManager cameraManager
    {
        get => m_CameraManager;
        set => m_CameraManager = value;
    }

    [SerializeField]
    Texture2D m_RawCameraImage;

    [SerializeField]
    Text RGBText;

    [SerializeField]
    Text ImageInfoText;

    [SerializeField]
    Text RawInputText;

    [SerializeField]
    Text TouchCoordText;

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
            m_CameraTexture = new Texture2D(image.width , image.height, format, false);
        }

        ImageInfoText.text = image.dimensions.ToString();

        // Convert the image to format, flipping the image across the Y axis.
        // We can also get a sub rectangle, but we'll get the full image here.
        var conversionParams = new XRCpuImage.ConversionParams(image, format);

        // Texture2D allows us write directly to the raw texture data
        // This allows us to do the conversion in-place without making any copies.
        var rawTextureData = m_CameraTexture.GetRawTextureData<Byte>();
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

        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }
        //updates debug text
        RawInputText.text = touchPosition.ToString();

        //converts the touch input from screen space to within our image (camera input)
        touchPosition.x = touchPosition.x / Screen.width * image.width;
        touchPosition.y = touchPosition.y / Screen.height * image.height;

        //updates debug text
        TouchCoordText.text = touchPosition.ToString();

        //get the pixel gathered from the touch screen
        RGBText.text = m_CameraTexture.GetPixel((int)touchPosition.x  , (int)touchPosition.y ).ToString();
    }
    void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        UpdateCameraImage();
    }

    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;


    }


    Texture2D m_CameraTexture;
}
