using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUseAbility : MonoBehaviour
{

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
                ItemManager.Instance.RefreshUI();
            }
            else 
            {
                // todo : 알림창 (아이템이 부족합니다.)
            }
            ItemManager.Instance.RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            ItemManager.Instance.TryUseItem(ItemType.Stamina);
            ItemManager.Instance.RefreshUI();   
        }
        else if (Input.GetKeyDown(KeyCode.U)) 
        {
            ItemManager.Instance.TryUseItem(ItemType.Bullet);
            ItemManager.Instance.RefreshUI();
        }
    }
}
