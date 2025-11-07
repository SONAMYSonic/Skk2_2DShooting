using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    [Header("아이템 설정")]
    public float HealthBoost = 3f; // 체력 증가량

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false) return;
        else
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            playerStats.HealthUP(HealthBoost);
            Disappear();
        }
    }

    private void Disappear()
    {
        Destroy(gameObject);
    }
}
