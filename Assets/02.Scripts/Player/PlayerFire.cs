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

    // 플레이어가 총알 생성 (PlayerFire)
    // 적이 총알 생성 (EnemyFire, Enemy, EnemyController)
    // 펫도 총알 생성 (PetFire)
    //  ㄴ 생성 로직이 바뀔때마다 모든 코드가 수정되어야 한다
    //  ㄴ 총알 생성이라는 공통 기능을 담당하는 클래스를 만들면 편하지 않을까?
    // 총알 생성기.만들어줘(타입, 대미지, 위치, 생성이펙트);


    [Header("총구")]
    public Transform FirePosition;          // 총알 발사 위치
    public Transform SubFirePositionLeft;   // 보조 총알 발사 위치
    public Transform SubFirePositionRight;  // 보조 총알 발사 위치
    private float _fireOffsetX = 0.25f;      // 총알 발사 X 오프셋

    [Header("발사 쿨타임")]
    private float _fireCooldown = 0.6f; // 발사 쿨타임
    private float _fireCooldownTimer = 0f; // 쿨타임 타이머

    [Header("자동 발사 모드")]
    public bool IsAutoFire = true; // 자동 발사 모드 여부

    [Header("필살기")]
    public GameObject SpecialAttackObject; // 필살기 오브젝트

    [Header("사운드")]
    public AudioSource FireSound; // 발사 사운드

    private void Update()
    {

        // 쿨타임 타이머 갱신.
        if (_fireCooldownTimer > 0f)
        {
            _fireCooldownTimer -= Time.deltaTime;   // 타이머 감소
        }

        // 스페이스바 입력 혹은 오토일 시 발사 (+ 쿨타임 체크).
        if ((Input.GetKey(KeyCode.Space) || IsAutoFire) && _fireCooldownTimer <= 0f)
        {
            FireSound.Play();

            FireBullet();
            SubFireBullet();
            // 쿨타임 초기화.
            _fireCooldownTimer = _fireCooldown;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // 필살기 발동.
            SpecialAttackObject.SetActive(true);
        }
    }

    private void FireBullet()
    {
        BulletFactory.Instance.MakeBullet(FirePosition.position - new Vector3(_fireOffsetX, 0f, 0f));
        BulletFactory.Instance.MakeBullet(FirePosition.position + new Vector3(_fireOffsetX, 0f, 0f));
    }

    private void SubFireBullet()
    {
        BulletFactory.Instance.MakeSubBullet(SubFirePositionLeft.position);
        BulletFactory.Instance.MakeSubBullet(SubFirePositionRight.position);
    }

    public void AttackSpeedUP(float amount)
    {
        // 발사 쿨타임 감소 연산.
        _fireCooldown *= amount;
        Debug.Log("공격 속도 상승!");
    }

    public void ToggleAutoFire(bool autofire)
    {
        IsAutoFire = autofire;
    }
}
