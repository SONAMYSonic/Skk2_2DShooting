using System.Collections.Generic;
using UnityEngine;

// 플레이어 이동
public class PlayerRecord : MonoBehaviour
{
    // 목표
    // 1. 키보드 입력에 따라 방향을 구하고 그 방향으로 이동시키고 싶다
    // 2. Q/E 키로 스피드를 조절하고 싶다.
    // 3. 특정 영역을 벗어나지 못하게 하고 싶다.
    // 4. 입력 기반 리플레이(녹화/재생)를 구현한다.

    // 필요 속성
    [Header("능력치")]
    public float Speed = 3f; // 이동 속도
    public float MaxSpeed = 20f; // 최대 이동 속도
    public float MinSpeed = 1f;  // 최소 이동 속도
    public float DashSpeedMultiplier = 1f; // 대시 시 속도 배율

    // 이동 제한 범위
    [Header("이동범위")]
    public float MinX = -3f; // 좌측 경계
    public float MaxX = 3f;  // 우측 경계
    public float MinY = -4.5f; // 하단 경계
    public float MaxY = 1f;  // 상단 경계

    [Header("기타")]
    public bool isDash = false; // 대시 상태 여부

    [Header("시작 위치")]
    private Vector3 _originPosition = new Vector3(0f, -3f, 0f); // 원점 위치

    [Header("리플레이")]
    // 리플레이 상태
    public bool isRecording = false; // 리플레이 녹화 상태
    public bool isReplaying = false; // 리플레이 재생 상태

    [Header("총알")]
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform bulletSpawnPoint; // 총알 발사 위치

    // 입력 프레임 데이터
    [System.Serializable]
    private struct InputFrame
    {
        public float deltaTime;     // 프레임 간 시간
        public float h;             // 수평 입력
        public float v;             // 수직 입력
        public bool qHeld;          // Q 키 상태
        public bool eHeld;          // E 키 상태
        public bool shiftHeld;      // Shift 키 상태
        public bool rHeld;          // R 키 상태
    }

    private readonly List<InputFrame> recordedFrames = new List<InputFrame>(4096);  
    // 녹화된 입력 프레임 버퍼, 4096프레임인 이유는 약 68초 분량
    
    private int replayIndex = 0;    // 리플레이 중 현재 프레임 인덱스

    // 녹화 시작 시 초기 상태 스냅샷(리플레이 재현을 위해)
    private Vector3 recordedStartPosition;      // 녹화 시작 위치
    private float recordedStartSpeed;           // 녹화 시작 속도
    private bool recordedStartDash;             // 녹화 시작 대시 상태
    private bool prevReplayShiftHeld = false;   // 리플레이 중 Shift 키 이전 상태

    void Update()
    {
        // 리플레이 토글
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (isReplaying) StopReplay();
            else StartReplay();
        }

