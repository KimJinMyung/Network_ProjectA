using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoomSetting_UI : Setting_UI
{
    public void Open()
    {
        CharacterMove.MyRoomPlayer.isMoveAble = false;
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        CharacterMove.MyRoomPlayer.isMoveAble = true;
    }

    public void ExitGameRoom()
    {
        var manager = GameRoomManager.singleton;
        if(manager.mode == Mirror.NetworkManagerMode.Host)
        {
            manager.StopHost();
        }else if(manager.mode == Mirror.NetworkManagerMode.ClientOnly)
        {
            manager.StopClient();
        }
    }
}
