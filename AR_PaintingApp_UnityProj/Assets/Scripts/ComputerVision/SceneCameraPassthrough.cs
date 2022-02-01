using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

//Basically a passthrough so all scripts that need the camera can get their image feed from 1 script,
//instead of each script having its own camera feed
public class SceneCameraPassthrough : MonoBehaviour
{
    public Texture2D camOutput;
    
    [SerializeField]
    [Tooltip("The ARCameraManager which will produce frame events.")]
    public ARCameraManager m_CameraManager;
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

        if (camOutput == null || camOutput.width != image.width || camOutput.height != image.height)
        {
            camOutput = new Texture2D(image.width, image.height, format, false);
        }

        // Convert the image to format, flipping the image across the Y axis.
        // We can also get a sub rectangle, but we'll get the full image here.
        var conversionParams = new XRCpuImage.ConversionParams(image, format);

        // Texture2D allows us write directly to the raw texture data
        // This allows us to do the conversion in-place without making any copies.
        var rawTextureData = camOutput.GetRawTextureData<byte>();
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
        camOutput.Apply();

    }

}
