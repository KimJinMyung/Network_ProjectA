using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Text;

public struct GmaeRuleData
{
    public float MissionCount;
    public float GunPartsCount;
}

public class GameRuleStore : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetIsRecommendRule_Hook))]
    private bool isRecommendRule;
    [SerializeField]
    private Toggle _isRecommendRuleToggle;

    public void SetIsRecommendRule_Hook(bool _, bool value)
    {
        UpdateGameRuleOverview();
    }

    public void OnRecommendToggle(bool value)
    {
        isRecommendRule = value;
        if(isRecommendRule)
        {
            SetRecommendGameRule();
        }
    }

    [SyncVar(hook = nameof(SetSmilerCountText_Hook))]
    private int SmilerCount;
    [SerializeField]
    private Text _SmilerCount;

    public void SetSmilerCountText_Hook(int _, int value)
    {
        _SmilerCount.text = value.ToString();
        UpdateGameRuleOverview();
    }
    public void OnChangeSmilerCount(bool isPlus)
    {
        int MaxValue = GameRoomManager.singleton.maxConnections < 5 ? 1 :
    GameRoomManager.singleton.maxConnections < 6 ? 2 : 3;

        SmilerCount = Mathf.Clamp(SmilerCount + (isPlus ? 1 : -1), 1, MaxValue);
        isRecommendRule = false;
        _isRecommendRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(SetExorcistCountText_Hook))]
    private int ExorcistCount;
    [SerializeField]
    private Text _ExorcistCount;

    public void SetExorcistCountText_Hook(int _, int value)
    {
        _ExorcistCount.text = value.ToString();
        UpdateGameRuleOverview();
    }
    public void OnChangeExorcistCount(bool isPlus)
    {
        int MaxValue = GameRoomManager.singleton.maxConnections < 5 ? 1 : 
            GameRoomManager.singleton.maxConnections < 6 ? 2 : 3;
        
        ExorcistCount = Mathf.Clamp(ExorcistCount + (isPlus ? 1 : -1), 1, MaxValue);
        isRecommendRule = false;
        _isRecommendRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(SetMissionCountText_Hook))]
    private int MissionCount;
    [SerializeField]
    private Text _MissionCount;

    public void SetMissionCountText_Hook(int _, int value)
    {
        _MissionCount.text = value.ToString();
        UpdateGameRuleOverview();
    }
    public void OnChangeMissionCount(bool isPlus)
    {
        MissionCount = Mathf.Clamp( MissionCount +(isPlus ? 1 : -1), 3, 10);
        isRecommendRule = false;
        _isRecommendRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(SetGunPartsCountText_Hook))]
    private int GunPartsCount;
    [SerializeField]
    private Text _GunPartsCount;

    public void SetGunPartsCountText_Hook(int _, int value)
    {
        _GunPartsCount.text = value.ToString();
        UpdateGameRuleOverview();
    }
    public void OnChangeGunPartsCount(bool isPlus)
    {
        GunPartsCount = Mathf.Clamp(GunPartsCount + (isPlus ? 1 : -1), 5, 20);
        isRecommendRule = false;
        _isRecommendRuleToggle.isOn = false;
    }

    [SerializeField]
    private Text gameRuleOverview;

    public void UpdateGameRuleOverview()
    {
        var manager = NetworkManager.singleton as GameRoomManager;
        StringBuilder sb = new StringBuilder(isRecommendRule ? "추천 설정\n" : "커스텀 설정\n");
        sb.Append("맵 : The Skeld \n");
        sb.Append('\n');
        sb.Append($"Smiler Count : {SmilerCount}\n");
        sb.Append($"Exorcist Count : {ExorcistCount}\n");
        sb.Append($"Gun Parts : {GunPartsCount}\n");
        sb.Append($"Misson Count : {MissionCount}\n");

        gameRuleOverview.text = sb.ToString();
    }
    private void SetRecommendGameRule()
    {
        GunPartsCount = 10;
        MissionCount = 5;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            var manager = GameRoomManager.singleton as GameRoomManager;
            SmilerCount = manager._smilerCount;
            ExorcistCount = manager._exorcistCount;

            SetRecommendGameRule();
        }  
    }

    public void Test()
    {
        Debug.Log("눌림");
    }
}
