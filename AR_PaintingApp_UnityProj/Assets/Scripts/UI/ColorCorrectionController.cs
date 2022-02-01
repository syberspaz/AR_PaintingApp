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
    public FlexibleColorPicker fcp;

    private bool effectToggle = false;

    //Just applies a color correct effect of the same color as the value in the color picker on the ui panel for color tools
    public void Update()
    {
        ColorAdjustments colorAdjustments;

        Volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);

        if (effectToggle)
        colorAdjustments.colorFilter.Override(fcp.color);
        else
        colorAdjustments.colorFilter.Override(Color.white);
    }

    public void ToggleEffect()
    {
        effectToggle = !effectToggle;
    }
}
