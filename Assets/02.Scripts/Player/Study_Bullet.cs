using UnityEngine;

public class Study_Bullet : MonoBehaviour
{
    [Header("총알 속도")]
    public float startSpeed = 1f;
    public float endSpeed = 7f;
    public float duration = 1.2f;

    [SerializeField]
    private float _speed;

    private void Start()
    {
        _speed = startSpeed; // 초기 속도로 설정
    }

    void Update()
    {
        // 방향을 구한다.
        Vector2 direction = transform.up;

        // 방향에 따라 이동한다.
        // 새로운 위치는 = 현재 위치 + 방향 * 속도 * 시간
        Vector2 position = transform.position;
        Vector2 newPosition = position + direction * _speed * Time.deltaTime;
        transform.position = newPosition;       // 총알 이동

        // transform.position += transform.up * speed * Time.deltaTime;\

        // 목표: Duration(1.2초) 동안 startSpeed(1)에서 endSpeed(7)까지 속도 증가

        float accerlation = (endSpeed - startSpeed) / duration; // 초당 증가해야 하는 속도량
        //                              6          /    1.2   =    5
        _speed += Time.deltaTime * accerlation;    // 초당 + 1 * 가속도(accerlation)과 같다
        _speed = Mathf.Min(_speed, endSpeed);       // 속도가 최대 속도를 넘지 않도록 제한
        //         ㄴ 어떤 속성과 어떤 메서드를 가지고 있는지 톺아볼 필요가 있다.
    }
}
