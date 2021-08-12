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



    private void SaveCurrentImage()
    {

        Texture mainCanvasTexture = MainImageRenderer.material.mainTexture;

        Texture2D tex2D = (Texture2D)mainCanvasTexture;

        tex2D.Apply();

        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(tex2D.EncodeToPNG(), "ARPaintingApp", "Image.png");



        Destroy(tex2D);
    }
    
    public void  SaveButtonPressed()
    {
        SaveCurrentImage();
    }

	


}
