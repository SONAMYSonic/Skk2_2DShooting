using UnityEngine;
using static EnemyMove;
using Random = UnityEngine.Random;  // 다른 랜덤 키워드와 충돌 방지를 위한 명시

public class EnemySpawner : MonoBehaviour
{
    // 과제 3. 일정 시간마다 자신의 위치에 적을 생성

    [Header("적 프리팹")]
    public GameObject[] EnemyPrefabs;

    [Header("생성 주기 (Float)")]
    public float SpawnInterval = 2f;
    public float RandomRangeMin = 1f;
    public float RandomRangeMax = 3f;
    private float _timer;

    [Header("디버그")]
    public bool IsDebugMode = false;

    [Header("디버그: 생성할 적 움직임 방식")]
    public GameObject DebugEnemyPrefab;

    private void Start()
    {
        // 쿨타임을 RandomRangeMin과 RandomRangeMax 사이로 랜덤하게 저장한다.
        float randomCoolTime = UnityEngine.Random.Range(RandomRangeMin, RandomRangeMax);
        SpawnInterval = randomCoolTime;

    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (IsDebugMode && _timer >= SpawnInterval)
        {
            DebugSpawn();
            return;
        }

        if (_timer >= SpawnInterval)
        {
            SpawnEnemy();
        }
    }

    // 30% 확률로 추적 적 생성, 70% 확률로 직진 적 생성
    private void SpawnEnemy()
    {
        float randomValue = Random.Range(0f, 1f);
        // 30% 확률(0.3f 보다 작을 때)로 추적 적 생성, 70% 확률로 직진 적 생성
        //GameObject enemyToSpawn = randomValue < 0.3f ? EnemyPrefabs[(int)EEnemyType.Following] : EnemyPrefabs[(int)EEnemyType.Straight];
        // 그냥 랜덤하게 적 생성
        GameObject enemyToSpawn = EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)];
        Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        _timer = 0f;    // 타이머 초기화
        // 다음 스폰 간격을 다시 랜덤하게 설정
        SpawnInterval = UnityEngine.Random.Range(RandomRangeMin, RandomRangeMax);
    }

    private void DebugSpawn()
    {
        Instantiate(DebugEnemyPrefab, transform.position, Quaternion.identity);
        _timer = 0f;
        SpawnInterval = UnityEngine.Random.Range(RandomRangeMin, RandomRangeMax);
    }
}
