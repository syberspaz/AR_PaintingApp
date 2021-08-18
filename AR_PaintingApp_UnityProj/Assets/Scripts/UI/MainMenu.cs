using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    //just ui code
    public void LaunchLearnMode()
    {
        SceneManager.LoadScene("LessonPicker");
    }
    public void LaunchExploreMode()
    {
        SceneManager.LoadScene("ExploreMode");//Explore mode is scene 1
    }

    public void LaunchTutorial()
    {
        SceneManager.LoadScene("Tutorials");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainScreen");
    }


}
