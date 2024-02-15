using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    // 카메라가 따라갈 대상
    public Transform Target;

    // 대상과의 Y축 거리
    public float YDistance = 20f;

    private Vector3 _initialEulerAngles;

    private void Start()
    {
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

        Vector3 targetEulerAngles = Target.eulerAngles;
        targetEulerAngles.x = _initialEulerAngles.x;
        targetEulerAngles.z = _initialEulerAngles.z;
        transform.eulerAngles = targetEulerAngles;
    }
}
