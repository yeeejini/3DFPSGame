using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemInventory : MonoBehaviour
{

    public TextMeshProUGUI HealthItemCountTextUI;
    public TextMeshProUGUI StaminaItemCountTextUI;
    public TextMeshProUGUI BulletItemCountTextUI;

   

    // UI를 새로고침 하는 함수
    public void Refresh()
    {
        HealthItemCountTextUI.text = $"x{ItemManager.Instance.GetItemCount(ItemType.Health)}";
        StaminaItemCountTextUI.text = $"x{ItemManager.Instance.GetItemCount(ItemType.Stamina)}";
        BulletItemCountTextUI.text = $"x{ItemManager.Instance.GetItemCount(ItemType.Bullet)}";
    }

}
