using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // 수류탄이 폭발할 때 생성할 이펙트 프리팹
    public GameObject BombEffectPrefabs;

    // 목표 : 수류탄 폭발 범위 데미지 기능 구현
    // 필요 속성
    // - 범위
    public float ExplosionRadius = 3;
    // 구현 순서
    // 1. 터질 때
    // 2. 범위 안에 있는 모든 콜라이더를 찾는다.
    // 3. 찾은 콜라이더 중에서 타격 가능한(IHitable) 오브젝트를 찾는다.
    // 4. Hit() 한다.

    public int Damage = 60;

    // 구현 순서
    // 1. 터질 때
    private void OnCollisionEnter(Collision other)
    {
        // 수류탄을 비활성화하여 풀로 반환하거나 제거
        gameObject.SetActive(false);

        // 수류탄이 폭발할 때 폭발 이펙트를 자기 위치에 생성하기
        GameObject bombfx = Instantiate(BombEffectPrefabs);
        // 생성된 이펙트 위치를 수류탄의 위치로 설정
        bombfx.transform.position = this.transform.position;


        // 2. 범위 안에 있는 모든 콜라이더를 찾는다.
        // -> 피직스.오버랩 함수는 특정 영역(radius) 안에 있는 특정 레이어들의 게임 오브젝트의
        //    콜라이더 컴포넌트들을 모두 찾아 배열로 반환하는 함수
        // 영역의 형태 : 스피어, 큐브, 캡슐                                                          
        int layer = LayerMask.GetMask("Monster") | LayerMask.GetMask("Player");
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, layer); 


        // 3. 찾은 콜라이더 중에서 타격 가능한(IHitable) 오브젝트를 찾는다.
        // 4. Hit() 한다.
        foreach (Collider collider in colliders) 
        {
            IHitable hitable = collider.GetComponent<IHitable>();
            if (hitable != null) 
            {
                hitable.Hit(Damage);
            }
        }
    }
}
