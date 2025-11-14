using UnityEngine;

public class EnemyBrakeAcc : MonoBehaviour
{
    // 처음에 속도 줄였다가 다시 가속하는 움직임

    [Header("적 속성")]
    private float _currentSpeed = 2f;
    private float _timer;                        // 경과 시간
    private float _initialSpeed = 2f;        // 초기 속도

    [Header("적 가속 설정")]
    public float EnemyMaxSpeed = 20f;        // 최대 속도
    public float EnemyMaxSpeedDuration = 1f;    // 최대 속도에 도달하는 시간

    private float _accelerationMin = 0.5f; // 최소 최대 속도에 도달하는 시간
    private float _accelerationMax = 2f; // 최대 최대 속도에 도달하는 시간

    [Header("적 감속 설정")]
    public float EnemyBrakeDuration = 1f;    // 멈추는 데 걸리는 시간

    private float _enemyBrakeSpeed = 0.2f;        // 멈출 때 속도
    private float EnemyBrakeTiming = 0.7f;       // 스폰 후 브레이크 밟기 시작하는 시간

    private float _brakeDurationMin = 0.2f; // 최소 멈추는 데 걸리는 시간
    private float _brakeDurationMax = 1f; // 최대 멈춤 시간

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        EnemyBrakeDuration = Random.Range(_brakeDurationMin, _brakeDurationMax);  // 멈추는 데 걸리는 시간을 랜덤하게 설정
        EnemyMaxSpeedDuration = Random.Range(_accelerationMin, _accelerationMax); // 최대 속도에 도달하는 시간을 랜덤하게 설정
        _timer = 0f;
        _currentSpeed = _initialSpeed;
    }

    void Update()
    {
        // 시간 누적
        _timer += Time.deltaTime;

        // 스폰 후 이동
        transform.position += Vector3.down * _currentSpeed * Time.deltaTime;       // 직선 이동

        if (_timer > EnemyBrakeTiming) // 일정 시간 후 감속 시작
        {
            // 감속 구간
            float deceleration = (_currentSpeed - _enemyBrakeSpeed) / EnemyBrakeDuration; // 초당 감소해야 하는 속도량
            _currentSpeed -= Time.deltaTime * deceleration;
            _currentSpeed = Mathf.Max(_currentSpeed, _enemyBrakeSpeed); // 속도가 최소 속도보다 작아지지 않도록 제한
        }
        
        if (_timer > EnemyBrakeTiming + EnemyBrakeDuration)  // 감속이 끝난 후 가속 시작
        {
            // 가속 구간
            float acceleration = (EnemyMaxSpeed - _enemyBrakeSpeed) / EnemyMaxSpeedDuration; // 초당 증가해야 하는 속도량
            _currentSpeed += Time.deltaTime * acceleration;
            _currentSpeed = Mathf.Min(_currentSpeed, EnemyMaxSpeed); // 속도가 최대 속도를 넘지 않도록 제한
        }

    }
}
