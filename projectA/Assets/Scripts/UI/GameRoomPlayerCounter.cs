using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GameRoomPlayerCounter : NetworkBehaviour
{
    [SyncVar]
    private int minPlayer;
    [SyncVar]
    private int maxPlayer;

    [SerializeField]
    private Text _playerCountText;

    public void UpdatePlayerCount()
    {
        var players = FindObjectsOfType<CharacterMove>();
        bool isStartable = players.Length >= minPlayer;
        _playerCountText.color = isStartable ? Color.red : Color.white;
        _playerCountText.text = string.Format("{0} / {1}", players.Length, maxPlayer);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            var manager = GameRoomManager.singleton as GameRoomManager;
            minPlayer = manager._minPlayerCount;
            maxPlayer = manager.maxConnections;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
