using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PictureFromDevCamera : MonoBehaviour
{
    //Pannel to draw to
    [SerializeField]
    private RawImage CameraFeedVisual;

    private WebCamTexture cameraTexture;

	[SerializeField]
	private Renderer mainImageRenderer;

	[SerializeField]
	private Marker userMarkerHandler;


    // Start is called before the first frame update
    void Start()
    {
		WebCamDevice[] devices = WebCamTexture.devices;

		if (devices.Length == 0)
			return;

		for (int i = 0; i < devices.Length; i++)
		{
			var curr = devices[i];

			if (curr.isFrontFacing == false)
			{
				cameraTexture = new WebCamTexture(curr.name, Screen.width, Screen.height);
				break;
			}
		}

		if (cameraTexture == null)
			return;

		cameraTexture.Play(); // Start the camera
		CameraFeedVisual.texture = cameraTexture; // Set the texture
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void TakePicture()
    {
		Color[] colors = cameraTexture.GetPixels();
		Texture2D newTex = new Texture2D(cameraTexture.width, cameraTexture.height);
		newTex.SetPixels(colors);
		newTex.Apply();
		mainImageRenderer.material.mainTexture = newTex;
		userMarkerHandler.restoreTex = newTex;

    }
}
