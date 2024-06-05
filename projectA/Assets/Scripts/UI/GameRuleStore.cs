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
        StringBuilder sb = new StringBuilder(isRecommendRule ? "ÃßÃµ ¼³Á¤\n" : "Ä¿½ºÅÒ ¼³Á¤\n");
        sb.Append("¸Ê : The Skeld \n");
        sb.Append('\n');
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
            SetRecommendGameRule();
        }  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
