using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameRoomManager : NetworkRoomManager
{
    public int _minPlayerCount;
    public int _smilerCount;
    public int _exorcistCount;

    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
    {
        //base.OnRoomServerConnect(conn);

        Debug.LogWarning("���� ���� ����");

        //var player = Instantiate(spawnPrefabs[0]);
        //NetworkServer.Spawn(player, conn);
    }
}
