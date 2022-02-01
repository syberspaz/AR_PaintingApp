using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

//Class for single image with link
public class ImageFromWeb : MonoBehaviour
{
    public Texture2D img;
    public Image profilePic;
    public InputField _url;
    public void buttonClick()
    {
        StartCoroutine(GetImageFromWeb(_url.text));
    }

    IEnumerator GetImageFromWeb(string url)
    {
        UnityWebRequest req = UnityWebRequestTexture.GetTexture(url);

        yield return req.SendWebRequest();

        //Check for error
        if(req.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(req.error);
        }
        else
        {
            //if there is no error we can set the texture
            img = ((DownloadHandlerTexture)req.downloadHandler).texture;
        }
    }
}
