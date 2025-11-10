using UnityEngine;

// 플레이어는 스탯을 중앙관리하고, 컴포넌트도 관리한다.
public class Study_Player : MonoBehaviour
{
    // SerializeField 속성 헷갈리므로 남발 금지
    private Study_PlayerManualMove _study_PlayerManualMove;
    private Study_PlayerAutoMove _study_PlayerAutoMove;

    private bool _autoMode = false;
    private float _health = 3;

    public float Speed = 3f;

    private void Start()
    {
        _study_PlayerAutoMove = GetComponent<Study_PlayerAutoMove>();
        _study_PlayerManualMove = GetComponent<Study_PlayerManualMove>();
    }

    public void Hit(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
