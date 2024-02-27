using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerGunFireAbility : MonoBehaviour
{
    public Gun CurrentGun; // 현재 들고있는 총
    private int _currentGunIndex; // 현재 들고있는 총의 순서

    // 목표 : 마우스 왼쪽 버튼을 누르면 시선이 바라보는 방향으로 총을 발사하고 싶다.
    // 필요 속성
    // - 총알 튀는 이펙트 프리팹
    public ParticleSystem HitEffect;
    // 구현 순서
    // 1. 만약에 마우스 왼쪽 버튼을 누르면
    // 2. 레이(광선)을 생성하고, 위치와 방향을 설정한다.
    // 3. 레이를 발사한다.
    // 4. 레이가 부딪힌 대상의 정보를 받아온다.
    // 5. 부딪힌 위치에 (총알이 튀는)이펙트를 생성한다.

    

    // - 총알 개수 텍스트 UI
    public TextMeshProUGUI BulletTextUI;

    // 장전코루틴
    private bool _isReloading = false;       // 재장전 중이냐?
    public GameObject ReloadTextObject;

    // 무기 이미지 UI
    public Image GunImageUI;

    private float _timer;

    // 총을 담는 인벤토리
    public List<Gun> GunInventory;

    private const int DefaultFOV = 60;
    private const int ZoomFOV = 20;
    private bool isZoomMode = false; // 줌 모드냐?
    private const float ZoomInDuration = 0.3f;
    private const float ZoomOutDuration = 0.2f;
    private float _zoomProgress;  // 0 ~ 1


    //public Image ZoomModeImageUI;
    public GameObject CrosshairUI;
    public GameObject CrosshairZoomUI;

    private Animator _animator;

    private void Start()
    {
        _currentGunIndex = 0;

        RefreshUI();
        RefreshGun();

        _animator = GetComponentInChildren<Animator>();
    }
    
    void Update()
    {
        if (GameManager.Instance.State != GameState.Go)
        {
            return;
        }
        // 마우스 휠 버튼(v로 변경했음) 눌렀을 때 && 현재 총이 스나이퍼
        if (Input.GetKeyDown(KeyCode.V) && CurrentGun.GType == GunType.Sniper)
        {
            isZoomMode = !isZoomMode;  // 줌 모드 뒤집기 
            _zoomProgress = 0f;
            RefreshUI();
        }

        if (CurrentGun.GType == GunType.Sniper && _zoomProgress < 1) 
        {
            if (isZoomMode) 
            {
                _zoomProgress += Time.deltaTime / ZoomInDuration;
                Camera.main.fieldOfView = Mathf.Lerp(DefaultFOV, ZoomFOV, _zoomProgress);
            }
            else
            {
                _zoomProgress += Time.deltaTime / ZoomOutDuration;
                Camera.main.fieldOfView = Mathf.Lerp(ZoomFOV, DefaultFOV, _zoomProgress);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            // 뒤로가기
            _currentGunIndex--;
            if (_currentGunIndex < 0) 
            {
                _currentGunIndex = GunInventory.Count - 1;
            }
            CurrentGun = GunInventory[_currentGunIndex];

            isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();

            RefreshGun();
            RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket)) 
        {
            // 앞으로 가기
            _currentGunIndex++;
            if (_currentGunIndex >= GunInventory.Count) 
            {
                _currentGunIndex = 0;
            }
            CurrentGun = GunInventory[_currentGunIndex];

            isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();

            RefreshGun();
            RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentGunIndex = 0;
            CurrentGun = GunInventory[0];

            isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();

            RefreshGun();
            RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentGunIndex = 1;
            CurrentGun = GunInventory[1];

            isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();

            RefreshGun();
            RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) 
        {
            _currentGunIndex = 2;
            CurrentGun = GunInventory[2];

            isZoomMode = false;
            _zoomProgress = 1f;
            RefreshZoomMode();

            RefreshGun();
            RefreshUI();
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!_isReloading) 
            {
                StartCoroutine(Reload_Coroutine());
            }
        }
        ReloadTextObject.SetActive(_isReloading);

        _timer += Time.deltaTime;

        // 구현 순서
        // 1. 만약에 마우스 왼쪽 버튼을 누르면(실습 과제 13. 마우스 왼쪽 버튼 누르고  있으면 연사 (쿨타임 적용))
        if (Input.GetMouseButton(0) && _timer >= CurrentGun.FireCooltime && CurrentGun.BulletRemainCount > 0) 
        {
            if(_isReloading) 
            {
                // 재장전 취소
                StopAllCoroutines();
                _isReloading = false;
            }

            _animator.SetTrigger("Shoot");


            CurrentGun.BulletRemainCount -= 1;
            RefreshUI();


            _timer = 0;

            // 2. 레이(광선)을 생성하고, 위치와 방향을 설정한다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            // 3. 레이를 발사한다.
            // 4. 레이가 부딪힌 대상의 정보를 받아온다.
            RaycastHit hitInfo;
            bool IsHit = Physics.Raycast(ray, out hitInfo);
            
            if(IsHit) 
            {
                // 실습 과제 18. 레이저를 몬스터에게 맞출 시 몬스터 체력 닳는 기능 구현
                // 레이저랑 몬스터를 부딪히게..
                /*if (hitInfo.collider.CompareTag("Monster")) 
                {
                    Monster monster = hitInfo.collider.GetComponent<Monster>();
                    monster.Hit(Damage);
                }*/
                IHitable hitObject = hitInfo.collider.GetComponent<IHitable>();
                if (hitObject != null)   // 때릴 수 있는 친구인가요?
                {
                    hitObject.Hit(CurrentGun.Damage);
                }


                // 5. 부딪힌 위치에 (총알이 튀는)이펙트를 위치한다.
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. 이펙트가 쳐다보는 방향을 부딪힌 위치의 법선 벡터로 한다.
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play();
            }
           
        }
    }
    private void RefreshGun() 
    {
        foreach (Gun gun in GunInventory) 
        {
            gun.gameObject.SetActive(gun == CurrentGun);
        }
    }
    public void RefreshUI()
    {
        GunImageUI.sprite = CurrentGun.ProfileImage;
        BulletTextUI.text = $"{CurrentGun.BulletRemainCount:d2} / {CurrentGun.BulletMaxCount}";

        CrosshairUI.SetActive(!isZoomMode);
        CrosshairZoomUI.SetActive(isZoomMode);
    }
    private IEnumerator Reload_Coroutine()
    {
        _isReloading = true;

        // R키 누르면 1.5초 후 재장전, (중간에 총 쏘는 행위를 하면 재장전 취소)
        yield return new WaitForSeconds(CurrentGun.ReloadTime);
        CurrentGun.BulletRemainCount = CurrentGun.BulletMaxCount;
        RefreshUI();

        _isReloading = false;
    }
    // 줌 모드에 따라 카메라 FOV 수정해주는 메서드
    void RefreshZoomMode()
    {
        if (!isZoomMode) 
        {
            Camera.main.fieldOfView = DefaultFOV;
        }
        else
        {
            Camera.main.fieldOfView = ZoomFOV;
        }
    }
}
