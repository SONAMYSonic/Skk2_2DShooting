using UnityEngine;

public class Study_Bullet : MonoBehaviour
{
    /* 
    심화 1. S자로 날아가는 총알
    심화 2. 소용돌이 치며 날라가는 총알
    */

    [Header("총알 속도")]
    public float StartSpeed = 1f;
    public float EndSpeed = 7f;
    public float Duration = 1.2f;

    [Header("총알 모드")]
    public bool is_S_mode = false;
    public bool is_Spiral_mode = false;

    [SerializeField]
    private float _speed;

    [Header("총알 속성")]
    public float Damage = 60f;

    private void Start()
    {
        _speed = StartSpeed; // 초기 속도로 설정
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

        float accerlation = (EndSpeed - StartSpeed) / Duration; // 초당 증가해야 하는 속도량
        //                              6          /    1.2   =    5
        _speed += Time.deltaTime * accerlation;    // 초당 + 1 * 가속도(accerlation)과 같다
        _speed = Mathf.Min(_speed, EndSpeed);       // 속도가 최대 속도를 넘지 않도록 제한
        //         ㄴ 어떤 속성과 어떤 메서드를 가지고 있는지 톺아볼 필요가 있다.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 총알은 Enemy와만 충돌 이벤트를 처리한다.
        if (collision.CompareTag("Enemy") == false)
            return;

        // GetComponent는 게임오브젝트에 붙어있는 컴포넌트를 가져올 수 있다
        Game.EnemySystem.Enemy enemy = collision.gameObject.GetComponent<Game.EnemySystem.Enemy>();

        // 객체간의 상호 작용을 할 때: 묻지말고 시켜라
        enemy.Hit(Damage);
        //Destroy(gameObject); // 총알 파괴
    }
}
