using UnityEngine;

public class PetFire : MonoBehaviour
{
    [Header("총알 프리팹")]
    public GameObject PetBulletPrefab;

    [Header("발사 쿨타임")]
    private float _petFireCooldown = 3f; // 발사 쿨타임
    private float _petFireCooldownTimer = 0f; // 쿨타임 타이머

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
        // 총알 발사 위치에서 y +0.25만큼 떨어진 곳에서 총알 생성
        Instantiate(PetBulletPrefab, transform.position + new Vector3(0f, 0.25f, 0f), Quaternion.identity);
    }
}
