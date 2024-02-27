using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum CameraMode 
{
    FPS,
    TPS,
    Top,
    Back,
}
public class CameraManager : MonoBehaviour
{
    // 역할 : 카메라를 관리하는 관리자
    // FPS 카메라 스크립트
    private FPSCamera fpsCamera;
    // TPS 카메라 스크립트
    private TPSCamera tpsCamera;

    // 현재의 카메라 모드
    public CameraMode Mode = CameraMode.FPS;

    // 카메라 매니저의 인스턴스를 외부에서 접근하기 위한 정적 변수
    public static CameraManager Instance { get; private set; }

    private void Awake()
    {
        // 인스턴스가 존재하지 않으면 현재 인스턴스로 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            // 인스턴스가 이미 존재하면 현재 오브젝트를 파괴
            Destroy(this);
        }

        // 카메라 스크립트들을 할당
        fpsCamera = GetComponent<FPSCamera>();
        tpsCamera = GetComponent<TPSCamera>();

        // 초기 카메라 모드를 FPS로 설정
        SetCameraMode(CameraMode.TPS);
    }

    // 외부에서 호출하여 카메라 모드를 설정하는 메서드
    public void SetCameraMode(CameraMode mode) 
    {
        // 입력된 모드로 현재의 카메라 모드를 변경
        Mode = mode;

        /*fpsCamera.enabled = (mode == CameraMode.FPS);
        tpsCamera.enabled = (mode == CameraMode.TPS);*/
    }
}
