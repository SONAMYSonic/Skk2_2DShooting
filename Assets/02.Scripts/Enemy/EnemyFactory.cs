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

    public GameObject SpawnEnemy(int enemyType, Vector3 EnemySpawnPosition)
    {
        // 적 타입에 따라 해당 풀에서 비활성화된 적 오브젝트를 찾아 반환
        GameObject[] selectedPool = enemyType switch
        {
            (int)EEnemyType.Enemy_BrakeAcc => _brakeAcc,
            (int)EEnemyType.Enemy_Straight => _straight,
            (int)EEnemyType.Enemy_Trace => _trace,
            (int)EEnemyType.Enemy_Zigzag => _zigzag,
            _ => null
        };

        // 선택된 풀이 null인 경우 null 반환
        if (selectedPool == null)
            return null;

        foreach (var enemy in selectedPool)
        {
            // 비활성화된 적 오브젝트를 찾아 반환
            if (!enemy.activeInHierarchy)
            {
                enemy.transform.position = EnemySpawnPosition; // 스폰 위치 설정
                enemy.SetActive(true); // 적 활성화

                return enemy;
            }
        }
        return null; // 모든 적이 사용 중인 경우 null 반환
    }
}
