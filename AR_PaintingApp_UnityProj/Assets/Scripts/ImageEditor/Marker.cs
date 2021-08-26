using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Marker : MonoBehaviour
{
    [SerializeField]
    Camera cam;

    [SerializeField]
    GameObject quad; //main image render plane user interacts with

    [SerializeField]
    FlexibleColorPicker fcp;

    public Texture2D restoreTex;

    public int MarkerSize;

    private float lastX, lastY;

    private bool isFirst = true;

    private bool isRestoreMode = false;

    private void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        Touch touch;
        touch = Input.GetTouch(0);
        

        //if the finger is just touching the screen or if it is already moving on the screen
        if(touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
        {
            if(!isRestoreMode) //if its not restore mode just regular marker code
            { 

            RaycastHit hit;
            if (!Physics.Raycast(cam.ScreenPointToRay(touch.position), out hit))
                return;
            Renderer renderer;
                if (hit.transform.gameObject.TryGetComponent<Renderer>(out renderer)) //do some other checks later to make sure this is main image
                {
                    Texture2D tex = renderer.material.mainTexture as Texture2D;
                    Vector2 pixelUV = hit.textureCoord;
                    pixelUV.x *= tex.width;
                    pixelUV.y *= tex.height;
                    Debug.Log(pixelUV);

                    //declare array of the right size for SetPixels block
                    //must be at least blockWidth*blockHeight (blockDimension is MarkerSize)
                    int ArraySize = MarkerSize * MarkerSize + 1;

                    Color[] colors = new Color[ArraySize];

                    //set all colors in array to be the selection
                    for (int i = 0; i < colors.Length; i++)
                    {
                        
                        colors[i] = fcp.color;
                    }



                    //interpolation to get a smooth line
                    if (!isFirst)
                    {
                        for (float t = 0.01f; t < 1.00f; t += 0.01f)
                        {
                            int lerpX = (int)Mathf.Lerp(lastX, (float)pixelUV.x, t);
                            int lerpY = (int)Mathf.Lerp(lastY, (float)pixelUV.y, t);
                            tex.SetPixels(lerpX, lerpY, MarkerSize, MarkerSize, colors);
                        }
                    }
                    else
                    {
                        isFirst = false;
                    }

                    //This is the actually heavy function call
                    tex.Apply();

                    renderer.material.mainTexture = tex;

                    lastX = pixelUV.x;
                    lastY = pixelUV.y;
                }
                else
                {

                }
            }
            else //restore mode code is almost the same, but instead of getting the draw color from the color picker
            // we get it from a saved copy of the original texture
            {
                RaycastHit hit;
                if (!Physics.Raycast(cam.ScreenPointToRay(touch.position), out hit))
                    return;
                Renderer renderer;
                if (hit.transform.gameObject.TryGetComponent<Renderer>(out renderer)) //do some other checks later to make sure this is main image
                {
                    Texture2D tex = renderer.material.mainTexture as Texture2D;
                    Vector2 pixelUV = hit.textureCoord;
                    pixelUV.x *= tex.width;
                    pixelUV.y *= tex.height;
                    Debug.Log(pixelUV);

                    //declare array of the right size for SetPixels block
                    //must be at least blockWidth*blockHeight (blockDimension is MarkerSize)
                    int ArraySize = MarkerSize * MarkerSize + 1;
                    Color[] colors = new Color[ArraySize];
                    if (!isFirst)
                    {
                        for (float t = 0.01f; t < 1.00f; t += 0.01f)
                        {
                            int lerpX = (int)Mathf.Lerp(lastX, (float)pixelUV.x, t);
                            int lerpY = (int)Mathf.Lerp(lastY, (float)pixelUV.y, t);
                            colors = restoreTex.GetPixels(lerpX, lerpY, MarkerSize, MarkerSize);
                        }
                    }
                    else
                    {
                        isFirst = false;
                    }



                    //interpolation to get a smooth line
                    if (!isFirst)
                    {
                        for (float t = 0.01f; t < 1.00f; t += 0.01f)
                        {
                            int lerpX = (int)Mathf.Lerp(lastX, (float)pixelUV.x, t);
                            int lerpY = (int)Mathf.Lerp(lastY, (float)pixelUV.y, t);
                            tex.SetPixels(lerpX, lerpY, MarkerSize, MarkerSize, colors);
                        }
                    }
                    else
                    {
                        isFirst = false;
                    }

                    //This is the actually heavy function call
                    tex.Apply();

                    renderer.material.mainTexture = tex;

                    lastX = pixelUV.x;
                    lastY = pixelUV.y;
                }
                else
                {

                }
            }
           
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            isFirst = true;
        }
        
    }

    public void SetMarkerSize(System.Single NewSize)
    {
        MarkerSize = (int)NewSize;
    }

    public void ToggleRestoreMode()
    {
        isRestoreMode = !isRestoreMode;
    }
    
    

}
