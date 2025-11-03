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
    public float Speed = 5f; // 이동 속도
    public float MaxSpeed = 20f; // 최대 이동 속도
    public float MinSpeed = 1f;  // 최소 이동 속도
    public float DashSpeedMultiplier = 1f; // 대시 시 속도 배율

    // 이동 제한 범위
    [Header("이동범위")]
    public float MinX = -8.5f; // 좌측 경계
    public float MaxX = 8.5f;  // 우측 경계
    public float MinY = -4.5f; // 하단 경계
    public float MaxY = 4.5f;  // 상단 경계

    [Header("기타")]
    public bool isDash = false; // 대시 상태 여부
    public Vector3 originPosition = new Vector3(0f, -3f, 0f); // 원점 위치
    public bool isRecording = false; // 리플레이 녹화 상태
    public bool isReplaying = false; // 리플레이 재생 상태

    void Update()
    {
        // 1. 스피드 조작 (Q: 스피드 업, E: 스피드 다운)
        if (Input.GetKey(KeyCode.Q))
        {
            Speed += Time.deltaTime * 3f; // 부드럽게 속도 증가
        }
        if (Input.GetKey(KeyCode.E))
        {
            Speed -= Time.deltaTime * 3f; // 부드럽게 속도 감소
        }
        // Speed를 MinSpeed와 MaxSpeed 사이로 제한
        Speed = Mathf.Clamp(Speed, MinSpeed, MaxSpeed);

        // 1-1. Shift 키를 누르고 있으면 속도가 1.2배가 된다.
        if (Input.GetKeyDown(KeyCode.LeftShift) && isDash == false)
        {
            // 대시 상태로 변경
            isDash = true;
            DashSpeedMultiplier = 2f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && isDash == true)
        {
            // 대시 상태 해제
            isDash = false;
            DashSpeedMultiplier = 1f;
        }

        // 2. 키보드 입력을 감지한다.
        // 벡터: 크기와 방향을 표현하는 물리 개념
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 3. 입력으로부터 방향을 구하고 정규화한다.
        //    normalized를 사용하여 벡터의 크기를 1로 만들어 대각선 이동 시에도 속도가 일정하게 유지됩니다.
        Vector2 direction = new Vector2(h, v).normalized;

        // 오른쪽  (1,0)
        // 위쪽  (0,1)
        // 대각선위오른쪽 (1,1)

        // 3-1. 방향을 크기 1로 만드는 정규화를 한다.
        // direction.Normalize();
        // direction = direction.normalized; // 위의 한줄과 동일한 기능

        //transform.Translate((Vector3)direction * Speed * Time.deltaTime); // 방향 * 속도 * 시간

        // 4. 새로운 위치를 계산한다.
        Vector3 newPosition = transform.position + (Vector3)direction * (Speed * DashSpeedMultiplier) * Time.deltaTime;

        // 5. 새로운 위치를 제한된 영역 내로 보정한다.
        newPosition.x = Mathf.Clamp(newPosition.x, MinX, MaxX);
        newPosition.y = Mathf.Clamp(newPosition.y, MinY, MaxY);

        // 6. 보정된 위치로 이동시킨다.
        transform.position = newPosition;

        // 7. 화면 끝으로 이동 시 반대편에서 나타나게 한다.
        if (transform.position.x <= MinX)
        {
            transform.position = new Vector3(MaxX, transform.position.y, transform.position.z);
        }
        else if (transform.position.x >= MaxX)
        {
            transform.position = new Vector3(MinX, transform.position.y, transform.position.z);
        }

        // 8. R 키를 누르면 플레이어가 자동으로 원점(0,-3,0)으로 점점 초기화한다.
        // 조건: Translate를 사용하여 구현
        if (Input.GetKey(KeyCode.R))
        {
            // 원점 방향 벡터 계산
            // 원점 방향    =       원점 위치 - 현재 위치.정규화
            Vector3 toOrigin = (originPosition - transform.position).normalized;

            // 원점으로 이동
            transform.Translate(toOrigin * (Speed * DashSpeedMultiplier) * Time.deltaTime);

            // 도착했을 때 위치 보정
            if (Vector3.Distance(transform.position, originPosition) < 0.1f)
            {
                // 만약 원점에 거의 도달했으면,
                transform.position = originPosition;        // 정확히 원점으로 위치 보정
            }
        }

        // 9. 리플레이 기능 구현
        // 사용자의 입력을 녹화하고 재생하는 기능
        if (Input.GetKeyDown(KeyCode.T))
        {
            isRecording = !isRecording; // 녹화 상태 토글
            if (isRecording)
            {
                Debug.Log("리플레이 녹화 시작");
                // 녹화 시작 시 초기화 작업 수행
            }
            else
            {
                Debug.Log("리플레이 녹화 종료");
                // 녹화 종료 시 저장 작업 수행
            }
        }
    }
}
