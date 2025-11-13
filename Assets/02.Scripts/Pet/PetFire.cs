using UnityEngine;

public class PetFire : MonoBehaviour
{
    [Header("총알 프리팹")]
    public GameObject PetBulletPrefab;

    [Header("발사 쿨타임")]
    private float _petFireCooldown = 3f; // 발사 쿨타임
    private float _petFireCooldownTimer = 0f; // 쿨타임 타이머

    private float _fireOffsetY = 0.25f;      // 총알 발사 Y 오프셋

    private void Update()
    {
        _petFireCooldownTimer += Time.deltaTime;

        // 쿨타임 시 발사 (+ 쿨타임 체크).
        if (_petFireCooldownTimer >= _petFireCooldown)
        {

            PetFireBullet();
            // 쿨타임 초기화.
            _petFireCooldownTimer = 0f;
        }
    }

    private void PetFireBullet()
    {
        BulletFactory.Instance.MakeBullet(transform.position + new Vector3(0f, _fireOffsetY, 0f));
    }
}
