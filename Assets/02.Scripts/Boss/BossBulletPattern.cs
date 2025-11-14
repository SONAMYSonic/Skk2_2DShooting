using UnityEngine;

public class BossBulletPattern : MonoBehaviour
{
    public enum BulletPatternType
    {
        StraightFourWay,
        Shotgun,
        Fan,
        Circle,
        Star
    }
    public BulletPatternType type;

    #region References

    [Header("참조")]
    [SerializeField] private BulletFactory _bulletFactory;
    [SerializeField] private Transform _fireOrigin; // 총알 발사 기준 위치 (없으면 자기 transform 사용)
    private Transform FireOrigin => _fireOrigin != null ? _fireOrigin : transform;

    private void Awake()
    {
        if (_bulletFactory == null)
        {
            _bulletFactory = BulletFactory.Instance;
        }
    }

    #endregion

    #region Straight 5-Way

    [Header("정방향 5발 발사 설정")]
    [SerializeField] private float _straightBulletSpeed = 6f;
    [SerializeField] private float[] _fiveWayOffsets = { -0.5f, -0.25f, 0f, 0.25f, 0.5f }; // X 오프셋 배열

    /// <summary>
    /// 아래 방향으로 5발(X 오프셋 적용) 발사
    /// </summary>
    public void FireStraightFiveWay()
    {
        Vector2 dir = Vector2.down;
        Vector3 origin = FireOrigin.position;

        for (int i = 0; i < _fiveWayOffsets.Length; i++)
        {
            Vector3 spawnPos = origin + new Vector3(_fiveWayOffsets[i], 0f, 0f);
            FireOneAt(spawnPos, dir, _straightBulletSpeed);
        }
    }

    #endregion

    #region Shotgun / Fan

    [Header("샷건(방사형) 발사 설정")]
    [SerializeField] private int _shotgunBulletCount = 8;
    [SerializeField] private float _shotgunBaseAngleDegree = 270f;
    [SerializeField] private float _shotgunSpreadAngleDegree = 45f;
    [SerializeField] private float _shotgunBulletSpeed = 7f;

    [Header("부채꼴 발사 설정")]
    [SerializeField] private int _fanBulletCount = 10;
    [SerializeField] private float _fanBaseAngleDegree = 270f;
    [SerializeField] private float _fanSpreadAngleDegree = 60f;
    [SerializeField] private float _fanBulletSpeed = 7f;

    public void FireShotgun()
    {
        FireShotgunInternal(_shotgunBulletCount, _shotgunBaseAngleDegree, _shotgunSpreadAngleDegree, _shotgunBulletSpeed);
    }

    public void FireFan()
    {
        FireShotgunInternal(_fanBulletCount, _fanBaseAngleDegree, _fanSpreadAngleDegree, _fanBulletSpeed);
    }

    private void FireShotgunInternal(int bulletCount, float baseAngleDegree, float spreadAngleDegree, float bulletSpeed)
    {
        if (bulletCount <= 0) return;

        if (bulletCount == 1)
        {
            FireOne(ConvertAngleToDirection(baseAngleDegree), bulletSpeed);
            return;
        }

        float startAngle = baseAngleDegree - spreadAngleDegree * 0.5f;
        float angleStep = spreadAngleDegree / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngleDegree = startAngle + angleStep * i;
            FireOne(ConvertAngleToDirection(currentAngleDegree), bulletSpeed);
        }
    }

    #endregion

    #region Circle

    [Header("원형 발사 설정")]
    [SerializeField] private int _circleBulletCount = 24;
    [SerializeField] private float _circleBulletSpeed = 5f;

    public void FireCircle()
    {
        FireCircleInternal(_circleBulletCount, _circleBulletSpeed);
    }

    private void FireCircleInternal(int bulletCount, float bulletSpeed)
    {
        if (bulletCount <= 0) return;

        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngleDegree = angleStep * i;
            FireOne(ConvertAngleToDirection(currentAngleDegree), bulletSpeed);
        }
    }

    #endregion

    #region Star

    [Header("별(*) 모양 발사 설정")]
    [SerializeField] private int _starBranchCount = 5;
    [SerializeField] private float _starInnerBulletSpeed = 4f;
    [SerializeField] private float _starOuterBulletSpeed = 8f;

    public void FireStar()
    {
        FireStarInternal(_starBranchCount, _starInnerBulletSpeed, _starOuterBulletSpeed);
    }

    private void FireStarInternal(int branchCount, float innerBulletSpeed, float outerBulletSpeed)
    {
        if (branchCount <= 0) return;

        float angleStep = 360f / branchCount;

        for (int i = 0; i < branchCount; i++)
        {
            float currentAngleDegree = angleStep * i;
            Vector2 direction = ConvertAngleToDirection(currentAngleDegree);

            FireOne(direction, innerBulletSpeed);
            FireOne(direction, outerBulletSpeed);
        }
    }

    #endregion

    #region Core Helpers

    private void FireOne(Vector2 direction, float bulletSpeed)
    {
        if (_bulletFactory == null)
        {
            Debug.LogWarning("BossBulletPattern: BulletFactory 참조가 없습니다.");
            return;
        }

        GameObject bulletObject = _bulletFactory.MakeBossBullet(FireOrigin.position);
        if (bulletObject == null) return;

        if (!bulletObject.TryGetComponent(out BossBullet bossBullet))
        {
            Debug.LogError("BossBulletPattern: BossBullet 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        bossBullet.BossBulletInitialize(direction, bulletSpeed);
    }

    private void FireOneAt(Vector3 spawnPosition, Vector2 direction, float bulletSpeed)
    {
        if (_bulletFactory == null)
        {
            Debug.LogWarning("BossBulletPattern: BulletFactory 참조가 없습니다.");
            return;
        }

        GameObject bulletObject = _bulletFactory.MakeBossBullet(spawnPosition);
        if (bulletObject == null) return;

        if (!bulletObject.TryGetComponent(out BossBullet bossBullet))
        {
            Debug.LogError("BossBulletPattern: BossBullet 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        bossBullet.BossBulletInitialize(direction, bulletSpeed);
    }

    private static Vector2 ConvertAngleToDirection(float angleDegree)
    {
        float angleRadians = angleDegree * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));
    }

    #endregion
}
