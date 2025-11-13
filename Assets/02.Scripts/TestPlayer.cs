using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    // 무결성: 속성(데이터)의 정확성, 일관성, 유효성
    private int _health;      // 0 ~ MaxHealth      // public -> private: 은닉화

    // 프로퍼티(Property) 사용
    public int Health => _health;   // get 프로퍼티

    // 체력이 바뀌는 경우: 맞았을때, 힐
    public void Heal(int amount)
    {
        // 규칙
        _health += amount;
    }

    public void TakeDamage(int amount)
    {
        // 규칙
        _health -= amount;
    }

    public void Revive()
    {
        //_health = MaxHealth;
    }

    // Getter / Setter <- 규칙이 없는 경우는 의미가 없다.
    // Setter는 가능한 한 사용하지 않는 것이 좋다.

    public void SetHealth(int health)
    {
        // 도메인 규칙
        // 체력은 0 ~ 100 사이여야 한다.
        _health = health;
    }

    

}
