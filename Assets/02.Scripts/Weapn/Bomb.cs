using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // 수류탄이 폭발할 때 생성할 이펙트 프리팹
    public GameObject BombEffectPrefabs;
    
    private void OnCollisionEnter(Collision collision)
    {
        // 수류탄을 비활성화하여 풀로 반환하거나 제거
        gameObject.SetActive(false);

        // 수류탄이 폭발할 때 폭발 이펙트를 자기 위치에 생성하기
        GameObject bombfx = Instantiate(BombEffectPrefabs);
        // 생성된 이펙트 위치를 수류탄의 위치로 설정
        bombfx.transform.position = this.transform.position;
    }
}
