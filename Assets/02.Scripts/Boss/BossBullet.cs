using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private float _damage = 1f;
    private Vector2 _direction;

    public void BossBulletInitialize(Vector2 direction, float speed)
    {
        _direction = direction.normalized;
        _speed = speed;
    }

    private void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
        // 수명, 화면 밖 처리 등...
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;
        else
        {
            collision.GetComponent<PlayerStats>().Hit(_damage);
            gameObject.SetActive(false);
        }
    }
}
