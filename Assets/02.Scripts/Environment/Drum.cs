using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour, IHitable
{
    private int _healthCount = 0;
   
    public void Hit(int damage)
    {
        _healthCount += 1;
        if (_healthCount >= 3) 
        {
            Destroy(gameObject);
        }
    }
}
