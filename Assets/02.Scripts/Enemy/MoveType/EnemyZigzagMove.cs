using UnityEngine;

public class EnemyZigzagMove : MonoBehaviour
{
    [Header("Zigzag 움직임 설정")]
    public float Speed = 2f;
    private float Frequency = 5f;            // S자의 진동수
    private float Magnitude = 1f;          // S자의 폭 (진폭)
    private Vector3 basePos;                // 직선 기준 위치
    private float t;                        // 경과 시간

    private void Awake()
    {
        basePos = transform.position;   // 초기 위치 저장
    }
    void Update()
    {
        // 시간 누적
        t += Time.deltaTime;
        _zigzagMode();
    }

    private void _zigzagMode()
    {
        basePos += Vector3.down * Speed * Time.deltaTime;       // 직선 이동
        float offsetX = Mathf.Sin(t * Frequency) * Magnitude;           // S자 궤적 계산
        transform.position = basePos + new Vector3(offsetX, 0f, 0f);    // 기준 위치 + X축 오프셋
    }
}
