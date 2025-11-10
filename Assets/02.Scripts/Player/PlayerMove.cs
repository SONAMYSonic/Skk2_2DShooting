using UnityEngine;

// 플레이어 이동
public class PlayerMove : MonoBehaviour
{
    // 목표
    // 1. 키보드 입력에 따라 방향을 구하고 그 방향으로 이동시키고 싶다
    // 2. Q/E 키로 스피드를 조절하고 싶다.
    // 3. 특정 영역을 벗어나지 못하게 하고 싶다.

    // 필요 속성
    [Header("능력치")]
    public float MaxSpeed = 20f;          // 최대 이동 속도
    public float MinSpeed = 1f;           // 최소 이동 속도
    public float DashSpeedMultiplier = 1f; // 대시 시 속도 배율
    [SerializeField]
    private float _speed = 3f;            // 현재 이동 속도

    [Header("이동범위")]
    public float MinX = -3f;   // 좌측 경계
    public float MaxX = 3f;    // 우측 경계
    public float MinY = -4.5f; // 하단 경계
    public float MaxY = 1f;    // 상단 경계

    [Header("기타")]
    public bool isDash = false; // 대시 상태 여부

    [Header("시작 위치")]
    [SerializeField]
    private Vector3 _originPosition = new Vector3(0f, -3f, 0f); // 원점 위치

    // Speed를 리플레이에서 건드릴 수 있도록 프로퍼티로 노출
    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    // 한 프레임 동안 들어온 입력을 기준으로 플레이어를 움직인다.
    public void ApplyInput(
        float deltaTime,
        float h,
        float v,
        bool qHeld,
        bool eHeld,
        bool shiftDown,
        bool shiftUp,
        bool rHeld)
    {
        // 1. 스피드 조작 (Q: 스피드 업, E: 스피드 다운)
        if (qHeld) _speed += deltaTime * 10f; // 부드럽게 속도 증가
        if (eHeld) _speed -= deltaTime * 10f; // 부드럽게 속도 감소

        // Speed를 MinSpeed와 MaxSpeed 사이로 제한
        _speed = Mathf.Clamp(_speed, MinSpeed, MaxSpeed);

        // 1-1. Shift 키 "에지" 기반 대시 토글
        if (shiftDown && isDash == false)
        {
            isDash = true;
            DashSpeedMultiplier = 2f;
        }
        else if (shiftUp && isDash == true)
        {
            isDash = false;
            DashSpeedMultiplier = 1f;
        }

        // 2. 입력으로부터 방향을 구하고 정규화
        Vector2 direction = new Vector2(h, v).normalized;

        // 3. 새로운 위치 계산
        Vector3 newPosition = transform.position +
                              (Vector3)direction * (_speed * DashSpeedMultiplier) * deltaTime;

        // 4. 제한된 영역 내로 보정
        newPosition.x = Mathf.Clamp(newPosition.x, MinX, MaxX);
        newPosition.y = Mathf.Clamp(newPosition.y, MinY, MaxY);

        // 5. 보정된 위치로 이동
        transform.position = newPosition;

        // 6. 화면 좌우 래핑
        if (transform.position.x <= MinX)
        {
            transform.position = new Vector3(MaxX, transform.position.y, transform.position.z);
        }
        else if (transform.position.x >= MaxX)
        {
            transform.position = new Vector3(MinX, transform.position.y, transform.position.z);
        }

        // 7. R 키: 원점 방향으로 점점 이동
        if (rHeld)
        {
            // 원점 방향 벡터
            Vector3 toOrigin = (_originPosition - transform.position).normalized;

            // 원점 방향으로 이동
            transform.Translate(toOrigin * (_speed * DashSpeedMultiplier) * deltaTime);

            // 원점에 거의 도달했으면 정확히 고정
            if (Vector3.Distance(transform.position, _originPosition) < 0.1f)
            {
                transform.position = _originPosition;
            }
        }
    }

    // 아이템 등으로 속도 버프
    public void Boost(float amount)
    {
        _speed += amount;
        Debug.Log("아이템을 먹었다!");
    }
}
