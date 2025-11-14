using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [Header("스탯")]
    public float Speed;
    public float Damage = 1f;
    private float _health = 100f;
    [SerializeField]
    private float _initialHealth = 100f;

    [Header("점수")]
    public int EnemyScore = 100;

    [Header("이펙트")]
    public GameObject ExplosionPrefab;

    private Animator _enemyAnimator;

    private void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // 체력 초기화
        _health = _initialHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;

        collision.GetComponent<PlayerStats>().Hit(Damage);
        EnemyDead();
    }

    public void Hit(float damage)
    {
        // 플레이어 총알 대미지 만큼 체력 감소.
        _health -= damage;
        // 히트 애니메이션 재생.
        _enemyAnimator.SetTrigger("Hit");
        if (_health <= 0)
        {
            EnemyDead();
        }
    }

    private void MakeExplosionEffect()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
    }

    public void EnemyDead()
    {
        MakeExplosionEffect();
        AudioManager.AudioManagerInstance.PlaySound(AudioManager.EAudioType.EnemyDeath);

        // 점수 관리자는 인스턴스가 단 하나다. 혹은 단 하나임을 보장해야 한다.
        // 아무 곳에서 빠르게 접근하고 싶다.
        // 싱글톤 패턴
        ScoreManager.Instance.AddScore(EnemyScore);
        // 아이템 드랍
        GetComponent<EnemyItemDrop>().ItemSpawn();
        gameObject.SetActive(false);
    }
}
