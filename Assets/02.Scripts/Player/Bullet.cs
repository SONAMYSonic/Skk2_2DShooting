using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("총알 속도")]
    public float speed = 1f;
    public float maxSpeed = 7f;        // 최대 속도
    public float zeroToMaxSpeed = 1.2f;    // 최대 속도에 도달하는 시간
    public float initialSpeed = 1f;    // 초기 속도

    private void Start()
    {
        speed = initialSpeed; // 초기 속도로 설정
    }

    void Update()
    {
        // 방향을 구한다.
        Vector2 direction = transform.up;

        // 방향에 따라 이동한다.
        // 새로운 위치는 = 현재 위치 + 방향 * 속도 * 시간
        Vector2 position = transform.position;
        Vector2 newPosition = position + direction * speed * Time.deltaTime;
        transform.position = newPosition;       // 총알 이동

        // transform.position += transform.up * speed * Time.deltaTime;\

        // 속도 증가 초기 총알 속도(speed) 1에서 최대 속도(maxSpeed) 7까지 1.2초(zeroToMaxSpeed) 동안 증가
        speed += (maxSpeed - initialSpeed) / zeroToMaxSpeed * Time.deltaTime;
        speed = Mathf.Min(speed, maxSpeed); // 속도가 최대 속도를 넘지 않도록 제한

    }
}
