using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBombFireAbility : MonoBehaviour
{
    // 목표 : 마우스 오른쪽 버튼을 누르면 시선이 바라보는 방향으로 수류탄을 던지고 싶다.
    // 필요 속성 :
    // - 수류탄 프리펩
    public GameObject BombPrefab;
    // - 수류탄 던지는 위치
    public Transform FirePosition;
    // - 수류탄 던지는 파워
    public float ThrowPower = 15f;

    // 폭탄 개수 3개로 제한 / ("현재 남은 개수 / 최대 개수") 형태로 왼쪽 하단 스태미너 UI 위에 text로 표시하기 (ex. 1/3)
    public int MaxBombCount = 10;
    private int presentbombcount;

    // UI에 표시할 텍스트
    public Text BombCountTextUI;

    // 수류탄을 담을 오브젝트 풀
    public List<GameObject> BombPool;
    public int BombPoolSize = 5;

    private void Start()
    {
        // 오브젝트 풀 초기화
        BombPool = new List<GameObject>();
        // 지정된 풀 크기만큼 반복하며 수류탄 오브젝트를 생성하고 비활성화 상태로 오브젝트 풀에 추가
        for (int i = 0; i < BombPoolSize; i++) 
        {
            // 수류탄 프리팹을 복제하여 수류탄 오브젝트 생성
            GameObject bombObject = Instantiate(BombPrefab);
            // 수류탄 오브젝트를 비활성화 상태로 설정
            bombObject.SetActive(false);
            // 수류탄 오브젝트를 오브젝트 풀에 추가
            BombPool.Add(bombObject);
        }

        // 시작 시 최대 수류탄 개수로 초기화하고 UI 갱신
        presentbombcount = MaxBombCount;
        RefreshUI();
    }

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Go)
        {
            return;
        }
        // ** 수류탄 투척 **
        // 1. 마우스 오른쪽 버튼을 감지
        if (Input.GetMouseButtonDown(1) && presentbombcount > 0)  // 왼쪽 0, 휠 2, 오른쪽 1
        {
            // 수류탄 개수 감소 및 UI 갱신
            presentbombcount--;
            RefreshUI();

            // 2. 수류탄 던지는 위치에다가 수류탄 생성 (창고에서 꺼내고)
            GameObject bomb = null;
            for (int i = 0; i < BombPool.Count; i++) 
            {
                // 수류탄 오브젝트가 비활성화 상태인지 확인
                if (BombPool[i].activeInHierarchy == false) 
                {
                    // 비활성화된 수류탄 오브젝트를 찾았으므로 해당 오브젝트를 활성화하고 루프 탈출
                    bomb = BombPool[i];
                    bomb.SetActive(true);
                    break;
                }
            }
            // 활성화된 수류탄 오브젝트의 위치를 던질 위치(FirePosition)로 설정
            bomb.transform.position = FirePosition.position;


            // 3. 시선이 바라보는 방향(카메라가 바라보는 방향 = 카메라의 전반) 으로 수류탄 투척
            Rigidbody rigidbody = bomb.GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Camera.main.transform.forward * ThrowPower, ForceMode.Impulse);
        }
    }
    void RefreshUI() 
    {
        BombCountTextUI.text = $"{presentbombcount} / {MaxBombCount}";
    }
}
