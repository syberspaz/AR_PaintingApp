using UnityEngine;
using UnityEngine.Android;

public class AndroidAskPermissions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(Application.platform == RuntimePlatform.Android) //only ask for android permissions if running on android
        {
            if (Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                // The user authorized use of the microphone.
            }
            else
            {
                Permission.RequestUserPermission(Permission.Camera);
            }
        }
    }

}
