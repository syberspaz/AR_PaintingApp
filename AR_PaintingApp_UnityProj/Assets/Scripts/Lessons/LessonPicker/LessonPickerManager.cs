using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LessonPickerManager : MonoBehaviour
{
    private int lessonProgress = 0;

    public List<GameObject> Wedges = new List<GameObject>();

    private void Start()
    {
        if (PlayerPrefs.GetInt("LessonProgress") == 0)
        {
            PlayerPrefs.SetInt("LessonProgress", 1);
        }
        else if (PlayerPrefs.GetInt("LessonProgress") > 6)
        {
            PlayerPrefs.SetInt("LessonProgress", 6);
        }

        lessonProgress = PlayerPrefs.GetInt("LessonProgress");

        for (int i = 0; i < lessonProgress * 2; i++)
        {
            Wedges[i].SetActive(true);
        }
    }

    public void ChangeScene(string sceneName)
    {
        SoundEffectManager sounds = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<SoundEffectManager>();
        sounds.PlayLessonStartedSound();
        SceneManager.LoadScene(sceneName);
    }
}
