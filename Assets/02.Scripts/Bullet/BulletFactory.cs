using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    // 팩토리 방식: 유지보수가 편하다

    private static BulletFactory _instance = null;
    public static BulletFactory Instance => _instance;
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    [Header("총알 프리팹")]
    public GameObject BulletPrefab;          // 총알 프리팹
    public GameObject SubBulletPrefab;      // 보조 총알 프리팹
    public GameObject PetBulletPrefab;      // 펫 총알 프리팹

    public GameObject MakeBullet(Vector3 position)
    {
        // 필요하다면 여기서 생성 이펙트도 생성하고
        // 필요하다면 인자값으로 대미지도 받아서 넘겨주고...

        return Instantiate(BulletPrefab, position, Quaternion.identity, transform);
    }
    public GameObject MakeSubBullet(Vector3 position)
    {

        return Instantiate(SubBulletPrefab, position, Quaternion.identity, transform);
    }

    public GameObject MakePetBullet(Vector3 position)
    {
        return Instantiate(PetBulletPrefab, position, Quaternion.identity, transform);
    }
}
