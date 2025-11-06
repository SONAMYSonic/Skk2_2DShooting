using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [Header("플레이어")]
    public GameObject Player;

    [Header("본인 능력치")]
    private Enemy _enemy;

    [Header("움직임 방식")]
    public bool IsFollowingPlayer = false;

    private void Start()
    {
        // 플레이어 오브젝트 찾기
        Player = GameObject.FindWithTag("Player");
        _enemy = gameObject.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFollowingPlayer == true)
            FollowingPlayer();
        else
            GoStraight();
    }

    public void FollowingPlayer()
    {
        // 플레이어 위치 추적
        Vector3 direction = (Player.transform.position - transform.position).normalized;
        transform.position += direction * _enemy.Speed * Time.deltaTime;
    }

    public void GoStraight()
    {
        Vector2 direction = Vector2.down;
        transform.Translate(direction * _enemy.Speed * Time.deltaTime);
    }
}
