using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadFromGallery : MonoBehaviour
{
    [SerializeField]
    private Renderer TextureCanvasRenderer;

    [SerializeField]
    private Transform objectTrans;

    private float aspectRatio;


    [SerializeField]
    Texture2D testTexture;
   

    public void ImageFromGallery()
    {
        //get texture applied
        PickImage(0);
        
       

    }

    private void PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture tex = NativeGallery.LoadImageAtPath(path, maxSize, false);
                aspectRatio = tex.height / (float)tex.width;
                TextureCanvasRenderer.material.mainTexture = tex;
            }
        }, "Select a PNG image", "image/png");

        Debug.Log("Permission result: " + permission);
    }

    private void Update()
    {
        TextureCanvasRenderer.material.mainTexture = testTexture;

        Texture newTex = TextureCanvasRenderer.material.mainTexture;

        aspectRatio = (float)newTex.width / (float)newTex.height;

        Vector3 newScale = new Vector3(aspectRatio, 1, 1);
        //newScale = newScale / (float)2;
        objectTrans.localScale = newScale;
        objectTrans.position = new Vector3(0, 0, 3.28f);
        objectTrans.rotation = Quaternion.Euler(0, 180, 0);
    }
}
