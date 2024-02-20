using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour, IHitable
{
    private int _healthCount = 0;

    public GameObject DrumEffectPrefabs;

    Rigidbody _rigidbody;
    public float Power = 20f;

    public float ExplosionRadius = 3f;
    public int Damage = 70;

    public float EXPLOSION_TIME = 3f;

    private bool _isExplosion = false;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void Hit(int damage)
    {
        _healthCount += 1;
        if (_healthCount >= 3) 
        {
            ExplosionDrum();
        }
    }
    public void ExplosionDrum() 
    {
        if (_isExplosion) 
        {
            return;
        }
        _isExplosion = true;


        _rigidbody.AddForce(transform.up * Power, ForceMode.Impulse);
        _rigidbody.AddTorque(new Vector3(1, 0, 1) * Power / 2f);

        StartCoroutine(Coroutine_Explosion());

        GameObject drumfx = Instantiate(DrumEffectPrefabs);
        drumfx.transform.position = this.transform.position;

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

        Destroy(gameObject);
    }
    private IEnumerator Coroutine_Explosion() 
    {
        yield return new WaitForSeconds(EXPLOSION_TIME);
    }
}
