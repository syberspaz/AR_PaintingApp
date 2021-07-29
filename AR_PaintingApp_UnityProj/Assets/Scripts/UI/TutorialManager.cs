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
    private GameObject movementTutLocation;

    [SerializeField]
    private GameObject imageSearchUIObject;

    [SerializeField]
    private GameObject paletteMenu;

    [SerializeField]
    private GameObject perspectiveLinesObject;

    private int PaletteMenuTutProgress = 0;

    private int CaliperTutProgress = 0;

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
            PaletteMenuTutBubble.SetActive(true);
            PaletteMenuTutProgress = 0; //resets the tutorial
        }

        DetectCaliperTutorial();

        if (CaliperTutProgress == 1)
        {

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
            }
        }
    }

    public void SkipTutorial()
    {
        //just opens directly to explore mode
        SceneManager.LoadScene(2);
    }

}
