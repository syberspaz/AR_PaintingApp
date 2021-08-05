using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    
    [SerializeField]
    private GameObject caliperObject;

    [SerializeField]
    private GameObject caliperUIObject;

    [SerializeField]
    private GameObject movementTutLocation;

    [SerializeField]
    private GameObject imageSearchUIObject;

    [SerializeField]
    private GameObject paletteMenu;

    [SerializeField]
    private GameObject perspectiveLinesObject;
    [SerializeField]
    private TogglePlaneDetection toggler;

    

    private int PaletteMenuTutProgress = 0;

    private int CaliperTutProgress = 0;

    private int PersLineTutProgress = 0;

    private int SearchTutProgress = 0;

    [SerializeField]
    private GameObject PaletteMenuTutBubble;

    [SerializeField]
    private GameObject CaliperTutBubble;

    [SerializeField]
    private GameObject ImageSearchTutBubble;

    [SerializeField]
    private GameObject PerspectiveLinesTutBubble;

    // Update is called once per frame
    void Update()
    {
        DetectPaletteMenuTutorial();

        if (PaletteMenuTutProgress == 2)
        {
            paletteMenu.SetActive(false);
            PaletteMenuTutBubble.SetActive(true);
            PaletteMenuTutProgress = 0; //resets the tutorial
        }

        DetectCaliperTutorial();

        if (CaliperTutProgress == 1)
        {
            caliperObject.SetActive(false);
            caliperUIObject.SetActive(false);
            movementTutLocation.SetActive(false);
            CaliperTutBubble.SetActive(true);
            CaliperTutProgress = 0;
        }

        if (PersLineTutProgress == 1)
        {
            PerspectiveLinesTutBubble.SetActive(true);
            perspectiveLinesObject.SetActive(false);
            PersLineTutProgress = 0;
        }

        if (SearchTutProgress == 1)
        {
            ImageSearchTutBubble.SetActive(true);
            imageSearchUIObject.SetActive(false);
            SearchTutProgress = 0;
        }

    }

    public void DetectPaletteMenuTutorial()
    {
        if (paletteMenu.activeSelf && PaletteMenuTutProgress == 0) // menu opened for first time
        {
            PaletteMenuTutProgress = 1;
        }
        else if (!paletteMenu.activeSelf && PaletteMenuTutProgress == 1) //menu closed
        {
            PaletteMenuTutProgress = 2;
        }
    }

    public void DetectCaliperTutorial()
    {
        if (Vector3.Distance(caliperObject.transform.position, movementTutLocation.transform.position) <= 1)
        {
            if (caliperObject.GetComponent<CaliperController>().Openness >= 0.95f)
            {
                CaliperTutProgress = 1;
                movementTutLocation.transform.position = new Vector3(100, 100, 100);
            }
        }
    }

    public void SkipTutorial()
    {
        //just opens directly to explore mode
        SceneManager.LoadScene(2);
    }

    public void PerspectiveLinesTutorialCheck()
    {
     

        GameObject[] PerspectiveLines = GameObject.FindGameObjectsWithTag("PerspectiveLine");

        if (PerspectiveLines.Length >= 1)
        {

            for (int i = 0; i < PerspectiveLines.Length; i++)
            {
                GameObject.Destroy(PerspectiveLines[i], 0.0f);
            }
            toggler.PlaneDetectionToggle();
            PersLineTutProgress = 1;

        }
    }

    public void ImageSearchTutorialCheck()
    {
        Debug.Log(":)");
        GameObject.Destroy(GameObject.Find("ReferenceImageQuad"));
        SearchTutProgress = 1;
    }


}
