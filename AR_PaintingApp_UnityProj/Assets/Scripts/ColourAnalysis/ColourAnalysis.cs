using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourAnalysis : MonoBehaviour
{
    [SerializeField]
    Texture2D texture2D;


    public Color monoColor;

    [SerializeField]
    bool checkColourKey;

    [SerializeField]
    bool isHighKey;

    [SerializeField]
    bool isLowKey;

    [SerializeField]
    bool isMidKey;

    [SerializeField]
    bool useSatRange;

    [SerializeField]
    float startRange;

    [SerializeField]
    float endRange;

    [SerializeField]
    //Text ErrPercentageText;

    // Start is called before the first frame update
    void Start()
    {
        Texture2D changedTexture2D = new Texture2D(512, 512);
        float hue, sat, val;
        Color.RGBToHSV(monoColor, out hue, out sat, out val);
        print("Hue: " + hue + " | Sat: " + sat + " | Val: " + val);
        int outsideColCounter = 0;
        float hueDiff = Map(10, 0.0f, 360.0f, 0.0f, 1.0f);
        float satDiff = Map(6, 0.0f, 100.0f, 0.0f, 1.0f);
        float valDiff = Map(6, 0.0f, 100.0f, 0.0f, 1.0f);

        float numPixels = texture2D.width * texture2D.height;
        for (int i = 0; i < texture2D.height; i++)
        {
            for (int j = 0; j < texture2D.width; j++)
            {
                Color thisCol = texture2D.GetPixel(i, j);
                float hue2, sat2, val2;
                Color.RGBToHSV(thisCol, out hue2, out sat2, out val2);
                if (!checkColourKey)
                {
                    if (useSatRange)
                    {
                        if ((hue2 < hue - hueDiff || hue2 > hue + hueDiff) || (val2 < val - valDiff || val2 > val + valDiff) || (sat2 < startRange || sat2 > endRange))
                        {
                            changedTexture2D.SetPixel(i, j, Color.HSVToRGB(hue2, sat2, 0.05f));
                            outsideColCounter++;
                        }
                        else
                        {
                            changedTexture2D.SetPixel(i, j, thisCol);
                        }
                    }
                    else
                    {
                        if ((hue2 < hue - hueDiff || hue2 > hue + hueDiff) || (val2 < val - valDiff || val2 > val + valDiff))
                        {
                            changedTexture2D.SetPixel(i, j, Color.HSVToRGB(hue2, sat2, 0.08f));
                            outsideColCounter++;
                        }
                        else
                        {
                            changedTexture2D.SetPixel(i, j, thisCol);
                        }
                    }
                }
                else
                {
                    if (isHighKey)
                    {
                        if (val2 < 0.5f)
                        {
                            changedTexture2D.SetPixel(i, j, Color.HSVToRGB(hue2, sat2, 0.08f));
                            outsideColCounter++;
                        }
                        else
                        {
                            changedTexture2D.SetPixel(i, j, thisCol);
                        }
                    }
                    if (isMidKey)
                    {

                        if (val2 <= 0.4f || val2 >= 0.6f)
                        {
                            changedTexture2D.SetPixel(i, j, Color.HSVToRGB(hue2, sat2, 0.08f));
                            outsideColCounter++;
                        }
                        else
                        {
                            changedTexture2D.SetPixel(i, j, thisCol);
                        }
                    }
                    if (isLowKey)
                    {

                        if (val2 >= 0.5f)
                        {
                            changedTexture2D.SetPixel(i, j, Color.HSVToRGB(hue2, sat2, 0.08f));
                            outsideColCounter++;
                        }
                        else
                        {
                            changedTexture2D.SetPixel(i, j, thisCol);
                        }
                    }
                }
            }
        }
        changedTexture2D.Apply();
        float errPer = outsideColCounter / numPixels * 100.0f;
        //ErrPercentageText.text = "Error: " + (int)errPer + "%";
        print("Outside Col Counter: " + outsideColCounter);
    }

    public float Map(float value, float min1, float max1, float min2, float max2)
    {
        return min2 + (max2 - min2) * ((value - min1) / (max1 - min1));
    }
}
