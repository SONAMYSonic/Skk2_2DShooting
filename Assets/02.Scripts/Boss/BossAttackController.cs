using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(BossBulletPattern))]
public class BossAttackController : MonoBehaviour
{
    // 패턴별 버스트/딜레이 설정
    [Serializable]
    public struct PatternBurstConfig
    {
        public BossBulletPattern.BulletPatternType Type;

        [Min(1)] public int ShotsPerBurst;          // 연속 발사 횟수
        [Min(0f)] public float DelayBetweenShots;   // 연속 발사 사이 간격

        [Min(1)] public int BurstRepeat;            // 버스트(묶음) 반복 횟수
        [Min(0f)] public float DelayBetweenBursts;  // 버스트 사이 간격
    }

    [Header("참조")]
    [SerializeField] private BossBulletPattern _bulletPattern;

    [Header("공격 주기 설정")]
    [SerializeField] private float _attackInterval = 1.5f;  // 패턴 시작 간격
    [SerializeField] private bool _autoAttack = true;       // 자동 공격

    [Header("패턴별 버스트 설정(인스펙터에서 조정 가능)")]
    [SerializeField] private PatternBurstConfig[] _configs; // Type-별 설정 목록

    // 런타임용 테이블(열거형 인덱싱)
    private PatternBurstConfig[] _configTable;

    private float _attackTimer;
    private bool _isBurstRunning;

    // 패턴 실행 테이블(매직넘버/스위치 최소화)
    private System.Action[] _patternActions;
    private int PatternCount => _patternActions?.Length ?? 0;

    private void Awake()
    {
        if (_bulletPattern == null)
            _bulletPattern = GetComponent<BossBulletPattern>();

        BuildPatternActions();
        BuildConfigTable(); // 기본값 채우고, 인스펙터 값으로 덮어쓰기

        _attackTimer = _attackInterval;
    }

    private void Update()
    {
        if (!_autoAttack || _isBurstRunning) return;

        _attackTimer -= Time.deltaTime;

        if (_attackTimer <= 0f)
        {
            int patternIndex = UnityEngine.Random.Range(0, PatternCount); // enum 개수 기반
            StartCoroutine(FirePatternBurst((BossBulletPattern.BulletPatternType)patternIndex));
        }
    }

    private IEnumerator FirePatternBurst(BossBulletPattern.BulletPatternType type)
    {
        _isBurstRunning = true;

        var cfg = _configTable[(int)type];

        for (int b = 0; b < cfg.BurstRepeat; b++)
        {
            for (int s = 0; s < cfg.ShotsPerBurst; s++)
            {
                _patternActions[(int)type].Invoke();

                if (s < cfg.ShotsPerBurst - 1 && cfg.DelayBetweenShots > 0f)
                    yield return new WaitForSeconds(cfg.DelayBetweenShots);
            }

            if (b < cfg.BurstRepeat - 1 && cfg.DelayBetweenBursts > 0f)
                yield return new WaitForSeconds(cfg.DelayBetweenBursts);
        }

        _isBurstRunning = false;
        _attackTimer = _attackInterval; // 다음 패턴까지 대기
    }

    public void SetAutoAttack(bool isOn) => _autoAttack = isOn;

    // 패턴 실행 함수 테이블 구성
    private void BuildPatternActions()
    {
        _patternActions = new System.Action[]
        {
            _bulletPattern.FireStraightFiveWay, // 0
            _bulletPattern.FireShotgun,         // 1
            _bulletPattern.FireFan,             // 2
            _bulletPattern.FireCircle,          // 3
            _bulletPattern.FireStar             // 4
        };
    }

    // 기본값(요구사항) + 인스펙터 설정 덮어쓰기
    private void BuildConfigTable()
    {
        int count = Enum.GetValues(typeof(BossBulletPattern.BulletPatternType)).Length;
        _configTable = new PatternBurstConfig[count];

        // 1) 기본값: 질문에서 요구한 동작으로 채움
        // - 그냥 발사(정방향 4발): 연속 4번
        _configTable[(int)BossBulletPattern.BulletPatternType.StraightFourWay] = new PatternBurstConfig
        {
            Type = BossBulletPattern.BulletPatternType.StraightFourWay,
            ShotsPerBurst = 4,
            DelayBetweenShots = 0.1f,
            BurstRepeat = 1,
            DelayBetweenBursts = 0f
        };

        // - 샷건: 3연속씩 3번
        _configTable[(int)BossBulletPattern.BulletPatternType.Shotgun] = new PatternBurstConfig
        {
            Type = BossBulletPattern.BulletPatternType.Shotgun,
            ShotsPerBurst = 3,
            DelayBetweenShots = 0.12f,
            BurstRepeat = 3,
            DelayBetweenBursts = 0.45f
        };

        // - 부채꼴: 5연속 1번
        _configTable[(int)BossBulletPattern.BulletPatternType.Fan] = new PatternBurstConfig
        {
            Type = BossBulletPattern.BulletPatternType.Fan,
            ShotsPerBurst = 5,
            DelayBetweenShots = 0.1f,
            BurstRepeat = 1,
            DelayBetweenBursts = 0f
        };

        // - 원형: 일정 간격으로 3번
        _configTable[(int)BossBulletPattern.BulletPatternType.Circle] = new PatternBurstConfig
        {
            Type = BossBulletPattern.BulletPatternType.Circle,
            ShotsPerBurst = 1,
            DelayBetweenShots = 0f,
            BurstRepeat = 3,
            DelayBetweenBursts = 0.5f
        };

        // - 별모양: 10번
        _configTable[(int)BossBulletPattern.BulletPatternType.Star] = new PatternBurstConfig
        {
            Type = BossBulletPattern.BulletPatternType.Star,
            ShotsPerBurst = 10,
            DelayBetweenShots = 0.08f,
            BurstRepeat = 1,
            DelayBetweenBursts = 0f
        };

        // 2) 인스펙터에서 설정한 값이 있으면 덮어쓰기
        if (_configs != null)
        {
            foreach (var c in _configs)
            {
                int idx = (int)c.Type;
                if (idx >= 0 && idx < count)
                    _configTable[idx] = c;
            }
        }
    }
}
