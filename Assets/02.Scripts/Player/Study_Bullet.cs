using UnityEngine;

public class Study_Bullet : MonoBehaviour
{
    /* 
    심화 1. S자로 날아가는 총알
    심화 2. 소용돌이 치며 날라가는 총알
    */

    [Header("총알 속도")]
    public float startSpeed = 1f;
    public float endSpeed = 7f;
    public float duration = 1.2f;

    [Header("총알 모드")]
    public bool is_S_mode = false;
    public bool is_Spiral_mode = false;

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

        // 심화 1. S자로 날아가는 총알
        if (is_S_mode)
        {
            float frequency = 30f; // 진동수
            float magnitude = 0.02f; // 진폭
            float offsetX = Mathf.Sin(Time.time * frequency) * magnitude;
            transform.position += new Vector3(offsetX, 0f, 0f);
        }
        else
        {

        }

        // 심화 2. 소용돌이 치며 날라가는 총알
        if (is_Spiral_mode)
        {
            float spiralFrequency = 10f; // 소용돌이 진동수
            float spiralMagnitude = 0.3f; // 소용돌이 진폭
            float offsetX = Mathf.Cos(Time.time * spiralFrequency) * spiralMagnitude;
            float offsetY = Mathf.Sin(Time.time * spiralFrequency) * spiralMagnitude;
            transform.position += new Vector3(offsetX, offsetY, 0f);
        }
    }
}
