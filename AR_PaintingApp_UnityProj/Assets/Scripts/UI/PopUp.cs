using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class PopUp : MonoBehaviour
{
    //this class handles the pop ups that give small bits of info to the user
    //handles text as well as the dismiss button
    [SerializeField]
    public TextMeshProUGUI PopupText;

    public void DismissPopup()
    {
        Destroy(gameObject, 0.2f);
    }

    public void ChangeText(string newText)
    {
        PopupText.SetText(newText);
    }
    
}
