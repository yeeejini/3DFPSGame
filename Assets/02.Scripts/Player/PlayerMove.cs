using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    private CameraManager cameraManager;
    // 목표 : 키보드 방향키 또는 WASD 를 누르면 캐릭터를 바라보는 방향 기준으로 이동시키고 싶다.
    // 속성 :
    // - 이동 속도
    public float MoveSpeed = 5f;    // 일반 속도
    public float RunSpeed = 10f;    // 뛰는 속도
    // 구현 순서
    // 1. 키 입력 받기
    // 2. '캐릭터가 바라보는 방향'을 기준으로 방향 구하기
    // 3. 이동하기

    public float Stamina = 100;       // 스태미나
    public const float MaxStamina = 100;
    public float StaminaConsumeSpeed = 33f; // 초당 스태미나 소모량
    public float StaminaChargeSpeed = 50f;  // 초당 스태미나 충전량

    [Header("스태미나 슬라이더 UI")]
    public Slider StaminaSliderUI;

    private CharacterController _characterController;






    // 목표 : 스페이스바를 누르면 캐릭터를 점프하고 싶다.
    // 필요 속성 :
    // - 점프 파워 값
    public float JumpPower = 10f;
    // 구현 순서 :
    // 1. 만약에 [Spacebar] 버튼을 누르면..
    // 2. 플레이어에게 y축에 있어 점프 파워를 적용한다.






    // 목표 : 캐릭터에게 중력을 적용하고 싶다.
    // 필요 속성 :
    // - 중력 값
    public float _gravity = -20; // 중력 변수
    // - 누적할 중력 변수 : y축 속도
    private float _yVelocity = 0f;
    // 구현 순서 :
    // 1. 중력 가속도가 누적된다.
    // 2. 플레이어에게 y축에 있어 중력을 적용한다.


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }


    private void Start()
    {
        Stamina = MaxStamina;
        cameraManager = GetComponent<CameraManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            CameraManager.Instance.SetCameraMode(CameraMode.FPS);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CameraManager.Instance.SetCameraMode(CameraMode.TPS);
        }

        // 1. 키 입력 받기
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");


        // 2. '캐릭터가 바라보는 방향'을 기준으로 방향 구하기
        Vector3 dir = new Vector3(x:h, y:0, z:v);             // 로컬 좌표계 (나만의 동서남북)
        dir.Normalize();
        dir = Camera.main.transform.TransformDirection(dir);  // 글로벌 좌표계 (세상의 동서남북)

        // 실습 과제 1. Shift 누르고 있으면 빨리 뛰기 (이동 속도 10)
        float speed = MoveSpeed; // 5
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // 실습 과제 2. 스태미너 구현
            // -Shfit 누른 동안에는 스태미나가 서서히 소모된다. (3초)
            Stamina -= StaminaConsumeSpeed * Time.deltaTime;
            if (Stamina > 0) 
            {
                speed = RunSpeed;  // 10
            }
        }
        else 
        {
            // - 아니면 스태미나가 소모 되는 속도보다 빠른 속도로 충전된다. (2초)
            Stamina += StaminaChargeSpeed * Time.deltaTime;
        }

        Stamina = Mathf.Clamp(value:Stamina, min:0, max:100);

        StaminaSliderUI.value = Stamina / MaxStamina; // 0 ~ 1 사이 반환




        // 점프 구현
        // 1. 만약에 [Spacebar] 버튼을 누르는 순간 && 땅이면..
        if(Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded) 
        {
            // 2. 플레이어에게 y축에 있어 점프 파워를 적용한다.
            _yVelocity = JumpPower;
        }




        // 3-1. 중력 적용
        // 1. 중력 가속도가 누적된다.
        _yVelocity += _gravity * Time.deltaTime;
        // 2. 플레이어에게 y축에 있어 중력을 적용한다.
        dir.y = _yVelocity;




        // 3-2. 이동하기                                  
        // transform.position += speed * dir * Time.deltaTime; -> 캐릭터 컨트롤러를 사용
        _characterController.Move(dir * speed * Time.deltaTime);
        
    }
}
