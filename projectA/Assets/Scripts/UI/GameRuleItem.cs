using UnityEngine;

public class GameRuleItem : MonoBehaviour
{
    [SerializeField]
    private GameObject inactiveObject;

    // Start is called before the first frame update
    void Start()
    {
        // 호스트가 아니면 조작 불가하도록 설정        
        if (!CharacterMove.MyRoomPlayer.isServer)
        {
            inactiveObject.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
