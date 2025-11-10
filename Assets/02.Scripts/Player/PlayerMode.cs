using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(PlayerAutoBattle))]
[RequireComponent(typeof(PlayerFire))]

public class PlayerMode : MonoBehaviour
{
    // 플레이어 모드 제어 스크립트

    [Header("플레이어 모드 설정")]
    public bool isAutoFireMode = true;   // 자동 발사 모드 여부
    public bool IsAutoMoveMode = true;   // 자동 이동 모드 여부

    private PlayerAutoBattle _playerAutoBattle;
    private PlayerFire _playerFire;

    private void Awake()
    {
        _playerAutoBattle = GetComponent<PlayerAutoBattle>();
        _playerFire = GetComponent<PlayerFire>();
    }

    private void Update()
    {
        // 발사 모드 키 입력 감지 (1번: 자동, 2번 수동)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isAutoFireMode = true;
            IsAutoMoveMode = true;
            _playerAutoBattle.ToggleAutoMove(true);
            _playerFire.ToggleAutoFire(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isAutoFireMode = false;
            IsAutoMoveMode = false;
            _playerAutoBattle.ToggleAutoMove(false);
            _playerFire.ToggleAutoFire(false);
        }
    }
}