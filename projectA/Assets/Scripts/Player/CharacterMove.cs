using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;
using UnityEngine.UI;

public class CharacterMove : NetworkRoomPlayer
{
    private static CharacterMove myRoomPlayer;

    public static CharacterMove MyRoomPlayer
    {
        get
        {
            if (myRoomPlayer == null)
            {
                var Players = FindObjectsOfType<CharacterMove>();
                foreach (var player in Players)
                {
                    if (player.isLocalPlayer)
                    {
                        myRoomPlayer = player;
                    }
                }
            }

            return myRoomPlayer;
        }
    }

    [SerializeField]
    private CinemachineVirtualCamera _playerCamera;

    [SyncVar(hook = nameof(SetNickNameText_Hook))]
    private string nickName;

    public string getNickName { get { return nickName; } }
    [SerializeField]
    private Text NickNameText;

    public void SetNickNameText_Hook(string _, string value)
    {
        NickNameText.text = value;
    }

    [Command]
    public void SetNickName(string value)
    {
        nickName = value;
    }

    [Header("그라운드 확인 overlap")]
    [SerializeField]
    private Transform overlapPos;

    [SerializeField]
    private LayerMask gravityLayermask;

    private List<Transform> _spawnpoint = new List<Transform>();

    public Cinemachine.AxisState x_Axis;
    public Cinemachine.AxisState y_Axis;

    private Quaternion _initRotation;
    private Quaternion _mouseRotation;

    private CharacterController _characterController;

    private Collider[] colliders;
    //중력 값
    private float gravity = -9.81f;
    //현재 중력 가속도
    private float _velocity;

    [SyncVar]
    [SerializeField]
    private float moveSpeed = 3.5f;

    public bool isMoveAble = true;
    private bool isGround;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public override void OnStartClient()
    {
        //foreach (Transform t in GameObject.FindWithTag("SpawnPoint").transform)
        //{
        //    _spawnpoint.Add(t);
        //}

        CmdSetInitPosition(_spawnpoint[Random.Range(0, _spawnpoint.Count)].position + new Vector3(0, 1, 0));
        //transform.position = (_spawnpoint[Random.Range(0, _spawnpoint.Count)].position + new Vector3(0, 1, 0));

        RoomUI.instance.playerCounter.UpdatePlayerCount();

        if (isServer)
        {
            RoomUI.instance.AciveStartButton();
        }

        if (!this.isLocalPlayer) return;

        _playerCamera.gameObject.SetActive(true);
        SetNickName(Player_Setting.nickName);
    }

    [Command]
    private void CmdSetInitPosition(Vector3 position)
    {
        RpcSetInitPosition(position);
    }

    [ClientRpc]
    private void RpcSetInitPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    private void OnDestroy()
    {
        if (RoomUI.instance == null) return;
        RoomUI.instance.playerCounter.UpdatePlayerCount();
    }

    private void OnEnable()
    {
        foreach (Transform t in GameObject.FindWithTag("SpawnPoint").transform)
        {
            _spawnpoint.Add(t);
        }        

        Debug.Log("스폰 완료");

        //로컬 플레이어만 실행
        if (!this.isLocalPlayer) return;
        
        InitRotation();
    }

    private void Update()
    {
        Gravity();
    }

    private void FixedUpdate()
    {
        CheckLocalPlayer();
        //로컬 플레이어만 실행
        //CameraRotation();
        //Move();
    }

    private void CheckLocalPlayer()
    {
        //Debug.Log("로컬 플레이어 : "+ isLocalPlayer);
        //Debug.Log("서버 : " + isServer);
        //Debug.Log("클라이언트 : "+isClient);
        if (!this.isLocalPlayer) return;

        CameraRotation();
        Move();
    }

    private void Move()
    {
        if (!isMoveAble) return;
        float Vertical = Input.GetAxis("Vertical");
        float Horizontal = Input.GetAxis("Horizontal");

        //_characterController.velocity = transform.forward * Vertical;

        Vector3 move = (transform.forward * Vertical) + (transform.right * Horizontal);
        if (move != Vector3.zero) _characterController.Move(move * moveSpeed * Time.fixedDeltaTime);
    }

    private void Gravity()
    {
        colliders = Physics.OverlapBox(overlapPos.position, new Vector3(0.3f, 0.1f, 0.3f), Quaternion.identity, gravityLayermask);

        if (colliders.Length > 0 && _velocity <= 0.0f)
        {
            _velocity = -1f;
            isGround = true;
        }
        else
        {
            _velocity += gravity * Time.deltaTime;
            isGround = false;
        }

        _characterController.Move(-Vector3.down * _velocity * Time.deltaTime);
        CmdSetPosition(transform.position);
    }

    [Command]
    private void CmdSetPosition(Vector3 position)
    {
        RpcUpdatePosition(position); // 모든 클라이언트에게 위치 업데이트
    }

    [ClientRpc]
    private void RpcUpdatePosition(Vector3 position)
    {
        if (!isLocalPlayer)
        {
            transform.position = position; // 모든 클라이언트에서 위치 동기화
        }
    }

    private void InitRotation()
    {
        x_Axis.Value = 0;
        y_Axis.Value = 0;

        _initRotation = _playerCamera.transform.rotation;

        Vector3 initEulerAngle = _initRotation.eulerAngles;
        x_Axis.Value = initEulerAngle.y;
        y_Axis.Value = initEulerAngle.x;

        _mouseRotation = _initRotation;
    }

    private void CameraRotation()
    {
        if (!isMoveAble) return;

        x_Axis.Update(Time.fixedDeltaTime);
        y_Axis.Update(Time.fixedDeltaTime);

        _mouseRotation = Quaternion.Euler(y_Axis.Value, x_Axis.Value, 0f);

        _playerCamera.transform.rotation = _mouseRotation;

        Vector3 Angle = transform.rotation.eulerAngles;
        Angle.y = _mouseRotation.eulerAngles.y;

        Quaternion newRotation = Quaternion.Euler(Angle);

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, 0.8f);
        /*Quaternion.Lerp(_playerCamera.transform.rotation, _mouseRotation, 1f);*/
    }
}
