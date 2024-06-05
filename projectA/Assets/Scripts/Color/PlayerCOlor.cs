using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerColor
{
    Red, 
    Blue
}

public class PlayerCOlor
{
    private static List<Color> Colors = new List<Color>()
    {
        new Color(255f,0f,0f),
        new Color(0,0,255f)
    };

    public static Color Red {  get { return Colors[(int)EPlayerColor.Red]; } }
    public static Color Blue { get { return Colors[(int)EPlayerColor.Blue]; } }
}
