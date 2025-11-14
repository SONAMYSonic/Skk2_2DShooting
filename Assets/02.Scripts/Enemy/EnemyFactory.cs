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

        _brakeAcc = InitializePool(EnemyPrefabs[(int)EEnemyType.Enemy_BrakeAcc], PoolSize);
        _straight = InitializePool(EnemyPrefabs[(int)EEnemyType.Enemy_Straight], PoolSize);
        _trace = InitializePool(EnemyPrefabs[(int)EEnemyType.Enemy_Trace], PoolSize);
        _zigzag = InitializePool(EnemyPrefabs[(int)EEnemyType.Enemy_Zigzag], PoolSize);
    }

    private GameObject[] InitializePool(GameObject prefab, int size)
    {
        GameObject[] pool = new GameObject[size];
        for (int i = 0; i < size; i++)
        {
            GameObject enemyObject = Instantiate(prefab, transform);
            enemyObject.SetActive(false);
            pool[i] = enemyObject;
        }
        return pool;
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
