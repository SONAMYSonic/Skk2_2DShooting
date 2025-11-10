using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 입력 기반 녹화 / 리플레이만 담당하는 스크립트
/// (실제 이동은 PlayerMove에 위임)
/// </summary>
[RequireComponent(typeof(PlayerMove))]
public class PlayerRecord : MonoBehaviour
{
    [Header("리플레이 상태")]
    public bool isRecording = false;  // 리플레이 녹화 상태
    public bool isReplaying = false;  // 리플레이 재생 상태

    private PlayerMove _move;         // 같은 오브젝트에 붙어 있는 PlayerMove 참조

    [System.Serializable]
    private struct InputFrame
    {
        public float deltaTime;  // 프레임 간 시간
        public float h;          // 수평 입력
        public float v;          // 수직 입력
        public bool qHeld;       // Q 키 상태
        public bool eHeld;       // E 키 상태
        public bool shiftDown;   // Shift 키 다운
        public bool shiftUp;     // Shift 키 업
        public bool rHeld;       // R 키 상태
    }

    private readonly List<InputFrame> recordedFrames = new List<InputFrame>(4096); // 약 68초 분량
    private int replayIndex = 0;   // 리플레이 중 현재 프레임 인덱스

    // 녹화 시작 시 초기 상태 스냅샷
    private Vector3 recordedStartPosition; // 녹화 시작 위치
    private float recordedStartSpeed;      // 녹화 시작 속도
    private bool recordedStartDash;        // 녹화 시작 대시 상태

    private void Awake()
    {
        _move = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        // 리플레이 토글 (Y 키)
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (isReplaying) StopReplay();
            else StartReplay();
        }

        // 녹화 토글 (T 키)
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (isRecording) StopRecording();
            else StartRecording();
        }

        // 리플레이 중이면 기록된 입력을 사용
        if (isReplaying)
        {
            ReplayFrame();
            return;
        }

        // ===== 실제 입력 처리 =====

        // 1. 입력 값 읽기
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        bool qHeld = Input.GetKey(KeyCode.Q);
        bool eHeld = Input.GetKey(KeyCode.E);
        bool rHeld = Input.GetKey(KeyCode.R);
        bool shiftDown = Input.GetKeyDown(KeyCode.LeftShift);
        bool shiftUp = Input.GetKeyUp(KeyCode.LeftShift);

        // 2. 이동 로직은 PlayerMove에 위임
        _move.ApplyInput(Time.deltaTime, h, v, qHeld, eHeld, shiftDown, shiftUp, rHeld);

        // 3. 녹화 중이면 현재 프레임 입력 기록
        if (isRecording)
        {
            recordedFrames.Add(new InputFrame
            {
                deltaTime = Time.deltaTime,
                h = h,
                v = v,
                qHeld = qHeld,
                eHeld = eHeld,
                shiftDown = shiftDown,
                shiftUp = shiftUp,
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

        // PlayerMove에 "녹화 당시 입력"을 그대로 넣어줌
        _move.ApplyInput(
            frame.deltaTime,
            frame.h,
            frame.v,
            frame.qHeld,
            frame.eHeld,
            frame.shiftDown,
            frame.shiftUp,
            frame.rHeld
        );

        replayIndex++;
    }

    private void StartRecording()
    {
        isRecording = true;
        isReplaying = false;

        recordedFrames.Clear();
        replayIndex = 0;

        // 현재 상태 스냅샷 저장
        recordedStartPosition = transform.position;
        recordedStartSpeed = _move.Speed;
        recordedStartDash = _move.isDash;

        Debug.Log("리플레이 녹화 시작 (프레임 버퍼 초기화)");
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

        // 녹화 시작 시점 상태로 복원 후 재생 시작
        transform.position = recordedStartPosition;
        _move.Speed = recordedStartSpeed;
        _move.isDash = recordedStartDash;
        _move.DashSpeedMultiplier = recordedStartDash ? 2f : 1f;

        Debug.Log("리플레이 재생 시작");
    }

    private void StopReplay()
    {
        isReplaying = false;
        Debug.Log("리플레이 재생 종료");
    }
}