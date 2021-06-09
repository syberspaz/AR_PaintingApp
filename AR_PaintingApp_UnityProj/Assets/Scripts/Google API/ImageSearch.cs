using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System;
using SimpleJSON;


//Handles the UI and searching for the tool that allows users to search for reference images in the app and place them in the scene
public class ImageSearch : MonoBehaviour
{
    [SerializeField]
    private string APIKey; //API key from google custom search engine
    [SerializeField]
    private string SearchEngineID; //Also from google, the id for the custom search engine
    [SerializeField]
    private InputField SearchBarInput;

    private string FullLinkString = string.Empty; //this is what will get sent into the webrequest

    private const string SearchEngineUrl = "https://www.googleapis.com/customsearch/v1?";//Add parameters after this, this will not change 

    //These parameters ALWAYS need to be included after the URL, and these 3 are ALWAYS required
    //Other parameters will be added to string as needed
    private const string KeyPrefix = "key=";
    private const string IDPrefix = "&cx=";
    private const string QueryPrefix = "&q=";

    //since we just want image results, this is not a required parameter
    //also defining the value
    private const string ImagePrefix = "&searchType=image";

    //This will get taken from the input field
    private string SearchQuery;

    private List<string> imageLinks = new List<string>(); //

    [SerializeField]
    private List<Image> imagePannels;

    private string DataFromWebReq;


    public void SearchButtonPress()
    {
        FullLinkString = string.Empty;
        FullLinkString += SearchEngineUrl; //Adds the base url
        FullLinkString += KeyPrefix; //Adds prefix for the key
        FullLinkString += APIKey; //Adds the API key
        FullLinkString += IDPrefix; //Adds the prefix for the Search Engine ID
        FullLinkString += SearchEngineID; //Adds the Search Engine ID
        FullLinkString += QueryPrefix; //Adds the prefix for Query
        FullLinkString += SearchBarInput.text; //Adds the input from the search bar
        FullLinkString += ImagePrefix; //Sets the search to be for images only
        Debug.Log(FullLinkString);

        StartCoroutine(GetRequest(FullLinkString));



       

        //StartCoroutine(LoadAllImagesFromWeb());

    }
    public void  GetWebResults()
    {
        
        imageLinks.Clear();

            string path = "Assets/Text Files/SearchResults.txt";

            if (File.Exists(path)) //If the file already exists, delete it before writing the search results
            {
                File.Delete(path);
            }

            //writes to actual JSON file for testing
            StreamWriter writer = new StreamWriter(path, true);
            writer.Write(DataFromWebReq);
            //string link;
            Debug.Log(DataFromWebReq);

            var N = JSON.Parse(DataFromWebReq);
            
            for (int i = 0; i < 8; i++)
            {
                string link = string.Empty;
                link = string.Empty;
                link = N["items"][i]["link"].Value;
                imageLinks.Add(link);
                Debug.Log(link);
            }

        for (int i = 0; i < imagePannels.Count; i++)
        {
            StartCoroutine(LoadImageFromWeb(i));
        }
    }








    IEnumerator LoadImageFromWeb(int index)
    {


        UnityWebRequest req = UnityWebRequestTexture.GetTexture(imageLinks[index]);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(req.error);
        }
        else
        {
            Debug.Log(imageLinks[index]);
            
            if (((DownloadHandlerTexture)req.downloadHandler).texture != null)
            {

                //if there is no error we can set the texture
                Texture2D img = ((DownloadHandlerTexture)req.downloadHandler).texture;
                imagePannels[index].sprite = Sprite.Create(img, new Rect(0, 0, 400, 400), Vector2.zero);
            }
            else
            {
                Debug.Log("Texture was invalid");
            }

        }


       
    }

    //Loads all the results into a JSON file
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    DataFromWebReq = webRequest.downloadHandler.text;
                    Debug.Log(webRequest.downloadHandler.text);
                    break;
            }
        }

        GetWebResults();
    }

}

