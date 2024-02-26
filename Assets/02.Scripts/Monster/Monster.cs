using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum MonsterState  // 몬스터의 상태
{
    Idle,     // 대기
    Trace,    // 추적
    Attack,   // 공격
    Comeback,   // 복귀
    Damaged,  // 공경 당함
    Die       // 사망
}
public class Monster : MonoBehaviour, IHitable
{
    [Range(0, 100)]  // int, float 변수를 슬라이드바로 표시하고 범위를 제한함
    public int Health;
    public int MaxHealth = 100;

    [Header("몬스터 슬라이더 UI")]
    public Slider MonsterHealthSliderUI;

   // private CharacterController _characterController;
    private NavMeshAgent _navMeshAgent;

    private MonsterState _currentState = MonsterState.Idle;

    private Transform _target;           // 플레이어
    public float FindDistance = 5f;      // 감지 거리
    public float AttackDistance = 2f;    // 공격 범위
    public float MoveSpeed = 2f;
    public Vector3 StartPosition;        // 시작 위치
    public float MoveDistance = 40f;     // 움직일 수 있는 거리
    public const float TOLERANCE = 0.1f; // 허용 오차
    public int Damage = 10;
    public float AttackDelay = 0.5f;
    private float _attackTimer = 0f;

    // 넉백
    private Vector3 _knockbackStartPosition;
    private Vector3 _knockbackEndPosition;
    private const float KNOCKBACK_DURATION = 0.1f;
    private float _knockbackProgress = 0f;
    public float KnockbackPower = 1.2f;


    private void Start()
    {
       // _characterController = GetComponent<CharacterController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = MoveSpeed;

        _target = GameObject.FindGameObjectWithTag("Player").transform;

        StartPosition = transform.position;

        Init();
    }
    private void Update()
    {
        MonsterHealthSliderUI.value = (float)Health / (float)MaxHealth;

        if (GameManager.Instance.State != GameState.Go)
        {
            return;
        }

        // 상태 패턴: 상태에 따라 행동을 다르게 하는 패턴
        // 1. 몬스터가 가질 수 있는 행동에 따라 상태를 나눈다.
        // 2. 상태들이 조건에 따라 자연스럽게 전환(Transition) 되게 설계한다.

        switch (_currentState) 
        {
            case MonsterState.Idle:
            {
                Idle();
                break;
            }
            case MonsterState.Trace: 
            {
                Trace();
                break;
            }
            case MonsterState.Comeback: 
            {
                Comeback(); 
                break;
            }
            case MonsterState.Attack: 
            {
                Attack(); 
                break;
            }
            case MonsterState.Damaged: 
            {
                Damaged(); 
                break;
            }
        }
    }

    private void Idle() 
    {
        // todo : 몬스터의 Idle 애니메이션 재생

        // 플레이어와의 거리가 일정 범위 안이면
        if (Vector3.Distance(_target.position, transform.position) <= FindDistance) 
        {
            Debug.Log("상태 전환: Idle -> Trace");
            _currentState = MonsterState.Trace;
        }
    }
    private void Trace() 
    {
        // Trace 상태일 때의 행동 코드를 작성
        // 플레이어에게 다가간다.


        // Trace 상태일 때 몬스터가 플레이어 쳐다보면서 추적 구현 / 조건에 따라 Attack 상태로 전이 구현
        Vector3 dir = _target.transform.position - this.transform.position;
        dir.y = 0;
        dir.Normalize();

        // _characterController.Move(dir * MoveSpeed * Time.deltaTime);

        // 내비게이션이 접근하는 최소 거리를 공격 가능 거리로 설정
        _navMeshAgent.stoppingDistance = AttackDistance;

        // 내비게이션의 목적지를 플레이어의 위치로 한다.
       _navMeshAgent.destination = _target.position;

       // transform.forward = dir;  // _target

        if (Vector3.Distance(transform.position, StartPosition) >= MoveDistance) // 원점과의 거리가 너무 멀어지면 
        {
            Debug.Log("상태 전환: Trace -> Comeback");
            _currentState = MonsterState.Comeback;
        }
        if (Vector3.Distance(_target.position, transform.position) <= AttackDistance) // 거리가 공격 범위 안이면 
        {
            Debug.Log("상태 전환: Trace -> Attack");
            _currentState = MonsterState.Attack;
        }

    }
    private void Comeback() 
    {
        // 복귀 상태의 행동 구현하기: 시작 지점 쳐다보면서 시작지점으로 이동하기 (이동 완료하면 다시 Idle 상태로 전환)
        Vector3 dir = StartPosition - transform.position;
        dir.y = 0;
        dir.Normalize();

        // 내비게이션이 접근하는 최소 거리를 오차 범위
        _navMeshAgent.stoppingDistance = TOLERANCE;

        // 내비게이션의 목적지를 플레이어의 위치로 한다.
        _navMeshAgent.destination = StartPosition;

        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE) 
        {
            Debug.Log("상태 전환: Comeback -> Idle");
            _currentState = MonsterState.Idle;
        }

        if (Vector3.Distance(transform.position, StartPosition) <= TOLERANCE)
        {
            Debug.Log("상태 전환: Comeback -> Idle");
            _currentState = MonsterState.Idle;
        }
    }
    private void Attack() 
    {
        // 전이 사건: 플레이어와 거리가 공격 범위보다 멀어지면 다시 Trace
        if (Vector3.Distance(_target.position, transform.position) > AttackDistance) 
        {
            _attackTimer = 0f;
            Debug.Log("상태 전환: Attack -> Trace");
            _currentState = MonsterState.Trace;
            return;
        }
        _attackTimer += Time.deltaTime;
        if(_attackTimer >= AttackDelay) 
        {
            // _target.GetComponent<IHitable>().Hit(Damage);
            IHitable playerHitable = _target.GetComponent<IHitable>();
            if (playerHitable != null) 
            {
                playerHitable.Hit(Damage);
                _attackTimer = 0f;
            }
        }
    }
    private void Damaged() 
    {
        // 1. Damaged 애니메이션 실행(0.5초)
        // 2. 넉백(Lerp -> 0.5초)
        // 2-1. 넉백 시작/최종 위치를 구한다.
        if (_knockbackProgress == 0) 
        {
            _knockbackStartPosition = transform.position;
            Vector3 dir = transform.position - _target.position;
            dir.y = 0;
            dir.Normalize();

            _knockbackEndPosition = transform.position + dir * KnockbackPower;
        }
        _knockbackProgress += Time.deltaTime / KNOCKBACK_DURATION;
        // 2-2. Lerp를 이용해 넉백하기
        transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackProgress);

        if (_knockbackProgress > 1) 
        {
            _knockbackProgress = 0;

            Debug.Log("상태 전환: Damaged -> Trace");
            _currentState = MonsterState.Trace;
        }
    }

    public void Hit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
        else 
        {
            Debug.Log("상태 전환: Any -> Damaged");
            _currentState = MonsterState.Damaged;
        }
    }
    public void Init()
    {
        Health = MaxHealth;
    }
    private void Die() 
    {
        ItemObjectFactory.Instance.MakePercent(transform.position);

        Destroy(gameObject);
    }
}
