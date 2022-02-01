using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveToolsManager : MonoBehaviour
{
    [SerializeField]
    public GameObject[] ToolGameObjects;

    [SerializeField]
    public GameObject[] ImagePanels;

    public void Update()
    {
        for (int i = 0; i < ToolGameObjects.Length; i++)
        {
            if (ToolGameObjects[i].activeSelf)
                ImagePanels[i].SetActive(true);
            else
                ImagePanels[i].SetActive(false);
        }
    }
}
