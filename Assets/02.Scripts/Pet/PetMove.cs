using System.Collections.Generic;
using UnityEngine;

public class PetMove : MonoBehaviour
{
    // 타겟을 따라다니는 펫 움직임
    public Transform TargetTransform;
    private Queue<Vector3> _targetPositionQueue = new Queue<Vector3>();
    private Vector3 _followPos;

    public int FollowDelay = 3;

    private void Start()
    {
        // 초기화
        _followPos = TargetTransform.position;
    }

    private void FixedUpdate()
    {
        PlayerWatch();
        Follow();
    }

    private void PlayerWatch()
    {
        // 펫이 타겟 포지션을 큐에 저장하는 메서드
        // 타겟 위치 저장 큐에 현재 위치가 없으면~!
        if (_targetPositionQueue.Contains(TargetTransform.position) == false)
        {
            // 큐에 타겟 위치 추가하라~!
            _targetPositionQueue.Enqueue(TargetTransform.position);
        }

        // 큐의 크기가 지연 시간보다 크면~
        if (_targetPositionQueue.Count > FollowDelay)
        {
            // 큐에서 가장 오래된 위치를 꺼내서 펫 위치로 설정
            // 즉 FollwDelay만큼 큐가 쌓인다
            _followPos = _targetPositionQueue.Dequeue();
        }

    }

    private void Follow()
    {
        transform.position = _followPos;
    }
}
