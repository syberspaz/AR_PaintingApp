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

        Texture2D mainCanvasTexture = MainImageRenderer.material.mainTexture as Texture2D;
       
    

        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(mainCanvasTexture.EncodeToPNG(), "ARPaintingApp", "Image.png");




    }
    
    public void  SaveButtonPressed()
    {
        SaveCurrentImage();
    }

	


}
