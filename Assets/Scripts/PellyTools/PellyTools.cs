using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

namespace PellyTools
{
    public class MathP : MonoBehaviour
    {
        public static float Round2NearestQuarter(float a)
        {
            return a = a - (a % 0.25f);
        }

        public static float Difference(float num1, float num2)
        {
            float cout;
            cout = Mathf.Max(num2, num1) - Mathf.Min(num1, num2);
            return cout;
        }
    }

    public static class Arrays
    {
        public static void Push<T>(ref T[] table, object value)
        {
            Array.Resize(ref table, table.Length + 1);
            table.SetValue(value, table.Length - 1);
        }
    }

    public class Colors : MonoBehaviour
    {
        public static string Color2Hex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }

        public static Color Hex2RGB(string hex)
        {
            hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
            hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
            byte a = 255;//assume fully visible unless specified in hex
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            //Only use alpha if the string has enough characters
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return new Color32(r, g, b, a);
        }
    }

    public class Transformer : MonoBehaviour
    {
        public static Vector3 Resize(Vector3 amount, Vector3 direction)
        {
            return new Vector3(direction.x * amount.x - 11f, direction.y * amount.y) / 2;
            // transform.localScale = new Vector3(direction.x * amount.x, direction.y * amount.y);
        }
    }

    public class Windows : MonoBehaviour
    {
        public enum WindowsCursor
        {
            StandardArrowAndSmallHourglass = 32650,
            StandardArrow = 32512,
            Crosshair = 32515,
            Hand = 32649,
            ArrowAndQuestionMark = 32651,
            IBeam = 32513,
            //Icon = 32641, // Obsolete for applications marked version 4.0 or later. 
            SlashedCircle = 32648,
            //Size = 32640,  // Obsolete for applications marked version 4.0 or later. Use FourPointedArrowPointingNorthSouthEastAndWest
            FourPointedArrowPointingNorthSouthEastAndWest = 32646,
            DoublePointedArrowPointingNortheastAndSouthwest = 32643,
            DoublePointedArrowPointingNorthAndSouth = 32645,
            DoublePointedArrowPointingNorthwestAndSoutheast = 32642,
            DoublePointedArrowPointingWestAndEast = 32644,
            VerticalArrow = 32516,
            Hourglass = 32514
        }

        public static void ChangeCursor(WindowsCursor cursor)
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                SetCursor(LoadCursor(IntPtr.Zero, (int)cursor));
            }
            else
                Debug.LogWarning("Not on Windows");
        }

        [DllImport("user32.dll", EntryPoint = "SetCursor")]
        public static extern IntPtr SetCursor(IntPtr hCursor);

        [DllImport("user32.dll", EntryPoint = "LoadCursor")]
        public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);
    }
}
