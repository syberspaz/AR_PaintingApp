using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    //just ui code
    public void LaunchLearnMode()
    {
        //put the code here to switch to learn mode
    }
    public void LaunchExploreMode()
    {
        SceneManager.LoadScene(1);//Explore mode is scene 1
    }


}
