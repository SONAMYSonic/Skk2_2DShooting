using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("스탯")]
    public float Speed;
    public float Damage = 1;
    private float _health = 100f;

    private void Update()
    {
        if (_health <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;

        collision.GetComponent<PlayerStats>().Hit(Damage);
        Destroy(gameObject);
    }

    public void Hit(float damage)
    {
        _health -= damage;      // 플레이어 총알 대미지 만큼 체력 감소
    }

}
