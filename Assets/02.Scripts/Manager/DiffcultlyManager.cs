using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    private static DifficultyManager _instance;
    public static DifficultyManager Instance => _instance;

    [Header("적 체력 난이도 커브")]
    [Tooltip("x = (현재점수 / MaxReferenceScore) 0~1 정규화, y = 체력 배율")]
    public AnimationCurve EnemyHealthCurve = AnimationCurve.Linear(0f, 1f, 1f, 3f);
    [Tooltip("점수 정규화 기준 (이 점수 이상이면 커브 x=1로 고정)")]
    public int MaxReferenceScore = 10000;

    [Header("배율 클램프")]
    public float MinHealthMultiplier = 1f;
    public float MaxHealthMultiplier = 5f;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    public float GetEnemyHealthMultiplier(int currentScore)
    {
        if (MaxReferenceScore <= 0)
            return 1f;

        float t = Mathf.Clamp01(currentScore / (float)MaxReferenceScore);
        float curveValue = EnemyHealthCurve.Evaluate(t);
        return Mathf.Clamp(curveValue, MinHealthMultiplier, MaxHealthMultiplier);
    }
}