using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    private static EnemyFactory _instance = null;
    public static EnemyFactory Instance => _instance;


    [Header("적 프리팹")]
    public GameObject[] EnemyPrefabs;          // 적 프리팹
    // 0: 가속 적 1: 일반 적 2: 따라오는 적 3: 지그재그 적

    public enum EEnemyType
    {
        Enemy_BrakeAcc,
        Enemy_Straight,
        Enemy_Trace,
        Enemy_Zigzag
    }

    [Header("풀링")]
    public int PoolSize = 20;
    private GameObject[] _brakeAcc;      // 게임 총알을 담아둘 풀: 탄창
    private GameObject[] _straight;
    private GameObject[] _trace;
    private GameObject[] _zigzag;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        BrakeAccPoolInit();
        StraightPoolInit();
        TracePoolInit();
        ZigzagPoolInit();
    }
    private void BrakeAccPoolInit()
    {
        _brakeAcc = new GameObject[PoolSize];
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject enemyObject = Instantiate(EnemyPrefabs[0], transform);
            _brakeAcc[i] = enemyObject;
            enemyObject.SetActive(false);
        }

    }
    private void StraightPoolInit()
    {
        _straight = new GameObject[PoolSize];
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject enemyObject = Instantiate(EnemyPrefabs[1], transform);
            _straight[i] = enemyObject;
            enemyObject.SetActive(false);
        }
    }
    private void TracePoolInit()
    {
        _trace = new GameObject[PoolSize];
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject enemyObject = Instantiate(EnemyPrefabs[2], transform);
            _trace[i] = enemyObject;
            enemyObject.SetActive(false);
        }
    }
    private void ZigzagPoolInit()
    {
        _zigzag = new GameObject[PoolSize];
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject enemyObject = Instantiate(EnemyPrefabs[3], transform);
            _zigzag[i] = enemyObject;
            enemyObject.SetActive(false);
        }
    }

    // 개선된 SpawnEnemy: enum 파라미터, for 루프, transform 캐시
    public GameObject SpawnEnemy(EEnemyType enemyType, Vector3 EnemySpawnPosition)
    {
        GameObject[] selectedPool = enemyType switch
        {
            EEnemyType.Enemy_BrakeAcc => _brakeAcc,
            EEnemyType.Enemy_Straight => _straight,
            EEnemyType.Enemy_Trace => _trace,
            EEnemyType.Enemy_Zigzag => _zigzag,
            _ => null
        };

        if (selectedPool == null)
            return null;

        for (int i = 0; i < selectedPool.Length; i++)
        {
            var enemy = selectedPool[i];
            if (enemy == null) continue;

            if (!enemy.activeInHierarchy)
            {
                // transform 캐시
                Transform t = enemy.transform;
                t.position = EnemySpawnPosition;
                enemy.SetActive(true);
                return enemy;
            }
        }
        return null;
    }
}
