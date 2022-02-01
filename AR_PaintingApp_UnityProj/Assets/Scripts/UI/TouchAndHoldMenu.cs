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
    private GameObject userPlaneUiObject;

    [SerializeField]
    public Image circleUIObject;

    [SerializeField]
    private RectTransform canvas;

    [SerializeField]
    private Camera uiCam;

    public float TimeDelay;

    public float InternalDelayTimer;

    private bool CanTogglePlane = true;

    private bool isDelay = false;

    private bool didVibrate = false;

    private int TouchCount;



    //this script just spawns and despawns an object based on touch + hold controls, mostly just used for 1 menu and the user plane
    //also manages the icon that shows how close to open it is

    void Update()
    {



    
        menuObject.SetActive(isMenuActive);
        CheckForTouchHold();


    }

    private void CheckForTouchHold()
    {

        

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Ended || touch.deltaPosition.magnitude > 60)
        {
            InternalDelayTimer = 0f;
            isDelay = false;
            didVibrate = false;
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
                    if (!didVibrate)
                    {
                        didVibrate = true;
                        Handheld.Vibrate();
                    }
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
                circleUIObject.fillAmount = 0f;
               isMenuActive = true;
                internalHoldTimer = 0;
                isDelay = false;
                didVibrate = false; 
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


                    if (!didVibrate)
                    {
                        didVibrate = true;
                        Handheld.Vibrate();
                    }

                    Vector2 newPos;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, touch.position, uiCam, out newPos);
                    circleUIObject.rectTransform.anchoredPosition = newPos;
                    circleUIObject.fillAmount = (internalDismissTimer / TimeToDismiss);


                    internalDismissTimer += Time.deltaTime;
                }
            }

            if (internalDismissTimer >= TimeToDismiss)
            {
                circleUIObject.fillAmount = 0f;
                isMenuActive = false;
                internalDismissTimer = 0;
                didVibrate = false;
                isDelay = false;
            }
        }


       
    }


    private void CheckForPlaneTouchHold()
    {

        /*
        Touch touch = Input.GetTouch(0);

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(touch.position);

        Physics.Raycast(ray, out hit);

        if (hit.transform.gameObject.tag == "UserToolPlane" && CanTogglePlane && touch.phase == TouchPhase.Began)
        {
            PlaceUserPlane userplane;
            hit.transform.gameObject.TryGetComponent<PlaceUserPlane>(out userplane);

            if (userPlaneUiObject.activeSelf == true)
            {
                userplane.RotationActive = false;
                userplane.LocationActive = false;
                userplane.ScaleActive = false;
            }


        }
        */
    }


    public void DisableMenu()
    { 
        isMenuActive = false;
    }

}
