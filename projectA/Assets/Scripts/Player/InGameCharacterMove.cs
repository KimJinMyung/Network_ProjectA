using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public class InGameCharacterMove : NetworkBehaviour
{
    //[SerializeField]
    //private CinemachineVirtualCamera _playerCamera;

    //[SyncVar]
    //public EPlayerColor playerColor;

    //[SerializeField]
    //private List<GameObject> Meshs;

    //[SerializeField]
    //private List<Material> materials;

    //[Header("그라운드 확인 overlap")]
    //[SerializeField]
    //private Transform overlapPos;

    //[SerializeField]
    //private LayerMask gravityLayermask;

    //[SyncVar(hook = nameof(SetNickNameText_Hook))]
    //private string nickName;
    //[SerializeField]
    //private Text NickNameText;

    //public void SetNickNameText_Hook(string _, string value)
    //{
    //    NickNameText.text = value;
    //}

    //[Command]
    //public void SetNickName(string value)
    //{
    //    nickName = value;
    //}


    //private List<Transform> _spawnpoint = new List<Transform>();

    //public Cinemachine.AxisState x_Axis;
    //public Cinemachine.AxisState y_Axis;

    //private Quaternion _initRotation;
    //private Quaternion _mouseRotation;

    //private CharacterController _characterController;

    //private Collider[] colliders;
    ////중력 값
    //private float gravity = -9.81f;
    ////현재 중력 가속도
    //private float _velocity;

    //[SyncVar]
    //[SerializeField]
    //private float moveSpeed = 3.5f;

    //public bool isMoveAble = true;
    //private bool isGround;

    //private void Awake()
    //{
    //    _characterController = GetComponent<CharacterController>();
    //}

    //private void OnEnable()
    //{
    //    foreach (Transform t in GameObject.FindWithTag("SpawnPoint").transform)
    //    {
    //        _spawnpoint.Add(t);
    //    }

    //    Debug.Log("스폰 완료");

    //    //NetworkServer.ReplacePlayerForConnection(this.connectionToClient, this.gameObject);

    //    //로컬 플레이어만 실행
    //    if (!this.isLocalPlayer) return;

    //    InitRotation();
    //}

    //private void Start()
    //{
    //    CmdSetInitPosition(_spawnpoint[Random.Range(0, _spawnpoint.Count)].position + new Vector3(0, 1, 0));

    //    if (!this.isLocalPlayer) return;

    //    _playerCamera.gameObject.SetActive(true);
    //    _playerCamera.transform.position = transform.position + new Vector3(0, 0.36f, 0.055f);
    //    SetNickName(Player_Setting.nickName);
    //}

    //[Command]
    //private void CmdSetInitPosition(Vector3 position)
    //{
    //    RpcSetInitPosition(position);
    //}

    //[ClientRpc]
    //private void RpcSetInitPosition(Vector3 pos)
    //{
    //    transform.position = pos;
    //}

    //private void Update()
    //{
    //    Gravity();
    //    Debug.Log(isLocalPlayer);
    //}

    //private void FixedUpdate()
    //{
    //    CheckLocalPlayer();
    //}

    //private void CheckLocalPlayer()
    //{
    //    if (!this.isLocalPlayer) return;

    //    CameraRotation();
    //    Move();
    //}

    //private void Move()
    //{
    //    if (!isMoveAble) return;
    //    float Vertical = Input.GetAxis("Vertical");
    //    float Horizontal = Input.GetAxis("Horizontal");

    //    //_characterController.velocity = transform.forward * Vertical;

    //    Vector3 move = (transform.forward * Vertical) + (transform.right * Horizontal);
    //    if (move != Vector3.zero) _characterController.Move(move * moveSpeed * Time.fixedDeltaTime);
    //}

    //private void Gravity()
    //{
    //    colliders = Physics.OverlapBox(overlapPos.position, new Vector3(0.3f, 0.1f, 0.3f), Quaternion.identity, gravityLayermask);

    //    if (colliders.Length > 0 && _velocity <= 0.0f)
    //    {
    //        _velocity = -1f;
    //        isGround = true;
    //    }
    //    else
    //    {
    //        _velocity += gravity * Time.deltaTime;
    //        isGround = false;
    //    }

    //    _characterController.Move(-Vector3.down * _velocity * Time.deltaTime);
    //    CmdSetPosition(transform.position);
    //}

    //[Command]
    //private void CmdSetPosition(Vector3 position)
    //{
    //    RpcUpdatePosition(position); // 모든 클라이언트에게 위치 업데이트
    //}

    //[ClientRpc]
    //private void RpcUpdatePosition(Vector3 position)
    //{
    //    if (!isLocalPlayer)
    //    {
    //        transform.position = position; // 모든 클라이언트에서 위치 동기화
    //    }
    //}

    //private void InitRotation()
    //{
    //    x_Axis.Value = 0;
    //    y_Axis.Value = 0;

    //    _initRotation = _playerCamera.transform.rotation;

    //    Vector3 initEulerAngle = _initRotation.eulerAngles;
    //    x_Axis.Value = initEulerAngle.y;
    //    y_Axis.Value = initEulerAngle.x;

    //    _mouseRotation = _initRotation;
    //}

    //private void CameraRotation()
    //{
    //    if (!isMoveAble) return;

    //    x_Axis.Update(Time.fixedDeltaTime);
    //    y_Axis.Update(Time.fixedDeltaTime);

    //    _mouseRotation = Quaternion.Euler(y_Axis.Value, x_Axis.Value, 0f);

    //    _playerCamera.transform.rotation = _mouseRotation;

    //    Vector3 Angle = transform.rotation.eulerAngles;
    //    Angle.y = _mouseRotation.eulerAngles.y;

    //    Quaternion newRotation = Quaternion.Euler(Angle);

    //    transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, 0.8f);
    //    /*Quaternion.Lerp(_playerCamera.transform.rotation, _mouseRotation, 1f);*/
    //}

    //[Command]
    //public void CmdSetPlayerColor(EPlayerColor color)
    //{
    //    playerColor = color;
    //}

    //[ClientRpc]
    //public void UpdatePlayerColor(EPlayerColor color)
    //{
    //    Material playerMaterial = materials[0];

    //    switch (color)
    //    {
    //        case EPlayerColor.Red:
    //            playerMaterial = materials[(int)EPlayerColor.Red];
    //            break;
    //        case EPlayerColor.Green:
    //            playerMaterial = materials[(int)EPlayerColor.Green];
    //            break;
    //        case EPlayerColor.Blue:
    //            playerMaterial = materials[(int)EPlayerColor.Blue];
    //            break;
    //    }

    //    foreach (var item in Meshs)
    //    {
    //        item.GetComponent<MeshRenderer>().material = playerMaterial;
    //    }
    //}
}
