using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    public static RoomUI instance;

    [SerializeField]
    private GameRoomPlayerCounter roomPlayerCounter;

    public GameRoomPlayerCounter playerCounter {  get { return roomPlayerCounter; } }

    private void Awake()
    {
        instance = this;
    }
}
