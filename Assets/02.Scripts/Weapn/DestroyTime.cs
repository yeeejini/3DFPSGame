using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTime : MonoBehaviour
{
    // 파괴까지의 시간
    public float DeleteTime = 1.5f;
    // 경과 시간을 측정하기 위한 타이머
    private float _timer = 0;

    
    private void Update()
    {
        // 타이머를 누적하여 경과 시간을 계산
        _timer += Time.deltaTime;
        // 경과 시간이 설정한 파괴 시간을 초과하면
        if (_timer >= DeleteTime) 
        {
            // 자신의 게임 오브젝트를 파괴
            Destroy(gameObject);
        }
    }
}
