 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum ItemState
{
    Idle,   // 대기상태
    Trace,  // 날아오는 상태(N초에 걸쳐서 Slerp로 플레이어에게 날라온다.)
}
public class ItemObject : MonoBehaviour
{
    public ItemType ItemType;

    private ItemState _itemState = ItemState.Idle;
    private Transform _targetPlayer;
    public float EatDistance = 10f;

    private Vector3 _itemStartPosition;
    private const float ITEM_DURATION = 0.3f;
    private float _itemProgress = 0f;

    private void Start()
    {
        _targetPlayer = GameObject.FindWithTag("Player").transform;
        _itemStartPosition = transform.position;   
    }
    private void Update()
    {
        switch (_itemState)
        {
            case ItemState.Idle:
            {
                Idle();
                break;
            }
            case ItemState.Trace:
            {
                Trace();
                break;
            }
        }
    }

    public void Init() 
    {
        _itemProgress = 0;
        _traceCoroutine = null;
    }

    private void Idle() 
    {
        float distance = Vector3.Distance(_targetPlayer.position, transform.position);
        if(distance <= EatDistance) 
        {
            _itemState = ItemState.Trace;
        }
    }
    private Coroutine _traceCoroutine;
    private void Trace() 
    {
        if (_traceCoroutine == null) 
        {
            _traceCoroutine = StartCoroutine(Trace_Coroutine());
        }
    }
    private IEnumerator Trace_Coroutine() 
    {
        // 날라오는 상태를 update가 아닌 코루틴 방식으로 변경
        
        while (_itemProgress < 0.6f)
        {
            _itemProgress += Time.deltaTime / ITEM_DURATION;
            transform.position = Vector3.Slerp(_itemStartPosition, _targetPlayer.position, _itemProgress);
            yield return null;
        }
        // 1. 아이템 매니저(인벤토리)에 추가하고,
        ItemManager.Instance.AddItem(ItemType);
        ItemManager.Instance.RefreshUI();

        // 2. 사라진다.
        gameObject.SetActive(false);
    }

    // Todo 1. 아이템 프리펩을 3개(Health, Stamina, Bullet) 만든다. (도형이나 색깔 다르게 해서 구현)
    // Todo 2. 플레이어와 일정 거리가 되면 아이템이 먹어지고 사라진다.


    // 실습 과제 31. 몬스터가 죽으면 아이템이 드랍(Health: 20%, Stamina: 20%, Bullet: 10%)
    // 실습 과제 32. 일정 거리가 되면 아이템이 베지어 곡선으로 날아오게 하기


}
