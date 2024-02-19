using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour, IHitable
{
    private int _healthCount = 0;

    public GameObject DrumEffectPrefabs;

    Rigidbody rigidbody;
    public float Power = 20f;

    public float ExplosionRadius = 3f;
    public int Damage = 70;

    public float EXPLOSION_TIME = 3f;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    public void Hit(int damage)
    {
        _healthCount += 1;
        if (_healthCount >= 3) 
        {
            StartCoroutine(ExplosionDrum());
        }
    }
    public IEnumerator ExplosionDrum() 
    {
        rigidbody.AddForce(transform.up * Power, ForceMode.Impulse);
        rigidbody.AddTorque(new Vector3(1, 0, 1) * Power / 2f);

        yield return new WaitForSeconds(EXPLOSION_TIME);

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

        Destroy(gameObject);
    }
}
