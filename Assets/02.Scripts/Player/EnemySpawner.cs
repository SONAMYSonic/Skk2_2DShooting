using UnityEngine;
using Random = UnityEngine.Random;  // 다른 랜덤 키워드와 충돌 방지를 위한 명시

public class EnemySpawner : MonoBehaviour
{
    // 과제 3. 일정 시간마다 자신의 위치에 적을 생성

    [Header("적 프리팹")]
    public GameObject EnemyPrefab;

    [Header("생성 주기")]
    public float SpawnInterval = 2f;
    private float _timer;

    private void Start()
    {
        // 쿨타임을 1과 2 사이로 랜덤하게 저장한다.
        float randomCoolTime = UnityEngine.Random.Range(1f, 3f);
        SpawnInterval = randomCoolTime;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= SpawnInterval)
        {
            SpawnEnemy();
            _timer = 0f;
            // 다음 스폰 간격을 다시 랜덤하게 설정
            SpawnInterval = UnityEngine.Random.Range(1f, 3f);
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
    }
}
