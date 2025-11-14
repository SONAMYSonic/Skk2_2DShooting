using UnityEngine;

public class BossFactory : MonoBehaviour
{
    private static BossFactory _instance;
    public static BossFactory Instance => _instance;

    [Header("보스 오브젝트 (Hierarchy")]
    public GameObject BossObject;

    [Header("보스 프리팹")]
    public GameObject BossPrefab;

    [Header("스폰 조건")]
    public int SpawnScoreThreshold = 3000;
    private bool _spawned = false;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    // ScoreManager에서 직접 호출
    public void TrySpawnBoss(int currentScore)
    {
        if (_spawned) return;
        if (currentScore >= SpawnScoreThreshold)
        {
            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
        BossObject.transform.position = gameObject.transform.position;
        BossObject.SetActive(true);
        _spawned = true;
    }
}
