using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("스탯")]
    public float Speed;
    public float Damage = 1;
    public float _health = 100f;

    void Update()
    {
        Vector2 direction = Vector2.down;
        transform.Translate(direction * Speed * Time.deltaTime);

        if (_health <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;

        collision.GetComponent<PlayerStats>().Hit(Damage);
        Debug.Log("으악!");
        Destroy(gameObject);
    }

    public void Hit(float damage)
    {
        _health -= damage;
        Debug.Log("남은 체력 " + _health);
    }
}
