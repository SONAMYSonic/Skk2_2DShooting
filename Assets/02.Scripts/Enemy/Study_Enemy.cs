using UnityEngine;

public class EnemyB : MonoBehaviour
{
    // 목표: 플레이어를 쫓아가는 적을 만들고 싶다
    public GameObject PlayerObject;
    public float Speed = 3f;

    private void Start()
    {
        PlayerObject = GameObject.FindWithTag("Player");
    }


    // Update is called once per frame
    void Update()
    {
        // 1. 플레이어의 위치를 알아낸다
        

        // 2. 위치에 따라 방향을 구한다.
        Vector3 direction = (PlayerObject.transform.position - transform.position).normalized;

        // 3. 방향에 맞게 이동한다.
        transform.position += direction * Speed * Time.deltaTime;

    }
}
