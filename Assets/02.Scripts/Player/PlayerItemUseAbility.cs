using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerItemUseAbility : MonoBehaviour
{
    public UnityEvent OnDataChanged;
    void Update()
    {
        if (GameManager.Instance.State != GameState.Go)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            bool result = ItemManager.Instance.TryUseItem(ItemType.Health);
            if (result)
            {
                // todo : 아이템 효과음 재생
                // todo : 파티클 시스템 재생
            }
            else 
            {
                // todo : 알림창 (아이템이 부족합니다.)
            }
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            ItemManager.Instance.TryUseItem(ItemType.Stamina);
        }
        else if (Input.GetKeyDown(KeyCode.U)) 
        {
            ItemManager.Instance.TryUseItem(ItemType.Bullet);
        }
    }
}
