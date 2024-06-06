using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    public static RoomUI instance;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private GameRoomPlayerCounter roomPlayerCounter;

    public GameRoomPlayerCounter playerCounter {  get { return roomPlayerCounter; } }

    private void Awake()
    {
        instance = this;
    }

    public void AciveStartButton()
    {
        startButton.gameObject.SetActive(true);
    }

    public void SetInteractableStartButton(bool interactableStartButton)
    {
        startButton.interactable = interactableStartButton;
    }

    public void OnClickStartButton()
    {
        var players = FindObjectsOfType<CharacterMove>();
        foreach (var player in players)
        {
            player.CmdChangeReadyState(true);
        }

        var manager = NetworkManager.singleton as GameRoomManager;
        //manager.OnServerChangeScene(manager.GameplayScene);
        manager.ServerChangeScene(manager.GameplayScene);
    }
}
