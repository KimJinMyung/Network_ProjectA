using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EControlType
{
    Mouse,
    Keyboard
}

public enum PlayerType
{
    Smiler,
    Exorcist,
    Survivor
}

public class Player_Setting
{
    public static EControlType controlType;
    public static string nickName;
    public static PlayerType playerType;
}
