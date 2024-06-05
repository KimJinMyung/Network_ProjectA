using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting_UI : MonoBehaviour
{
    [SerializeField]
    private Button Btn_Mouse;
    [SerializeField]
    private Button Btn_Keyboard;

    private void OnEnable()
    {
        ButtonColorChange();
    }

    public void SetControlMode(int controlType)
    {
        Player_Setting.controlType = (EControlType)controlType;

        ButtonColorChange();
    }

    private void ButtonColorChange()
    {
        switch (Player_Setting.controlType)
        {
            case EControlType.Mouse:
                Btn_Mouse.image.color = Color.red;
                Btn_Keyboard.image.color = Color.white;
                break;
            case EControlType.Keyboard:
                Btn_Mouse.image.color = Color.white;
                Btn_Keyboard.image.color = Color.red;
                break;
        }
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
