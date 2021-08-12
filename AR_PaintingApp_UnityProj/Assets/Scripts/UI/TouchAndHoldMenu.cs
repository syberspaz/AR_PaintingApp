using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchAndHoldMenu : MonoBehaviour
{
    public float TimeToHold;

    public float internalHoldTimer;

    public float TimeToDismiss;

    public float internalDismissTimer;

    public bool isMenuActive = false;

    [SerializeField]
    private GameObject menuObject;

    [SerializeField]
    public Image circleUIObject;

    [SerializeField]
    private RectTransform canvas;

    [SerializeField]
    private Camera uiCam;

    [SerializeField]
    private Text debugText;

    //this script just spawns and despawns an object based on touch + hold controls, mostly just used for 1 menu
    //also manages the icon that shows how close to open it is

    void Update()
    {



    
        menuObject.SetActive(isMenuActive);
        CheckForTouchHold();

    }

    private void CheckForTouchHold()
    {



        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Ended)
        {
            internalHoldTimer = 0f;
            internalDismissTimer = 0f;
            circleUIObject.fillAmount = 0f;
        }

        if (!isMenuActive)
        {
            if (touch.phase == TouchPhase.Stationary)
            {
                //the filling circle

                //calculate on the canvas where it needs to go
                Vector2 newPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, touch.position, uiCam, out newPos);
                circleUIObject.rectTransform.anchoredPosition = newPos;
                circleUIObject.fillAmount = (internalHoldTimer / TimeToHold);


                internalHoldTimer += Time.deltaTime;
            }

            if (internalHoldTimer >= TimeToHold)
            {
                isMenuActive = true;
                internalHoldTimer = 0;
            }
        }
        else if (isMenuActive)
        {
            if (touch.phase == TouchPhase.Stationary)
            {
                Vector2 newPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, touch.position, uiCam, out newPos);
                circleUIObject.rectTransform.anchoredPosition = newPos;
                circleUIObject.fillAmount = (internalDismissTimer / TimeToDismiss);


                internalDismissTimer += Time.deltaTime;
            }

            if (internalDismissTimer >= TimeToDismiss)
            {
                isMenuActive = false;
                internalDismissTimer = 0;
            }
        }



    }

    public void DisableMenu()
    {
        debugText.text = "Disabling Menu";
        isMenuActive = false;
    }

}
