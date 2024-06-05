using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSetting_UI : MonoBehaviour
{
    public void Open()
    {
        CharacterMove.MyRoomPlayer.isMoveAble = false;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        CharacterMove.MyRoomPlayer.isMoveAble = true;
        gameObject.SetActive(false);
    }
}
