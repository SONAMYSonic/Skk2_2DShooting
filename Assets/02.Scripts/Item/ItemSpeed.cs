using UnityEngine;

public class ItemSpeed : MonoBehaviour
{
    // 충돌 트리거가 일어났을때
    // 만약 플레이어 태그라면
    // 플레이어 게임오브젝트의 PlayerMove 컴포넌트를 읽어온다
    // 스피드를 +N 해 준다.
    // 나를 삭제

    [Header("아이템 설정")]
    public float SpeedBoost = 2f; // 스피드 증가량

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false) return;

        else
        {
            PlayerRecord playerRecord = collision.GetComponent<PlayerRecord>();
            playerRecord.Boost(SpeedBoost);
            Disappear();
        }
    }

    private void Disappear()
    {
        Destroy(gameObject);
    }
}
