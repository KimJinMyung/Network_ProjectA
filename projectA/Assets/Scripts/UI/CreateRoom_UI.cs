using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CreateRoom_UI : MonoBehaviour
{
    [SerializeField]
    private List<Button> SmilerCountButton;

    [SerializeField]
    private List<Button> ExorcistCountButton;

    [SerializeField]
    private List<Button> MaxPlayerCountButton;

    private CreateGameRoomData roomData;

    // Start is called before the first frame update
    void Start()
    {
        roomData = new CreateGameRoomData() { SmilerCount = 1, ExorcistCount = 1, MaxPlayerCount = 10 };

        UpdateMaxPlayerCount(roomData.MaxPlayerCount);
        UpdateSmilerCount(roomData.SmilerCount);
        UpdateExorcistCount(roomData.ExorcistCount);
    }

    public void UpdateSmilerCount(int count)
    {
        //UpdateMaxPlayerCount(roomData.MaxPlayerCount);
        //int index = count - 1;
        roomData.SmilerCount = count;

        int index = 0;
        foreach (Button button in SmilerCountButton)
        {
            if (index == count - 1)
            {
                button.GetComponentInChildren<Text>().color = Color.red;
            }

            index++;
        }
        UpdateMaxPlayerCount(roomData.MaxPlayerCount);
    }

    public void UpdateExorcistCount(int count)
    {
        //UpdateMaxPlayerCount(roomData.MaxPlayerCount);
        roomData.ExorcistCount = count;

        int index = 0;
        foreach (Button button in ExorcistCountButton)
        {
            if (index == count - 1)
            {
                button.GetComponentInChildren<Text>().color = Color.red;
            }

            index++;
        }
        UpdateMaxPlayerCount(roomData.MaxPlayerCount);
    }
    
    public void UpdateMaxPlayerCount(int count)
    {        
        roomData.MaxPlayerCount = count;

        int index = 0;
        foreach (Button button in MaxPlayerCountButton)
        {
            if (index == count - 4)
            {
                button.GetComponentInChildren<Text>().color = Color.red;
            }
            else
            {
                button.GetComponentInChildren<Text>().color = Color.white;
            }
            index++;
        }

        if(roomData.MaxPlayerCount < 5)
        {
            MaxPeople_four();
        }
        else if(roomData.MaxPlayerCount < 6)
        {
            MaxPeople_five();
        }
        else
        {
            MaxPeople_others();
        }
    }

    private void MaxPeople_four()
    {
        ResetButton(SmilerCountButton);
        ResetButton(ExorcistCountButton);

        for (int i = 1; i <= 2; i++)
        {
            SmilerCountButton[SmilerCountButton.Count - i].interactable = false;
            SmilerCountButton[SmilerCountButton.Count - i].GetComponentInChildren<Text>().color = Color.gray;
            ExorcistCountButton[ExorcistCountButton.Count - i].interactable = false;
            ExorcistCountButton[ExorcistCountButton.Count - i].GetComponentInChildren<Text>().color = Color.gray;
        }

        if (roomData.SmilerCount > 1) roomData.SmilerCount = 1;
        if (roomData.ExorcistCount > 1) roomData.ExorcistCount = 1;

        SmilerCountButton[roomData.SmilerCount - 1].GetComponentInChildren<Text>().color = Color.red;
        ExorcistCountButton[roomData.ExorcistCount - 1].GetComponentInChildren<Text>().color = Color.red;
    }

    private void MaxPeople_five()
    {
        ResetButton(SmilerCountButton);
        ResetButton(ExorcistCountButton);

        SmilerCountButton[SmilerCountButton.Count - 1].interactable = false;
        SmilerCountButton[SmilerCountButton.Count - 1].GetComponentInChildren<Text>().color = Color.gray;
        ExorcistCountButton[ExorcistCountButton.Count - 1].interactable = false;
        ExorcistCountButton[ExorcistCountButton.Count - 1].GetComponentInChildren<Text>().color = Color.gray;

        if (roomData.SmilerCount > 2) roomData.SmilerCount = 2;
        if (roomData.ExorcistCount > 2) roomData.ExorcistCount = 2;

        SmilerCountButton[roomData.SmilerCount - 1].GetComponentInChildren<Text>().color = Color.red;
        ExorcistCountButton[roomData.ExorcistCount - 1].GetComponentInChildren<Text>().color = Color.red;
    }

    private void MaxPeople_others()
    {
        ResetButton(SmilerCountButton);
        ResetButton(ExorcistCountButton);

        SmilerCountButton[roomData.SmilerCount - 1].GetComponentInChildren<Text>().color = Color.red;
        ExorcistCountButton[roomData.ExorcistCount - 1].GetComponentInChildren<Text>().color = Color.red;
    }

    private void ResetButton(List<Button> buttonListName)
    {
        foreach(var item in buttonListName)
        {
            item.interactable = true;
            item.GetComponentInChildren<Text>().color = Color.white;
        }
    }

    public void CreateRoom()
    {
        var manager = NetworkManager.singleton as GameRoomManager;

        //방 설정 작업 처리
        manager._minPlayerCount = roomData.MaxPlayerCount;
        manager.maxConnections = roomData.MaxPlayerCount;
        manager._smilerCount = roomData.SmilerCount;
        manager._exorcistCount = roomData.ExorcistCount;

        manager.StartHost();
    }
}

public class CreateGameRoomData
{
    public int SmilerCount;
    public int ExorcistCount;
    public int MaxPlayerCount;
}
