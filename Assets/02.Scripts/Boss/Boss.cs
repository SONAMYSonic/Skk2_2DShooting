using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("스탯")]
    public float Speed;
    public float ColliderDamage = 9999f;                   // 플레이어와 충돌 시 입히는 대미지
    private float _bossCurrentHealth = 3500f;
    [SerializeField]
    private float _initialHealth = 3500f;

    [Header("점수")]
    [SerializeField]
    private int _bossScore = 1000;

    [Header("이펙트")]
    public GameObject ExplosionPrefab;

    private Animator _bossAnimator;

    private void Awake()
    {
        _bossAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // 체력 초기화
        _bossCurrentHealth = _initialHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;

        collision.GetComponent<PlayerStats>().Hit(ColliderDamage);
    }

    public void Hit(float damage)
    {
        // 플레이어 총알 대미지 만큼 체력 감소.
        _bossCurrentHealth -= damage;
        // 히트 애니메이션 재생.
        _bossAnimator.SetTrigger("BossHit");
        if (_bossCurrentHealth <= 0)
        {
            BossDead();
        }
    }

    private void MakeExplosionEffect()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
    }

    public void BossDead()
    {
        MakeExplosionEffect();
        AudioManager.AudioManagerInstance.PlaySound(AudioManager.EAudioType.EnemyDeath);
        ScoreManager.Instance.AddScore(_bossScore);
        gameObject.SetActive(false);
    }
}
