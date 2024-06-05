using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
using Cinemachine;

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
                    if (player.authority)
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

    private List<Transform> _spawnpoint = new List<Transform>();

    private Rigidbody rigidbody;
    public Cinemachine.AxisState x_Axis;
    public Cinemachine.AxisState y_Axis;

    private Quaternion _initRotation;
    private Quaternion _mouseRotation;

    [SyncVar]
    [SerializeField]
    private float moveSpeed = 3.5f;

    public bool isMoveAble = true;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        foreach (Transform t in GameObject.FindWithTag("SpawnPoint").transform)
        {
            _spawnpoint.Add(t);
        }

        transform.position = _spawnpoint[Random.Range(0, _spawnpoint.Count-1)].position;

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

        rigidbody.velocity = ((transform.forward * Vertical) + (transform.right * Horizontal)) * moveSpeed;
        //agent.velocity = ((transform.forward * Vertical) + (transform.right * Horizontal)) * moveSpeed;
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
        transform.rotation = _playerCamera.transform.rotation;
        /*Quaternion.Lerp(_playerCamera.transform.rotation, _mouseRotation, 1f);*/
    }
}
