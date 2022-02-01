using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConvertToXYZ
{
    /// <summary>
    /// Structure to define CIE XYZ.
    /// </summary>
    public struct CIEXYZ
    {
        /// <summary>
        /// Gets an empty CIEXYZ structure.
        /// </summary>
        public static readonly CIEXYZ Empty = new CIEXYZ();
        /// <summary>
        /// Gets the CIE D65 (white) structure.
        /// </summary>
        public static readonly CIEXYZ D65 = new CIEXYZ(0.9505, 1.0, 1.0890);


        private double x;
        private double y;
        private double z;

        public static bool operator ==(CIEXYZ item1, CIEXYZ item2)
        {
            return (
                item1.X == item2.X
                && item1.Y == item2.Y
                && item1.Z == item2.Z
                );
        }

        public static bool operator !=(CIEXYZ item1, CIEXYZ item2)
        {
            return (
                item1.X != item2.X
                || item1.Y != item2.Y
                || item1.Z != item2.Z
                );
        }

        /// <summary>
        /// Gets or sets X component.
        /// </summary>
        public double X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = (value > 0.9505) ? 0.9505 : ((value < 0) ? 0 : value);
            }
        }

        /// <summary>
        /// Gets or sets Y component.
        /// </summary>
        public double Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = (value > 1.0) ? 1.0 : ((value < 0) ? 0 : value);
            }
        }

        /// <summary>
        /// Gets or sets Z component.
        /// </summary>
        public double Z
        {
            get
            {
                return this.z;
            }
            set
            {
                this.z = (value > 1.089) ? 1.089 : ((value < 0) ? 0 : value);
            }
        }

        public CIEXYZ(double x, double y, double z)
        {
            this.x = (x > 0.9505) ? 0.9505 : ((x < 0) ? 0 : x);
            this.y = (y > 1.0) ? 1.0 : ((y < 0) ? 0 : y);
            this.z = (z > 1.089) ? 1.089 : ((z < 0) ? 0 : z);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

    }

    /// <summary>
    /// Converts RGB to CIE XYZ (CIE 1931 color space)
    /// </summary>
    public static CIEXYZ ColortoXYZ(Color RGB, string white)
    {
        // normalize red, green, blue values
        float rLinear = RGB.r / 1.0f;
        float gLinear = RGB.g / 1.0f;
        float bLinear = RGB.b / 1.0f;

        // convert to a sRGB form
        double r = (rLinear > 0.04045) ? Mathf.Pow((rLinear + 0.055f) / (
            1 + 0.055f), 2.2f) : (rLinear / 12.92f);
        double g = (gLinear > 0.04045) ? Mathf.Pow((gLinear + 0.055f) / (
            1 + 0.055f), 2.2f) : (gLinear / 12.92f);
        double b = (bLinear > 0.04045) ? Mathf.Pow((bLinear + 0.055f) / (
            1 + 0.055f), 2.2f) : (bLinear / 12.92f);

        switch (white)
        {
            case "D65":
                // converts
                return new CIEXYZ(
                    (r * 0.4124564 + g * 0.3575761 + b * 0.1804375),
                    (r * 0.2126729 + g * 0.7151522 + b * 0.0721750),
                    (r * 0.0193339 + g * 0.1191920 + b * 0.9503041)
                    );
            case "D50":
                return new CIEXYZ(
                    (r * 0.7161046 + g * 0.1009296 + b * 0.1471858),
                    (r * 0.2581874 + g * 0.7249378 + b * 0.0168748),
                    (r * 0.0000000 + g * 0.0517813 + b * 0.7734287)
                    );
            default:
                return new CIEXYZ(
                    (r * 0.4124564 + g * 0.3575761 + b * 0.1804375),
                    (r * 0.2126729 + g * 0.7151522 + b * 0.0721750),
                    (r * 0.0193339 + g * 0.1191920 + b * 0.9503041)
                    );
        }
    }

    public static CIEXYZ ColortoXYZ(Color RGB)
    {
        // normalize red, green, blue values
        float rLinear = RGB.r;
        float gLinear = RGB.g;
        float bLinear = RGB.b;

        // convert to a sRGB form
        double r = (rLinear > 0.04045) ? Mathf.Pow((rLinear + 0.055f) / (
            1 + 0.055f), 2.2f) : (rLinear / 12.92f);
        double g = (gLinear > 0.04045) ? Mathf.Pow((gLinear + 0.055f) / (
            1 + 0.055f), 2.2f) : (gLinear / 12.92f);
        double b = (bLinear > 0.04045) ? Mathf.Pow((bLinear + 0.055f) / (
            1 + 0.055f), 2.2f) : (bLinear / 12.92f);

        return new CIEXYZ(
            (r * 0.4124564 + g * 0.3575761 + b * 0.1804375),
            (r * 0.2126729 + g * 0.7151522 + b * 0.0721750),
            (r * 0.0193339 + g * 0.1191920 + b * 0.9503041)
            );
    }

    /// <summary>
    /// Converts CIEXYZ to RGB structure.
    /// </summary>
    public static Color XYZtoColor(CIEXYZ color)
    {
        double[] Clinear = new double[3];
        Clinear[0] = color.X * 3.2410 - color.Y * 1.5374 - color.Z * 0.4986; // red
        Clinear[1] = -color.X * 0.9692 + color.Y * 1.8760 - color.Z * 0.0416; // green
        Clinear[2] = color.X * 0.0556 - color.Y * 0.2040 + color.Z * 1.0570; // blue

        for (int i = 0; i < 3; i++)
        {
            Clinear[i] = (Clinear[i] <= 0.0031308) ? 12.92 * Clinear[i] : (
                1 + 0.055) * Mathf.Pow((float)Clinear[i], (1.0f / 2.4f)) - 0.055;
        }

        return new Color((float)(Clinear[0]), (float)(Clinear[1]), (float)(Clinear[2]));
    }
}
