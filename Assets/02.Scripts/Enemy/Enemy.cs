using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("스탯")]
    public float Speed;
    public float Damage = 1f;
    private float _health = 100f;
    
    [Header("이펙트")]
    public GameObject ExplosionPrefab;

    private Animator _enemyAnimator;

    private void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
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
        Destroy(gameObject);
    }
}
