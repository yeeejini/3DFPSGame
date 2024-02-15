using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject BombEffectPrefabs;
    // 플레이어를 제외하고 물체에 닿으면
    // 자기 자신을 사라지게 하는 코드 작성
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);

        // 수류탄이 폭발할 때 폭발 이펙트를 자기 위치에 생성하기
        GameObject bombfx = Instantiate(BombEffectPrefabs);
        bombfx.transform.position = this.transform.position;
    }
}
