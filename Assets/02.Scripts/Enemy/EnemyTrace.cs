using UnityEngine;

public class EnemyTrace : MonoBehaviour
{
    [Header("플레이어")]
    public GameObject Player;

    [Header("본인 능력치")]
    private Enemy _enemy;

    private void Start()
    {
        // 캐싱: 자주 쓰는 데이터를 미리 가까운 곳에 저장해두고 참조하는 것
        // 플레이어 오브젝트 찾기
        Player = GameObject.FindWithTag("Player");
        _enemy = gameObject.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowingPlayer();
    }

    public void FollowingPlayer()
    {
        // 플레이어 위치 추적
        Vector3 direction = (Player.transform.position - transform.position).normalized;
        transform.position += direction * _enemy.Speed * Time.deltaTime;
    }
}
