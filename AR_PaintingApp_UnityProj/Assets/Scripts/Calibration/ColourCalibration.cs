using System;
using System.IO;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using OpenCvSharp;

public class ColourCalibration : MonoBehaviour
{
    public Texture2D texture2D;

    public RawImage unchangedImage;
    public RawImage changedImage;

    private Mat hFactor;

    private void Start()
    {
        HomographyCalculation();
        //ChangeColours();
    }

    void HomographyCalculation()
    {
        // CAMERA Colours
        Color camRedRGB;
        ColorUtility.TryParseHtmlString("#d05546", out camRedRGB);
        ConvertToXYZ.CIEXYZ camRed = ConvertToXYZ.ColortoXYZ(camRedRGB);

        Color camGreenRGB;
        ColorUtility.TryParseHtmlString("#378d5c", out camGreenRGB);
        ConvertToXYZ.CIEXYZ camGreen = ConvertToXYZ.ColortoXYZ(camGreenRGB);

        Color camBlueRGB;
        ColorUtility.TryParseHtmlString("#395585", out camBlueRGB);
        ConvertToXYZ.CIEXYZ camBlue = ConvertToXYZ.ColortoXYZ(camBlueRGB);

        Color camYellowRGB;
        ColorUtility.TryParseHtmlString("#f0bd26", out camYellowRGB);
        ConvertToXYZ.CIEXYZ camYellow = ConvertToXYZ.ColortoXYZ(camYellowRGB);
        //print("X: " + newConv.X + " | Y: " + newConv.Y + " | Z: " + newConv.Z);

        Point3d[] src = {
                new Point3d(camRed.X, camRed.Y, camRed.Z),
                new Point3d(camGreen.X, camGreen.Y, camGreen.Z),
                new Point3d(camBlue.X, camBlue.Y, camBlue.Z),
                new Point3d(camYellow.X, camYellow.Y, camYellow.Z)
        };

        // IDEAL Colours
        ConvertToXYZ.CIEXYZ red = ConvertToXYZ.ColortoXYZ(Color.red);
        ConvertToXYZ.CIEXYZ green = ConvertToXYZ.ColortoXYZ(Color.green);
        ConvertToXYZ.CIEXYZ blue = ConvertToXYZ.ColortoXYZ(Color.blue);
        ConvertToXYZ.CIEXYZ yellow = ConvertToXYZ.ColortoXYZ(Color.yellow);

        Point3d[] dst = {
                new Point3d(red.X, red.Y, red.Z),
                new Point3d(green.X, green.Y, green.Z),
                new Point3d(blue.X, blue.Y, blue.Z),
                new Point3d(yellow.X, yellow.Y, yellow.Z)
        };

        InputArray src_pts = InputArray.Create(src);
        InputArray dst_pts = InputArray.Create(dst);

        // 1. Calculate Homography
        hFactor = Cv2.FindHomography(src_pts, dst_pts);
        for (var rowIndex = 0; rowIndex < hFactor.Rows; rowIndex++)
        {
            for (var colIndex = 0; colIndex < hFactor.Cols; colIndex++)
            {
                print($"{hFactor.At<double>(rowIndex, colIndex)} ");
            }
        }


    }

    ConvertToXYZ.CIEXYZ CalibColor(ConvertToXYZ.CIEXYZ Pixel)
    {
        return new ConvertToXYZ.CIEXYZ(
                (Pixel.X * hFactor.At<double>(0, 0) + Pixel.Y * hFactor.At<double>(0, 1) + Pixel.Z * hFactor.At<double>(0, 2)),
                (Pixel.X * hFactor.At<double>(1, 0) + Pixel.Y * hFactor.At<double>(1, 1) + Pixel.Z * hFactor.At<double>(1, 2)),
                (Pixel.X * hFactor.At<double>(2, 0) + Pixel.Y * hFactor.At<double>(2, 1) + Pixel.Z * hFactor.At<double>(2, 2))
                );
    }

    void ChangeColours()
    {
        Texture2D newTex = new Texture2D(texture2D.width, texture2D.height);
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                Color thisCol = texture2D.GetPixel(i, j);
                ConvertToXYZ.CIEXYZ newConv = ConvertToXYZ.ColortoXYZ(thisCol);
                //print(newCol);
                ConvertToXYZ.CIEXYZ calib = CalibColor(newConv);
                //Color newCol = new Color((float)newConv.X, (float)newConv.Y, (float)newConv.Z);
                Color newCol = ConvertToXYZ.XYZtoColor(newConv);
                //print(newCol);
                newTex.SetPixel(i, j, newCol);
            }
        }
        newTex.Apply();
        texture2D = newTex;
        changedImage.texture = newTex;
        byte[] itemBGBytes = newTex.EncodeToPNG();
        File.WriteAllBytes("../AR_PaintingApp_UnityProj/Assets/Images/ColourAnalysisImages/CalibColours.png", itemBGBytes);
    }
}
