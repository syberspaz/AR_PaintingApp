using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchAndHoldMenu : MonoBehaviour
{
    public float TimeToHold;

    private float internalHoldTimer;

    public float TimeToDismiss;

    private float internalDismissTimer;

    public bool isMenuActive = false;

    [SerializeField]
    private GameObject menuObject;

    //this script just spawns and despawns an object based on touch + hold controls, mostly just used for 1 menu

    void Update()
    {
        menuObject.SetActive(isMenuActive);
        CheckForTouchHold();

    }

    private void CheckForTouchHold()
    {
        Touch touch = Input.GetTouch(0);

        if (!isMenuActive)
        {
            if (touch.phase == TouchPhase.Stationary)
            {
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
                internalDismissTimer += Time.deltaTime;
            }

            if (internalDismissTimer >= TimeToDismiss)
            {
                isMenuActive = false;
                internalDismissTimer = 0;
            }
        }



    }

}
