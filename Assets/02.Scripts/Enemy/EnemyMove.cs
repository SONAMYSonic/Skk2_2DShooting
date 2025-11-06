using System;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [Header("본인 능력치")]
    private Enemy _enemy;

    public enum EEnemyType
    {
        Straight,
        Following,
        Zigzag
    }
    [Header("적 움직임 방식")]
    public EEnemyType Type;
    

    private void Start()
    {
        // 캐싱: 자주 쓰는 데이터를 미리 가까운 곳에 저장해두고 참조하는 것
        // 플레이어 오브젝트 찾기
        _enemy = gameObject.GetComponent<Enemy>();
    }

    // Update is called once per frame
    private void Update()
    {
        GoStraight();

        // 타입을 조건문으로 분기하기 보다는
        // 0. 타입에 따라 동작이 다르다?               -> 함수로 쪼개자
        // 1. 함수가 너무 많아질거 같다 (OCP위반)      -> 클래스로 쪼개자
        // 2. 쪼개고 나니까 똑같은 기능/속성이 있다    -> 상속
        // 3. 상속을 하자니 책임이 너무 크다 (SRP위반) -> 조합
    }

    public void GoStraight()
    {
        Vector2 direction = Vector2.down;
        transform.Translate(direction * _enemy.Speed * Time.deltaTime);
    }
}
