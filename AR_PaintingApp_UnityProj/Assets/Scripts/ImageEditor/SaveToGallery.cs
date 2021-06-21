using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SaveToGallery : MonoBehaviour
{
    [SerializeField]
    private Renderer MainImageRenderer;

    [SerializeField]
    RawImage test;

    //debug
    [SerializeField]
    public Text debugText;

    private void SaveCurrentImage()
    {

        Texture mainCanvasTexture = MainImageRenderer.material.mainTexture;

        Texture2D tex2D = (Texture2D)mainCanvasTexture;

        tex2D.Apply();

        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(tex2D.EncodeToPNG(), "ARPaintingApp", "Image.png");

        debugText.text = permission.ToString();

        Destroy(tex2D);
    }
    
    public void  SaveButtonPressed()
    {
        SaveCurrentImage();
    }

	public void LoadButtonPressed()
    {
        PickImage(2048);
    }


	private void PickImage(int maxSize)
	{
		NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
		{
			Debug.Log("Image path: " + path);
			if (path != null)
			{
				// Create Texture from selected image
				Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize, false);

                //texture.Apply();
                MainImageRenderer.material.mainTexture = texture;


			}
		}, "Select a PNG image", "image/png");

		Debug.Log("Permission result: " + permission);
	}


}
