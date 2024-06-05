using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class JoinGame_UI : MonoBehaviour
{
    [SerializeField] private InputField _nickNameInput;
    [SerializeField] private GameObject CreateRoomUI;

    private Animator _nickNameAnimator;

    private void Awake()
    {
        _nickNameAnimator = _nickNameInput.GetComponent<Animator>();
    }

    public void OnClick_CreateRoomButton()
    {
        if (!string.IsNullOrWhiteSpace(_nickNameInput.text))
        {
            Player_Setting.nickName = _nickNameInput.text;
            CreateRoomUI.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            _nickNameAnimator.SetTrigger("On");
        }
    }

    public void OnClick_StartClient()
    {
        if (!string.IsNullOrWhiteSpace(_nickNameInput.text))
        {
            Player_Setting.nickName = _nickNameInput.text;
            GameRoomManager.singleton.StartClient();
        }
        else
        {
            _nickNameAnimator.SetTrigger("On");
        }

    }
}
