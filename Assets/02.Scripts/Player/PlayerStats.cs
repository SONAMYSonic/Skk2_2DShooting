using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("플레이어 스탯")]
    private float _playerLife = 3;

    public void Hit(float damage)
    {
        if (_playerLife > 1)
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
