using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMenuScript : MonoBehaviour
{

    [SerializeField]
    List<int> TutorialSceneIndex;

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainScreen");
    }

    public void ReturnToTutorialMainMenu()
    {
        SceneManager.LoadScene("MainTutorialMenu");
    }

    public void LoadTutorialScene(int TutorialNumber)
    {
        SceneManager.LoadScene(TutorialSceneIndex[TutorialNumber]);
    }
}
