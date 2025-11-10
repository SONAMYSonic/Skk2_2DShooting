using UnityEngine;
using UnityEngine.Rendering;

public class MoveItem : MonoBehaviour
{
    // 드랍된 아이템은 2초 후 플레이어에게 날아온다
    // 베지어 곡선을 이용하여 휘어지게

    [Header("아이템 이동 시간")]
    public float MoveDuration = 5f;

    [Header("시간 설정")]
    public float DelayTime = 2f;

    private Transform _playerTransform;
    private Vector3 _startPosition;
    private float _elapsedTime = 0f;    // 경과 시간
    private bool _isMovingToPlayer = false;

    [Header("베지어 곡선 조절")]
    public float BezierHeight = 2f; // 베지어 곡선의 높이 조절 변수


    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _startPosition = transform.position;
    }

    private void Update()
    {
        // DelayTime 만큼 대기
        DelayTime -= Time.deltaTime;

        // 대기 시간이 끝나면 플레이어에게 이동 시작
        if (DelayTime <= 0f)
        {
            _isMovingToPlayer = true;
        }

        if (_isMovingToPlayer)
        {
            _elapsedTime += Time.deltaTime / MoveDuration;

            // 베지어 곡선 계산
            Vector3 controlPoint = (_startPosition + _playerTransform.position) / 2 + Vector3.right * BezierHeight;
            Vector3 m1 = Vector3.Lerp(_startPosition, controlPoint, _elapsedTime);
            Vector3 m2 = Vector3.Lerp(controlPoint, _playerTransform.position, _elapsedTime);
            transform.position = Vector3.Lerp(m1, m2, _elapsedTime);
        }
    }




}
