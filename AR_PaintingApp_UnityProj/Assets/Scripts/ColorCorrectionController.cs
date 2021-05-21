using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorCorrectionController : MonoBehaviour
{
    [Tooltip("This is the global volume")]
    [SerializeField]
    private Volume Volume;

    public void ChangeColor()
    {
        ColorAdjustments colorAdjustments;

        Volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);

        colorAdjustments.colorFilter.Override(Color.green); //SetValue(new ColorParameter(Color.blue, true, false, true));




      
    }
}
