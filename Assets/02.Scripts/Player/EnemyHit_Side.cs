using UnityEngine;

public class EnemyHit_Side : MonoBehaviour
{
    private void Start()
    {
        // 부모 오브젝트 (Enemy)의 Enemy 컴포넌트를 가져온다.
        Enemy parentEnemy = GetComponentInParent<Enemy>();
    }
}
