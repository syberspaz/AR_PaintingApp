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

    public float TimeDelay;

    public float InternalDelayTimer;

    public bool isDelay = false;

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

                if (InternalDelayTimer < TimeDelay)
                {
                  

                    InternalDelayTimer += Time.deltaTime;
                    if (InternalDelayTimer > TimeDelay)
                    {

                        isDelay = true;

                    }

                }
                if (isDelay)
                {
                    InternalDelayTimer = 0;
                    //calculate on the canvas where it needs to go
                    Vector2 newPos;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, touch.position, uiCam, out newPos);
                    circleUIObject.rectTransform.anchoredPosition = newPos;
                    circleUIObject.fillAmount = (internalHoldTimer / TimeToHold);


                    internalHoldTimer += Time.deltaTime;
                }
            }

            if (internalHoldTimer >= TimeToHold)
            {
                
                isMenuActive = true;
                internalHoldTimer = 0;
                isDelay = false;
            }
        }
        else if (isMenuActive)
        {
            if (touch.phase == TouchPhase.Stationary)
            {
                if (InternalDelayTimer < TimeDelay)
                {
                    InternalDelayTimer += Time.deltaTime;
                    if (InternalDelayTimer > TimeDelay)
                    {
                        
                        isDelay = true;
                    }

                }
                if (isDelay)
                {
                    InternalDelayTimer = 0;

                    Vector2 newPos;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, touch.position, uiCam, out newPos);
                    circleUIObject.rectTransform.anchoredPosition = newPos;
                    circleUIObject.fillAmount = (internalDismissTimer / TimeToDismiss);


                    internalDismissTimer += Time.deltaTime;
                }
            }

            if (internalDismissTimer >= TimeToDismiss)
            {
                
                isMenuActive = false;
                internalDismissTimer = 0;
                
                isDelay = false;
            }
        }



    }

    public void DisableMenu()
    { 
        isMenuActive = false;
    }

}
