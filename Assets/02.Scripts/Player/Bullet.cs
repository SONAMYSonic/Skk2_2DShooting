using UnityEngine;


public class Bullet : MonoBehaviour
{
    [Header("총알 속도")]
    public float Speed = 1f;
    public float MaxSpeed = 7f;        // 최대 속도
    public float ZeroToMaxSpeed = 1.2f;    // 최대 속도에 도달하는 시간
    public float InitialSpeed = 1f;    // 초기 속도

    [Header("총알 모드")]
    public bool is_S_mode = false; // S 모드 여부
    public bool is_Spiral_mode = false; // 나선형 모드 여부

    [Header("총알 경로 설정")]
    public float Frequency = 5f;            // S자의 진동수
    public float Magnitude = 0.3f;          // S자의 폭 (진폭)
    public float SpiralRadius = 0.5f;       // 원의 크기
    public float SpiralAngularSpeed = 5f;   // 얼마나 빨리 도는지 (원의 각속도)

    [Header("총알 속성")]
    public bool is_SubBullet = false; // 서브 총알 여부
    public float MissDamageMultiplier = 0.5f; // 빗맞은 총알 데미지 배율

    [Header("총알 데미지")]
    public float Damage_Main = 60f; // 메인 총알 데미지
    public float Damage_Sub = 40f;  // 서브 총알 데미지

    [SerializeField]
    Vector3 _basePos;                // 직선 기준 위치
    float _timer;                        // 경과 시간

    private void OnEnable()
    {
        BulletInitialize();
    }

    void Update()
    {
        // 시간 누적
        _timer += Time.deltaTime;

        // 속도 증가 초기 총알 속도(speed) 1에서 최대 속도(maxSpeed) 7까지 1.2초(zeroToMaxSpeed) 동안 증가
        Speed += (MaxSpeed - InitialSpeed) / ZeroToMaxSpeed * Time.deltaTime;
        Speed = Mathf.Min(Speed, MaxSpeed); // 속도가 최대 속도를 넘지 않도록 제한

        _basePos += transform.up * Speed * Time.deltaTime;       // 직선 이동

        if (is_S_mode)
        {
            _sMode();      // S자 모드
            return;
        }
        else if (is_Spiral_mode)
        {
            _spiralMode(); // 나선형 모드
            return;
        }
        else
        {
            transform.position = _basePos; // 직선 이동
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 총알은 Enemy와만 충돌 이벤트를 처리한다.
        if ((collision.CompareTag("Enemy") || collision.CompareTag("EnemySide") || collision.CompareTag("Boss")) == false)
            return;

        if (collision.CompareTag("Enemy") == true)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (is_SubBullet == false)
                enemy.GetComponent<Enemy>().Hit(Damage_Main);
            else
                enemy.GetComponent<Enemy>().Hit(Damage_Sub);
        }
        else if (collision.CompareTag("Boss"))
        {
            Boss boss = collision.gameObject.GetComponent<Boss>();
            if (is_SubBullet == false)
                boss.GetComponent<Boss>().Hit(Damage_Main);
            else
                boss.GetComponent<Boss>().Hit(Damage_Sub);
        }

        if (collision.CompareTag("EnemySide") == true)
        {
            Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
            Debug.Log("빗맞았다!");
            if (is_SubBullet == false)  // 빗맞은 메인 총알
                enemy.GetComponent<Enemy>().Hit(Damage_Main * MissDamageMultiplier); // 적 체력 메인
            else  // 빗맞은 서브 총알
                enemy.GetComponent<Enemy>().Hit(Damage_Sub * MissDamageMultiplier); // 적 체력 서브
        }

        gameObject.SetActive(false); // 총알 비활성화
    }

    private void _sMode()
    {
        float offsetX = Mathf.Sin(_timer * Frequency) * Magnitude;           // S자 궤적 계산
        transform.position = _basePos + new Vector3(offsetX, 0f, 0f);    // 기준 위치 + X축 오프셋
    }

    private void _spiralMode()
    {
        // 원형(소용돌이) 오프셋
        float angle = _timer * SpiralAngularSpeed;   // 시간에 따라 각도 증가
        float offsetX = Mathf.Cos(angle) * SpiralRadius;
        float offsetY = Mathf.Sin(angle) * SpiralRadius;

        transform.position = _basePos + new Vector3(offsetX, offsetY, 0f); // 기준 위치 + 원형 오프셋
    }

    private void BulletInitialize()
    {
        Speed = InitialSpeed; // 초기 속도로 설정
        _basePos = transform.position;   // 발사 시작 위치
    }
}
