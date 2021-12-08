using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class UserPlaneFromARPlane : MonoBehaviour
{

    public bool isEnabled;
    public bool isRotationEnabled;



    [SerializeField]
    private Text ToggleButtonText;

    [SerializeField]
    private Text RotationLockText;

    [SerializeField]
    private GameObject RotationButton;



    // Start is called before the first frame update
    void Start()
    {

       

    }

    // Update is called once per frame
    void Update()
    {

        RotationButton.SetActive(isEnabled);

        if (isEnabled)
        {

            Touch touch = Input.GetTouch(0);

            RaycastHit[] hits;

            if (isRotationEnabled)
            {
                Vector2 deltaPosTouch = touch.deltaPosition;

                Vector3 CurRot = transform.rotation.eulerAngles;

                CurRot.z += deltaPosTouch.x * Time.deltaTime;

                transform.rotation = Quaternion.Euler(CurRot);

            }
            else
            {
                hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(touch.position));

                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.gameObject.tag == "ARPlane")
                    {
                        transform.position = hits[i].point;
                        Vector3 temp = hits[i].transform.rotation.eulerAngles;
                        temp.x += 90;

                        transform.rotation = Quaternion.Euler(temp);
                    }
                }

            }

            //if no touch, don't need to run the rest of the code
        }

    }
    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    public void ToggleEnable()
    {
        if (isEnabled)
        {
            ToggleButtonText.text = "Enable";
        }
        else
        {
            ToggleButtonText.text = "Disable";
        }

        isEnabled = !isEnabled;
    }

    public void ToggleRotation()
    {
       

        if (isRotationEnabled)
        {
            RotationLockText.text = "Switch to Position";
        }
        else
        {
            RotationLockText.text = "Switch To Rotation";
        }

        isRotationEnabled = !isRotationEnabled;
    }


}
