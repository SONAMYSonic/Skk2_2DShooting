using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    // 목표: 배경 스크롤이 되도록 하고 싶다

    // 필요 속성
    public Material BackgroundMaterial;
    public float ScrollSpeed = 0.1f;
    // - 머터리얼
    // - 스크롤 속도

    private void Update()
    {
        // 방향을 구한다.
        Vector2 direction = Vector2.up; // 위쪽 방향 (0, 1)
        // 움직인다 (스크롤한다)
        BackgroundMaterial.mainTextureOffset += direction * (ScrollSpeed * Time.deltaTime);
    }
}
