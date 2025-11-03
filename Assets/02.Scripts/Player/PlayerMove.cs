using UnityEngine;

// 플레이어 이동
public class PlayerMove : MonoBehaviour
{
    // 목표
    // 키보드 입력에 따라 방향을 구하고 그 방향으로 이동시키고 싶다

    // 구현 순서
    // 1. 키보드 입력
    // 2. 방향 구하는 방법
    // 3. 이동

    // 필요 속성
    public float Speed = 0.5f; // 이동 속도

    void Start()
    {
        
    }

    void Update()
    {
        // 1. 키보드 입력을 감지한다.
        // 유니티에서는 Input이라고 하는 모듈이 입력에 관한 모든것을 담당
        float h = Input.GetAxis("Horizontal");    // 수평 입력에 대한 값을 -1, 0, 1로 가져온다
        float v = Input.GetAxis("Vertical");      // 수직 입력에 대한 값을 -1, 0, 1로 가져온다

        Debug.Log($"h : {h}, v : {v}");

        // 2. 입력으로부터 방향을 구한다.
        Vector2 direcrion = new Vector2(h, v);
        Debug.Log($"direction : {direcrion.x}, {direcrion.y}");

        // 3. 그 방향으로 이동을 한다.
        Vector2 position = transform.position; // 현재 위치를 가져온다


        // 새로운 위치 = 현재 위치 + (방향 * 속력) * 시간
        // 새로운 위치 = 현재 위치 + 속도 * 시간
        //       새로운 위치 = 현재 위치 + 방향 * 속도
        Vector2 newPosition = position + direcrion * Speed * Time.deltaTime;                    // 새로운 위치

        // Time.deltaTime : 이전 프레임으로부터 현재 프레임까지 시간이 얼마나 흘렀는지.. 나타내는 값
        // 1초 / FPS 값과 비슷하다.

        // 이동속도 : 10
        // 컴퓨터1:  50FPS: Update함수가 1초에  50번 호출 -> 10 *  50 =  500 * Time.deltaTime = 두개의 값이 같아진다
        // 컴퓨터2: 200FPS: Update함수가 1초에 200번 호출 -> 10 * 200 = 2000 * Time.deltaTime

        transform.position = newPosition;       // 새로운 위치로 이동을 한다
    }
}
