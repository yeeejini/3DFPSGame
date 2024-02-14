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
    private FPSCamera fpsCamera;
    private TPSCamera tpsCamera;

    public static CameraManager Instance { get; private set; }

    public CameraMode Mode = CameraMode.FPS;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(this);
        }

        fpsCamera = GetComponent<FPSCamera>();
        tpsCamera = GetComponent<TPSCamera>();

        SetCameraMode(CameraMode.FPS);
    }

    public void SetCameraMode(CameraMode mode) 
    {
        Mode = mode;

        /*fpsCamera.enabled = (mode == CameraMode.FPS);
        tpsCamera.enabled = (mode == CameraMode.TPS);*/
    }
}
