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
        _enemy = GetComponent<Enemy>();
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

        // 적이 플레이어를 바라보도록 회전
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Atan2: y,x 좌표를 받아서 각도를 라디안으로 반환
        // Rad2Deg: 라디안을 도 단위로 변환
        // Atan2는 라디안 단위이므로 Deg(Degree, 도)로 변환 필요

        // 회전 적용
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}
