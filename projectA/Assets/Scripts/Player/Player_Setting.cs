using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EControlType
{
    Mouse,
    Keyboard
}

public class Player_Setting : MonoBehaviour
{
    public static EControlType controlType;
    public static string nickName;
}
