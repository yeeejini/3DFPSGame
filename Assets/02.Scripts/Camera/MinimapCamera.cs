using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    // 카메라가 따라갈 대상
    public Transform Target;

    // 대상과의 Y축 거리
    public float YDistance = 20f;

    // 카메라 초기 회전값을 저장할 변수
    private Vector3 _initialEulerAngles;

    private void Start()
    {
        // 카메라의 초기 회전값을 저장
        _initialEulerAngles = transform.eulerAngles;
    }

    private void LateUpdate()
    {
        // 대상의 현재 위치를 가져옴
        Vector3 targetPosition = Target.position;

        // Y축 위치를 원하는 거리로 조정
        targetPosition.y = YDistance;

        // 카메라의 위치를 대상의 위치로 설정하여 따라가게 함
        transform.position = targetPosition;

        // 대상의 현재 회전값을 가져옴
        Vector3 targetEulerAngles = Target.eulerAngles;
        // 카메라의 x와 z축 회전값을 초기 값으로 고정하여, 대상의 y축 회전만 따라가도록 함
        targetEulerAngles.x = _initialEulerAngles.x;
        targetEulerAngles.z = _initialEulerAngles.z;
        // 카메라의 회전을 대상의 회전값으로 설정
        transform.eulerAngles = targetEulerAngles;
    }
}
