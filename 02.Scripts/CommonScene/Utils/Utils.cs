using System;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace Utils
{
    public static class Constants
    {
        public static readonly int TILE_VERTEX = 4;
        public static readonly int[] DIRECTION_X = new int[4] { 1, -1, 0, 0 };
        public static readonly int[] DIRECTION_Y = new int[4] { 0, 0, 1, -1 };


        public static readonly int[] DIRECTION_XY = new int[4] { 1, 1, -1, -1 };
        public static readonly int[] DIRECTION_YX = new int[4] { -1, 1, -1, 1 };

        public static readonly string SCREEN_INDEX = "ScreenIndex";

        public static readonly Resolution[] SCREEN_RESOLUTION = new Resolution[5]
        {
            new Resolution(-1, -1), new Resolution(1280, 720), new Resolution(1600, 900), new Resolution(1920, 1080),
            new Resolution(1920, 1200)
        };
    }

    public static class Utilities
    {
        public static Vector3 ClampVector3(Vector3 vec, Vector3 minVec, Vector3 maxVec)
        {
            float x = Math.Clamp(vec.x, minVec.x, maxVec.x);
            float y = Math.Clamp(vec.y, minVec.y, maxVec.y);
            float z = Math.Clamp(vec.z, minVec.z, maxVec.z);
            return new Vector3(x, y, z);
        }
        public static string GetGearSignString(float amount)
        {
            if (amount >= 0)
            {
                return "+";
            }
            else
            {
                return "";
            }

        }


        public static Color HexToColor(string hex)
        {
            // Remove the hash at the start if it's there
            if (hex.StartsWith("#"))
                hex = hex.Substring(1);

            // Parse the color
            if (hex.Length == 6) // #RRGGBB
            {
                if (ColorUtility.TryParseHtmlString($"#{hex}", out var color))
                    return color;
            }
            else if (hex.Length == 8) // #RRGGBBAA
            {
                if (ColorUtility.TryParseHtmlString($"#{hex}", out var color))
                    return color;
            }

            // Return white if parsing fails
            Debug.LogWarning($"Invalid HEX color string: {hex}");
            return Color.white;
        }


    }


}

