using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

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

    public override void OnServerChangeScene(string newSceneName)
    {
        base.OnServerChangeScene(newSceneName);
    }

    public override void ServerChangeScene(string newSceneName)
    {

        if (newSceneName == GameplayScene)
        {
            List<NetworkConnectionToClient> objectsToDestroy = new List<NetworkConnectionToClient>();

            foreach (var roomplayers in roomSlots)
            {
                var conn = roomplayers.connectionToClient;
                if (conn != null && conn.identity.GetComponent<NetworkRoomPlayer>() != null)
                {
                    objectsToDestroy.Add(conn);
                }
            }

            foreach (var obj in objectsToDestroy)
            {
                Debug.Log(obj.identity.gameObject.name);
                NetworkServer.Destroy(obj.identity.gameObject);
            }

            //foreach (var conn in objectsToDestroy)
            //{
            //    GameObject mainPlayer = Instantiate(playerPrefab);
            //    NetworkServer.Spawn(mainPlayer);
            //    Debug.Log(mainPlayer.name + "�õ���");
            //    //NetworkServer.ReplacePlayerForConnection(conn, mainPlayer, true);
            //}
            StartCoroutine(WaitForSceneLoadAndReplacePlayers(newSceneName, objectsToDestroy));
        }
        else
        base.ServerChangeScene(newSceneName);
    }

    private IEnumerator WaitForSceneLoadAndReplacePlayers(string newSceneName, List<NetworkConnectionToClient> connectionsToReplace)
    {
        // �� �ε尡 �Ϸ�� ������ ���
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == newSceneName);

        // MainPlayerPrefab ���� �� ����
        foreach (var conn in connectionsToReplace)
        {
            GameObject mainPlayer = Instantiate(playerPrefab);
            NetworkServer.Spawn(mainPlayer);
            if (conn.identity != null)
            {
                NetworkServer.ReplacePlayerForConnection(conn, mainPlayer, true);
            }
            //NetworkServer.ReplacePlayerForConnection(conn, mainPlayer, true);
        }
    }
    //public void OnServerAddPlayer(NetworkConnectionToClient conn, string newSceneName)
    //{
    //    if(newSceneName == GameplayScene)
    //    {
    //        GameObject player = Instantiate(playerPrefab);
    //        NetworkServer.AddPlayerForConnection(conn, player);
    //    }
    //    else
    //    {
    //        base.OnServerAddPlayer(conn);
    //    }        
    //}
}
