using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("플레이어 스탯")]
    private float _playerLife = 3;

    public void Hit(float damage)
    {
        if (_playerLife > 1)    // 남은 생명이 1 이상일 때
        {
            _playerLife -= damage;
        }
        else    // 죽음 ㅋㅋㅋㅋ
        {
            Destroy(gameObject);
        }
    }

    public void HealthUP(float amount)
    {
        _playerLife += amount;
    }
}
