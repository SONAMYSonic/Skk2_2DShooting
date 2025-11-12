using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    // 필살기. 닿는 적 모두 파괴, 3초후 비활성화.
    
    [Header("필살기 지속 시간")]
    public float UltimateDuration = 3f;  // 필살기 지속 시간
    private float _ultimateTimer = 0f;   // 필살기 타이머

    public AudioManager AudioManager;

    private void OnEnable()
    {
        _ultimateTimer = UltimateDuration;
        // 필살기 사운드 재생
        AudioManager.PlaySound(AudioManager.EAudioType.SpecialAttack);
    }

    private void Update()
    {
        _ultimateTimer -= Time.deltaTime;
        if (_ultimateTimer <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().EnemyDead();
        }
    }

}
