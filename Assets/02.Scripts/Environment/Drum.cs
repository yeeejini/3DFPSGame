using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour, IHitable
{
    // Drum 체력
    private int _healthCount = 0;

    // 폭발 프리펩
    public GameObject DrumEffectPrefabs;


    Rigidbody _rigidbody;
    // 폭발 세기
    public float Power = 20f;

    // 폭발 범위
    public float ExplosionRadius = 3f;
    // 폭발 데미지
    public int Damage = 70;

    // 지속 시간
    public float EXPLOSION_TIME = 3f;

    // 폭발 상태
    private bool _isExplosion = false;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void Hit(int damage)
    {
        // 피격 시 체력횟수 증가 및 일정 체력 이상이면 폭발
        _healthCount += 1;
        if (_healthCount >= 3) 
        {
            ExplosionDrum();
        }
    }
    public void ExplosionDrum() 
    {
        // 이미 폭발 중인 경우 더 이상 실행하지 않음
        if (_isExplosion) 
        {
            return;
        }
        _isExplosion = true;

        // 폭발 힘과 회전 힘을 가하여 드럼통 폭발
        _rigidbody.AddForce(transform.up * Power, ForceMode.Impulse);
        _rigidbody.AddTorque(new Vector3(1, 0, 1) * Power / 2f);

        

        // 폭발 이펙트 생성
        GameObject drumfx = Instantiate(DrumEffectPrefabs);
        drumfx.transform.position = this.transform.position;

        

        // 몬스터 및 플레이어에게 데미지
        int findlayer = LayerMask.GetMask("Monster") | LayerMask.GetMask("Player");
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, findlayer);

        foreach (Collider collider in colliders)
        {
            IHitable hitable = collider.GetComponent<IHitable>();
            if (hitable != null)
            {
                hitable.Hit(Damage);
            }
        }

        int environmentLayer = LayerMask.GetMask("Environment");
        Collider[] environmentColliders = Physics.OverlapSphere(transform.position, ExplosionRadius, environmentLayer);
        foreach (Collider c in environmentColliders)
        {
            Drum drum = null;
            if (c.TryGetComponent<Drum>(out drum)) 
            {
                drum.ExplosionDrum();
            }
        }
        // 일정 시간 후에 폭발 효과 종료
        StartCoroutine(Coroutine_Explosion());
        ItemObjectFactory.Instance.MakePercent(transform.position);
    }
    private IEnumerator Coroutine_Explosion() 
    {
        yield return new WaitForSeconds(EXPLOSION_TIME);

        Destroy(gameObject);
    }
}
