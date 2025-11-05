using UnityEngine;
using UnityEngine.Rendering;

public class PlayerFire : MonoBehaviour
{
    // 목표: 스페이스바를 누르면 총알 발사

    /*
    1. 총알을 양 옆으로 2개 발사
    2. 총알 발사에 쿨타임 적용
        - 0.6초에 한 발 발사 가능
    3. 자동 공격 모드 구현
        - 1번 누르면 자동 공격
        - 2번 누르면 수동 공격
        - Default는 1번
    4. 캐릭터 양 옆으로 보조 총알 발사
        - 보조 총알은 속도와 모양이 다르다
    */

    // 필요 속성
    [Header("총알 프리팹")]
    public GameObject BulletPrefab; // 총알 프리팹
    public GameObject SubBulletPrefab; // 보조 총알 프리팹

    [Header("총구")]
    public Transform FirePosition; // 총알 발사 위치
    public Transform SubFirePositionLeft; // 보조 총알 발사 위치
    public Transform SubFirePositionRight; // 보조 총알 발사 위치

    [Header("발사 쿨타임")]
    public const float FireCooldown = 0.6f; // 발사 쿨타임
    private float _fireCooldownTimer = 0f; // 쿨타임 타이머

    [Header("자동 발사 모드")]
    public bool isAutoFire = true; // 자동 발사 모드 여부

    private void Update()
    {
        // 발사 모드 키 입력 감지 (1번: 자동, 2번 수동)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isAutoFire = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isAutoFire = false;
        }

        // 쿨타임 타이머 갱신
        if (_fireCooldownTimer > 0f)
        {
            _fireCooldownTimer -= Time.deltaTime;   // 타이머 감소
        }

        // 스페이스바 입력 혹은 오토일 시 발사 (+ 쿨타임 체크)
        if ((Input.GetKey(KeyCode.Space) || isAutoFire) && _fireCooldownTimer <= 0f)
        {
            FireBullet();
            SubFireBullet();
            _fireCooldownTimer = FireCooldown; // 쿨타임 초기화
        }
    }

    private void FireBullet()
    {
        // 총알 발사 위치에서 x -0.25만큼 떨어진 곳에서 첫번째 총알 생성
        Instantiate(BulletPrefab, FirePosition.position - new Vector3(0.25f, 0f, 0f), FirePosition.rotation);
        // 총알 발사 위치에서 x +0.25만큼 떨어진 곳에서 두번째 총알 생성
        Instantiate(BulletPrefab, FirePosition.position + new Vector3(0.25f, 0f, 0f), FirePosition.rotation);
    }

    private void SubFireBullet()
    {
        Instantiate(SubBulletPrefab, SubFirePositionLeft.position, FirePosition.rotation);
        Instantiate(SubBulletPrefab, SubFirePositionRight.position, FirePosition.rotation);
    }
}
