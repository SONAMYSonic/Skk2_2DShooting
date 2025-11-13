using UnityEngine;

public class EnemyZigzagMove : MonoBehaviour
{
    [Header("Zigzag 움직임 설정")]
    public float Speed = 2f;

    private float _frequency = 5f;            // S자의 진동수
    private float _magnitude = 1f;          // S자의 폭 (진폭)
    private Vector3 _basePos;                // 직선 기준 위치
    private float _time;                        // 경과 시간

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        _basePos = transform.position;   // 초기 위치 저장
    }
    private void Update()
    {
        // 시간 누적
        _time += Time.deltaTime;
        ZigzagMode();
    }

    private void ZigzagMode()
    {
        _basePos += Vector3.down * Speed * Time.deltaTime;       // 직선 이동
        float offsetX = Mathf.Sin(_time * _frequency) * _magnitude;           // S자 궤적 계산
        transform.position = _basePos + new Vector3(offsetX, 0f, 0f);    // 기준 위치 + X축 오프셋
    }
}
