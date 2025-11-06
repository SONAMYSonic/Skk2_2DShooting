using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("스탯")]
    public float Speed;
    public float Damage = 1;
    private float _health = 100f;

    [Header("플레이어")]
    public GameObject Player;

    [Header("상태")]
    public bool IsFollowingPlayer = false;

    private void Start()
    {
        // 플레이어 오브젝트 찾기
        Player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (IsFollowingPlayer == true)
            FollowingPlayer();
        else
            GoStraight();

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

    public void FollowingPlayer()
    {
        // 플레이어 위치 추적
        Vector3 direction = (Player.transform.position - transform.position).normalized;
        transform.position += direction * Speed * Time.deltaTime;
    }

    public void GoStraight()
    {
        Vector2 direction = Vector2.down;
        transform.Translate(direction * Speed * Time.deltaTime);
    }
}
