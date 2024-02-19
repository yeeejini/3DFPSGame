using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour, IHitable
{
    [Range(0, 100)]  // int, float 변수를 슬라이드바로 표시하고 범위를 제한함
    public int Health;
    public int MaxHealth = 100;

    [Header("몬스터 슬라이더 UI")]
    public Slider MonsterHealthSliderUI;

    public void Hit(int damage)
    {
        Health -= damage;
        if (Health < 0) 
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        Health = MaxHealth;
    }
    private void Update()
    {
        MonsterHealthSliderUI.value = (float)Health / (float)MaxHealth;
    }
}
