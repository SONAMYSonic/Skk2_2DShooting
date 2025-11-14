using UnityEngine;

public class BossMove : MonoBehaviour
{
    // 보스는 Y좌표 1.5f 지점까지 내려오면 멈춘다.
    [SerializeField]
    private float _bossStopYPosition = 1.5f;

    private void Update()
    {
        // 보스가 일정 위치까지 내려오면 멈추도록 한다.
        if (transform.position.y > _bossStopYPosition)
        {
            GoStraight();
        }
    }

    private void GoStraight()
    {
        Vector2 direction = Vector2.down;
        float speed = GetComponent<Boss>().Speed;
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
