using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    // 3인칭 슈팅(Third Person Shoot
    // 게임상의 캐릭터가 보는 시점이 아닌, 캐릭터를 보는 시점 즉, 3인칭 관찰자 시점의 카메라

    // ** 카메라 회전 **
    // 목표 : 마우스를 조작하면 카메라를 캐릭터 중심에 따라 그 방향으로 회전 시키고 싶다.
    // 필요 속성 :
    // - 회전 속도
    public float RotationSpeed = 200;

    // - 타켓(캐릭터)
    public Transform Target;
    public Vector3 Offset = new Vector3(0, 3f, -3f);

    // - 누적할 x각도와 y각도
    private float _mx = 0;
    private float _my = 0;


    

    void Start()
    {
        
    }

    
    void LateUpdate()
    {
        if (CameraManager.Instance.Mode == CameraMode.TPS) 
        {
            // 구현 순서 :
            // 1. 카메라를 타켓으로 이동시킨다. (따라다니게 한다.)
            transform.position = Target.position + new Vector3(0, 3f, -3f); // 오프셋 더해주기 (여백)

            // 2. 플레이어를 쳐다보게 한다.
            transform.LookAt(Target);
        }

        // 3. 마우스 입력을 받는다.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");


        // 4. 마우스 입력에 따라 회전 방향을 구한다.
        _mx += mouseX * RotationSpeed * Time.deltaTime;
        _my += mouseY * RotationSpeed * Time.deltaTime;

        if (CameraManager.Instance.Mode == CameraMode.TPS) 
        {
            // 5. 타겟 중심으로 회전 방향에 맞게 회전한다.
            transform.RotateAround(Target.position, Vector3.up, _mx);
            transform.RotateAround(Target.position, transform.right, -_my);
        }
        

    }
    
}
