using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
using Cinemachine;

public class CharacterMove : NetworkBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _playerCamera;

    private List<Transform> _spawnpoint = new List<Transform>();

    private NavMeshAgent agent;

    public Cinemachine.AxisState x_Axis;
    public Cinemachine.AxisState y_Axis;

    private Quaternion _initRotation;
    private Quaternion _mouseRotation;

    [SyncVar]
    [SerializeField]
    private float moveSpeed = 3.5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        foreach (Transform t in GameObject.FindWithTag("SpawnPoint").transform)
        {
            _spawnpoint.Add(t);
        }

        transform.position = _spawnpoint[Random.Range(0, _spawnpoint.Count-1)].position;

        //로컬 플레이어만 실행
        if (!authority) return;
        InitRotation();
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

        float Vertical = Input.GetAxis("Vertical");
        float Horizontal = Input.GetAxis("Horizontal");

        agent.velocity = ((transform.forward * Vertical) + (transform.right * Horizontal)) * moveSpeed;
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
        x_Axis.Update(Time.fixedDeltaTime);
        y_Axis.Update(Time.fixedDeltaTime);

        _mouseRotation = Quaternion.Euler(y_Axis.Value, x_Axis.Value, 0f);

        _playerCamera.transform.rotation = _mouseRotation;
        transform.LookAt(_playerCamera.transform);
        /*Quaternion.Lerp(_playerCamera.transform.rotation, _mouseRotation, 1f);*/
    }
}
