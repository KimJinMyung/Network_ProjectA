using UnityEngine;

public class GameRuleItem : MonoBehaviour
{
    [SerializeField]
    private GameObject inactiveObject;

    // Start is called before the first frame update
    void Start()
    {
        // ȣ��Ʈ�� �ƴϸ� ���� �Ұ��ϵ��� ����        
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
