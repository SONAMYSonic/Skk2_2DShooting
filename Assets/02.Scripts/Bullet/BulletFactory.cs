using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    // 팩토리 방식: 유지보수가 편하다

    private static BulletFactory _instance = null;
    public static BulletFactory Instance => _instance;


    [Header("총알 프리팹")]
    public GameObject BulletPrefab;          // 총알 프리팹
    public GameObject SubBulletPrefab;      // 보조 총알 프리팹
    public GameObject PetBulletPrefab;      // 펫 총알 프리팹

    [Header("풀링")]
    public int PoolSize = 30;
    private GameObject[] _bulletObjectPool;      // 게임 총알을 담아둘 풀: 탄창
    private GameObject[] _subBulletObjectPool;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        PlayerPoolInit();
        PlayerSubPoolInit();
    }

    private void PlayerPoolInit()
    {
        // 1. 탄창을 총알을 담을 수 있는 크기 배열 만들어준다
        _bulletObjectPool = new GameObject[PoolSize];

        // 2. 탄창 크기만큼 반복해서
        for (int i = 0; i < PoolSize; i++)
        {
            // 3. 총알을 생성한다.
            GameObject bulletObject = Instantiate(BulletPrefab, transform);

            // 4. 생성한 총알을 탄창에 담는다
            _bulletObjectPool[i] = bulletObject;

            // 5. 비활성화 한다
            bulletObject.SetActive(false);
        }
    }

    private void PlayerSubPoolInit()
    {
        _subBulletObjectPool = new GameObject[PoolSize];

        for (int i = 0; i < PoolSize; i++)
        {
            GameObject subBulletObject = Instantiate(SubBulletPrefab, transform);
            _subBulletObjectPool[i] = subBulletObject;
            subBulletObject.SetActive(false);
        }
    }

    public GameObject MakeBullet(Vector3 playerFirePosition)
    {
        // 필요하다면 여기서 생성 이펙트도 생성하고
        // 필요하다면 인자값으로 대미지도 받아서 넘겨주고...

        // 1. 탄창 안에 있는 총알들 중에서
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject bulletObject = _bulletObjectPool[i];

            // 2. 비활성화 된 총알 하나를 찾아
            if (bulletObject.activeInHierarchy == false)
            {
                // 3. 위치를 수정하고, 활성화한다
                bulletObject.transform.position = playerFirePosition;
                bulletObject.SetActive(true);

                // 4. 총알 반환
                return bulletObject;
            }
        }

        Debug.LogWarning("탄창이 부족합니다!");
        return null;
    }
    public GameObject MakeSubBullet(Vector3 position)
    {
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject subBulletObject = _subBulletObjectPool[i];
            if (subBulletObject.activeInHierarchy == false)
            {
                subBulletObject.transform.position = position;
                subBulletObject.SetActive(true);
                return subBulletObject;
            }
        }
        Debug.LogWarning("서브 탄창이 부족합니다!");
        return null;
    }

    public GameObject MakePetBullet(Vector3 position)
    {
        return Instantiate(PetBulletPrefab, position, Quaternion.identity, transform);
    }
}
