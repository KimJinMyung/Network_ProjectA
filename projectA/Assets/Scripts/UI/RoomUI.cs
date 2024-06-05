using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    [SerializeField]
    private Text _playerCountText;

    public void UpdatePlayerCount()
    {
        var manager = NetworkManager.singleton as GameRoomManager;
        var players = FindObjectsOfType<CharacterMove>();
        _playerCountText.text = string.Format("{0} / {1}", players.Length, manager.maxConnections);
    }
}