        // 녹화 토글
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (isRecording) StopRecording();
            else StartRecording();
        }

        // 리플레이 중이면, 실제 입력을 무시하고 기록된 프레임을 재생
        if (isReplaying)
        {
            ReplayFrame();
            return;
        }

        // ==== 실제 입력 처리(녹화 아님) ====

        // 1. 스피드 조작 (Q: 스피드 업, E: 스피드 다운)
        bool qHeld = Input.GetKey(KeyCode.Q);
        bool eHeld = Input.GetKey(KeyCode.E);

        if (qHeld) Speed += Time.deltaTime * 10f; // 부드럽게 속도 증가
        if (eHeld) Speed -= Time.deltaTime * 10f; // 부드럽게 속도 감소

        // Speed를 MinSpeed와 MaxSpeed 사이로 제한
        Speed = Mathf.Clamp(Speed, MinSpeed, MaxSpeed);

        // 1-1. Shift 키를 누르고 있으면 대시 상태 토글
        if (Input.GetKeyDown(KeyCode.LeftShift) && isDash == false)
        {
            isDash = true;
            DashSpeedMultiplier = 2f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && isDash == true)
        {
            isDash = false;
            DashSpeedMultiplier = 1f;
        }

        // 2. 키보드 입력을 감지한다.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift);
        bool rHeld = Input.GetKey(KeyCode.R);

        // 3. 입력으로부터 방향을 구하고 정규화한다.
        Vector2 direction = new Vector2(h, v).normalized;

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
        if (rHeld)
        {
            // 원점 방향 벡터 계산
            // 원점 방향    =       원점 위치 - 현재 위치.정규화
            Vector3 toOrigin = (_originPosition - transform.position).normalized;

            // 원점 방향으로 이동
            transform.Translate(toOrigin * (Speed * DashSpeedMultiplier) * Time.deltaTime);

            // 원점에 거의 도달했으면 정확히 고정
            if (Vector3.Distance(transform.position, _originPosition) < 0.1f)
            {
                transform.position = _originPosition;
            }
        }

        // 9. 리플레이 녹화: 현재 프레임의 입력 상태와 시간 기록
        if (isRecording)    // 녹화 중이면,
        {
            // 현재 프레임 입력 상태를 리스트에 추가하면서 기록
            recordedFrames.Add(new InputFrame
            {
                deltaTime = Time.deltaTime, // 프레임 간 시간
                h = h,
                v = v,
                qHeld = qHeld,
                eHeld = eHeld,
                shiftHeld = shiftHeld,
                rHeld = rHeld
            });
        }
    }

    // 리플레이 한 프레임 재생
    private void ReplayFrame()
    {
        if (replayIndex >= recordedFrames.Count)
        {
            StopReplay();
            return;
        }

        var frame = recordedFrames[replayIndex];

        // 스피드 조작(Q/E)
        if (frame.qHeld) Speed += frame.deltaTime * 3f;
        if (frame.eHeld) Speed -= frame.deltaTime * 3f;
        Speed = Mathf.Clamp(Speed, MinSpeed, MaxSpeed);

        // Shift 대시 토글(에지 검출)
        if (frame.shiftHeld && !prevReplayShiftHeld)
        {
            isDash = true;
            DashSpeedMultiplier = 2f;
        }
        else if (!frame.shiftHeld && prevReplayShiftHeld)
        {
            isDash = false;
            DashSpeedMultiplier = 1f;
        }
        prevReplayShiftHeld = frame.shiftHeld;

        // 이동
        Vector2 direction = new Vector2(frame.h, frame.v).normalized;
        Vector3 newPosition = transform.position + (Vector3)direction * (Speed * DashSpeedMultiplier) * frame.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, MinX, MaxX);
        newPosition.y = Mathf.Clamp(newPosition.y, MinY, MaxY);
        transform.position = newPosition;

        // 화면 좌우 래핑
        if (transform.position.x <= MinX)
        {
            transform.position = new Vector3(MaxX, transform.position.y, transform.position.z);
        }
        else if (transform.position.x >= MaxX)
        {
            transform.position = new Vector3(MinX, transform.position.y, transform.position.z);
        }

        // 원점 복귀(R)
        if (frame.rHeld)
        {
            Vector3 toOrigin = (_originPosition - transform.position).normalized;
            transform.Translate(toOrigin * (Speed * DashSpeedMultiplier) * frame.deltaTime);

            if (Vector3.Distance(transform.position, _originPosition) < 0.1f)
            {
                transform.position = _originPosition;
            }
        }

        replayIndex++;
    }

    private void StartRecording()
    {
        isRecording = true;
        isReplaying = false;

        recordedFrames.Clear();
        replayIndex = 0;

        recordedStartPosition = transform.position;
        recordedStartSpeed = Speed;
        recordedStartDash = isDash;
        prevReplayShiftHeld = recordedStartDash;

        Debug.Log($"리플레이 녹화 시작 (프레임 버퍼 초기화)");
    }

    private void StopRecording()
    {
        isRecording = false;
        Debug.Log($"리플레이 녹화 종료 (기록 프레임 수: {recordedFrames.Count})");
    }

    private void StartReplay()
    {
        if (recordedFrames.Count == 0)
        {
            Debug.LogWarning("리플레이 데이터가 없습니다. 먼저 녹화를 시작하세요(T).");
            return;
        }

        isReplaying = true;
        isRecording = false;
        replayIndex = 0;

        // 녹화 시작 시점 상태로 복원 후 재생
        transform.position = recordedStartPosition;
        Speed = recordedStartSpeed;
        isDash = recordedStartDash;
        DashSpeedMultiplier = isDash ? 2f : 1f;
        prevReplayShiftHeld = recordedStartDash;

        Debug.Log("리플레이 재생 시작");
    }

    private void StopReplay()
    {
        isReplaying = false;
        Debug.Log("리플레이 재생 종료");
    }
}