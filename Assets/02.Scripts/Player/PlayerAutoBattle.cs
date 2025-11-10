using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerFire))]

public class PlayerAutoBattle : MonoBehaviour
{
    [Header("자동 전투 모드")]
    public bool AutoMove = true;

    [Header("적 탐색 설정")]
    public string EnemyTag = "Enemy";           // 적 태그
    public float DetectionRange = 5f;          // 적 탐색 범위

    [Header("회피 설정")]
    public float EvadeDistance = 1f;            // 적이 이 거리 안으로 다가오면 회피
    public float MinMoveThreshold = 0.1f;        // 너무 가까울 때 떨림 방지

    private PlayerMove _playerMove;
    private PlayerFire _playerFire;

    [SerializeField]
    private float _lastEvadeDirectionX = 1f;    // 마지막 회피 방향 저장

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerFire = GetComponent<PlayerFire>();
    }

    private void Update()
    {
        // 자동 전투 모드 OFF 시 실행 안 함
        if (AutoMove == false) return;

        Transform target = FindNearestEnemy();
        if (target == null)
        {
            // 적이 없으면 정지
            _playerMove.ApplyInput(Time.deltaTime, 0f, 0f, false, false, false, false, false);
            // 순서대로 Q, E, Shift, UP, R
            return;
        }

        Vector3 directionToEnemy = target.position - transform.position;
        float distanceToEnemy = directionToEnemy.magnitude;

        float horizontalInput = 0f;
        float verticalInput = 0f;   // 세로 이동은 사용하지 않는다고 가정

        // 회피 거리 안으로 들어온 경우
        if (distanceToEnemy < EvadeDistance)
        {
            // 적과 거의 같은 X 위치면 → 이전에 선택했던 회피 방향을 유지
            if (Mathf.Abs(directionToEnemy.x) < MinMoveThreshold)
            {
                horizontalInput = _lastEvadeDirectionX;
            }
            else
            {
                // 적이 왼쪽에 있으면 오른쪽(+1), 오른쪽에 있으면 왼쪽(-1)으로 회피
                horizontalInput = -Mathf.Sign(directionToEnemy.x);
                _lastEvadeDirectionX = horizontalInput;
            }
        }
        else
        {
            // 추적 모드: 적을 향해 이동
            if (Mathf.Abs(directionToEnemy.x) < MinMoveThreshold)
            {
                horizontalInput = 0f;
            }
            else
            {
                horizontalInput = Mathf.Sign(directionToEnemy.x);
            }
        }

        // Q, E, Shift, UP, R 모두 사용 안 함
        _playerMove.ApplyInput(Time.deltaTime, horizontalInput, verticalInput, false, false, false, false, false);
    }

    // 가장 가까운 적 찾기
    private Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(EnemyTag);
        if (enemies.Length == 0) return null;
        // 적 없으면 null 반환

        Vector3 playerPosition = transform.position;    // 플레이어 위치
        float nearestEnemySqrDistance = DetectionRange * DetectionRange; // 탐색 범위 제곱
        Transform nearestEnemyTransform = null;

        foreach (GameObject enemyGameObject in enemies)
        {
            if (enemyGameObject.activeInHierarchy == false)
            {
                continue; // 비활성화된 적은 무시
            }

            Vector3 fromPlayerToEnemy = enemyGameObject.transform.position - playerPosition;
            float sqrDistanceToEnemy = fromPlayerToEnemy.sqrMagnitude;

            if (sqrDistanceToEnemy < nearestEnemySqrDistance)
            {
                nearestEnemySqrDistance = sqrDistanceToEnemy;
                nearestEnemyTransform = enemyGameObject.transform;
            }
        }

        return nearestEnemyTransform;
    }

    public void ToggleAutoMove(bool automove)
    {
        AutoMove = automove;
    }
}
