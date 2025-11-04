using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // 목표: 스페이스바를 누르면 총알 발사

    // 필요 속성
    [Header("총알 프리팹")]
    public GameObject BulletPrefab; // 총알 프리팹

    [Header("총구")]
    public Transform FirePosition; // 총알 발사 위치

    private void Update()
    {
        // 1. 발사 버튼을 누르면
        if (Input.GetKey(KeyCode.Space))
        {
            // 유니티에서 게임 오브젝트를 생성하는 메서드: Instantiate(프리팹, 위치, 회전)
            // 클래스 -> 객체(속성+기능) -> 메모리에 로드된 객체를 인스턴스
            //                           ㄴ 인스턴스화

            // 2. 프리팹으로부터 게임 오브젝트를 생성한다.
            GameObject bullet = Instantiate(BulletPrefab);
            bullet.transform.position = FirePosition.position; // 플레이어 위치에서 발사
        }
        // 
        
    }
}
