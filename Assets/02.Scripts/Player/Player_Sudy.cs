using UnityEngine;

public class Player_Sudy : MonoBehaviour
{
    private float _health = 3;

    public void Hit(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
