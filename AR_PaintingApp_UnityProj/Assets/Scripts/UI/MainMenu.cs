using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    //just ui code
    public void LaunchLearnMode()
    {
        SceneManager.LoadSceneAsync("LessonPicker");
    }
    public void LaunchExploreMode()
    {
        SceneManager.LoadSceneAsync("ExploreMode");
    }

    public void LaunchTutorial()
    {
        SceneManager.LoadSceneAsync("MainTutorialMenu");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadSceneAsync("MainScreen");
    }

    public void Launch2dImageEditor()
    {
        SceneManager.LoadSceneAsync("2D_Image_Editor");
    }


}
