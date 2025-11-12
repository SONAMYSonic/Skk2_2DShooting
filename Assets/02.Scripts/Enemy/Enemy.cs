using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [Header("스탯")]
    public float Speed;
    public float Damage = 1f;
    private float _health = 100f;

    [Header("점수")]
    public int EnemyScore = 100;

    [Header("이펙트")]
    public GameObject ExplosionPrefab;

    private Animator _enemyAnimator;
    private AudioManager _audioManager;

    private void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
        // tag가 AudioManager인 오브젝트를 찾아서 AudioManager 컴포넌트를 가져온다.
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
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
        _audioManager.PlaySound(AudioManager.EAudioType.EnemyDeath);

        ScoreManager scoreManager = FindAnyObjectByType<ScoreManager>();
        scoreManager.AddScore(EnemyScore);     // todo: 매직넘버 수정.


        Destroy(gameObject);
    }
}
