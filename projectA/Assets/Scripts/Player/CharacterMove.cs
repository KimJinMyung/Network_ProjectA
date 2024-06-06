using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
using Cinemachine;
using System.Threading;

public class CharacterMove : NetworkRoomPlayer/*NetworkBehaviour*/
{
    private static CharacterMove myRoomPlayer;

    public static CharacterMove MyRoomPlayer
    {
        get 
        {
            if(myRoomPlayer == null)
            {
                var Players = FindObjectsOfType<CharacterMove>();
                foreach(var player in Players)
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

    [SyncVar]
    public EPlayerColor playerColor;

    [SerializeField]
    private CinemachineVirtualCamera _playerCamera;

    [SerializeField]
    private List<GameObject> Meshs;

    [SerializeField]
    private List<Material> materials;

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
        foreach (Transform t in GameObject.FindWithTag("SpawnPoint").transform)
        {
            _spawnpoint.Add(t);
        }

        transform.position = _spawnpoint[Random.Range(0, _spawnpoint.Count - 1)].position + new Vector3(0, 1, 0);

        RoomUI.instance.playerCounter.UpdatePlayerCount();

        if (!this.isLocalPlayer) return;

        _playerCamera.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if (RoomUI.instance == null) return;
        RoomUI.instance.playerCounter.UpdatePlayerCount();
    }

    private void OnEnable()
    {        
        Debug.Log("스폰 완료");

        //로컬 플레이어만 실행
        if (!this.isLocalPlayer) return;
        
        InitRotation();
    }

    [Command]
    public void CmdSetPlayerColor(EPlayerColor color)
    {
        playerColor = color;
    }

    [ClientRpc]
    public void UpdatePlayerColor(EPlayerColor color)
    {
        Material playerMaterial = materials[0];

        switch (color)
        {
            case EPlayerColor.Red:
                playerMaterial = materials[(int)EPlayerColor.Red];
                break;
            case EPlayerColor.Green:
                playerMaterial = materials[(int)EPlayerColor.Green];
                break;
            case EPlayerColor.Blue:
                playerMaterial = materials[(int)EPlayerColor.Blue];
                break;
        }

        foreach(var item in Meshs)
        {
            item.GetComponent<MeshRenderer>().material = playerMaterial;
        }
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

        _characterController.Move(new Vector3(0, _velocity, 0) * Time.deltaTime);
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
