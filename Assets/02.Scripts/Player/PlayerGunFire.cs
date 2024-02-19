using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerGunFire : MonoBehaviour
{
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

    // - 발사 쿨타임
    public float FireCooltime = 0.2f;
    private float _timer;

    // - 총알 개수
    public int BulletRemainCount;
    public int BulletMaxCount = 30;

    // - 총알 개수 텍스트 UI
    public Text BulletTextUI;

    // 장전코루틴
    private const float RELOAD_TIME = 1.5f;  // 재장전 시간
    private bool _isReloading = false;       // 재장전 중이냐?


    private void Start()
    {
        // 총알 개수 초기화
        BulletRemainCount = BulletMaxCount;
        RefreshUI();
    }
    private void RefreshUI() 
    {
        BulletTextUI.text = $"{BulletRemainCount:d2}/{BulletMaxCount}";
    }
    private IEnumerator Reload_Coroutine() 
    {
        _isReloading = true;

        // R키 누르면 1.5초 후 재장전, (중간에 총 쏘는 행위를 하면 재장전 취소)
        yield return new WaitForSeconds(RELOAD_TIME);
        BulletRemainCount = BulletMaxCount;
        RefreshUI();

        _isReloading = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!_isReloading) 
            {
                StartCoroutine(Reload_Coroutine());
            }
        }

        _timer += Time.deltaTime;

        // 구현 순서
        // 1. 만약에 마우스 왼쪽 버튼을 누르면(실습 과제 13. 마우스 왼쪽 버튼 누르고  있으면 연사 (쿨타임 적용))
        if (Input.GetMouseButton(0) && _timer >= FireCooltime && BulletRemainCount > 0) 
        {
            // 재장전 취소
            StopAllCoroutines();
            _isReloading = false;


            BulletRemainCount -= 1;
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
                // 5. 부딪힌 위치에 (총알이 튀는)이펙트를 위치한다.
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. 이펙트가 쳐다보는 방향을 부딪힌 위치의 법선 벡터로 한다.
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play();
            }
        }
    }
}
