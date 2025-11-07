using UnityEngine;

public class ItemAttackSpeed : MonoBehaviour
{
    [Header("공격 속도 조정량")]
    public float AttackSpeedAmount = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false) return;
        else
        {
            PlayerFire playerFire = collision.GetComponent<PlayerFire>();
            playerFire.AttackSpeedUP(AttackSpeedAmount);
            Disappear();
        }
    }

    private void Disappear()
    {
        Destroy(gameObject);
    }
}
