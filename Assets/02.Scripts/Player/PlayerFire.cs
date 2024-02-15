using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
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
    public int MaxBombCount = 3;
    private int presentbombcount;

    public Text BombCountTextUI;

    private void Start()
    {
        presentbombcount = MaxBombCount;

        RefreshUI();
    }

    private void Update()
    {
        // ** 수류탄 투척 **
        // 1. 마우스 오른쪽 버튼을 감지
        if (Input.GetMouseButtonDown(1) && presentbombcount > 0)  // 왼쪽 0, 휠 2, 오른쪽 1
        {
            presentbombcount--;

            RefreshUI();

            // 2. 수류탄 던지는 위치에다가 수류탄 생성
            GameObject bomb = Instantiate(BombPrefab);
            bomb.transform.position = FirePosition.position;

            // 3. 시선이 바라보는 방향(카메라가 바라보는 방향 = 카메라의 전반) 으로 수류탄 투척
            Rigidbody rigidbody = bomb.GetComponent<Rigidbody>();
            rigidbody.AddForce(Camera.main.transform.forward * ThrowPower, ForceMode.Impulse);
        }
    }
    void RefreshUI() 
    {
        BombCountTextUI.text = $"{presentbombcount} / {MaxBombCount}";
    }
}
