using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("플레이어 스탯")]
    private short _playerLife = 3;

    public void Hit(float damage)
    {
        if (_playerLife >= 0)
        {
            _playerLife--;
        }
        else
        {
            Debug.Log("꽥!");
            Destroy(gameObject);
        }
    }
}
