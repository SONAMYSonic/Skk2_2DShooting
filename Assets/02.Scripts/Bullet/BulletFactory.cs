using UnityEngine;
using System.Collections.Generic;

public class BulletFactory : MonoBehaviour
{
    // 팩토리 방식: 유지보수가 편하다

    private static BulletFactory _instance = null;
    public static BulletFactory Instance => _instance;


    [Header("총알 프리팹")]
    public GameObject BulletPrefab;          // 총알 프리팹
    public GameObject SubBulletPrefab;      // 보조 총알 프리팹
    public GameObject PetBulletPrefab;      // 펫 총알 프리팹
    public GameObject BossBulletPrefab;

    [Header("풀링")]
    public int PoolSize = 30;
    public int BossPoolSize = 50;
    private GameObject[] _bossBulletObjectPool;

    private List<GameObject> _playerBulletList = new List<GameObject>();
    private List<GameObject> _subBulletList = new List<GameObject>();
    private List<GameObject> _bossBulletList = new List<GameObject>();
    private int _playerBulletPoolSize = 50;
    private int _bossBulletPoolSize = 50;
    private int _bonusPoolSize = 25;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        _bossBulletObjectPool = InitializePool(BossBulletPrefab, BossPoolSize);

        BulletAddList(_playerBulletPoolSize, _playerBulletList, BulletPrefab);
        BulletAddList(_playerBulletPoolSize, _subBulletList, SubBulletPrefab);
        BulletAddList(_bossBulletPoolSize, _bossBulletList, BossBulletPrefab);
    }

    private GameObject[] InitializePool(GameObject prefab, int size)
    {
        // 1. 탄창을 총알을 담을 수 있는 크기 배열 만들어준다
        GameObject[] pool = new GameObject[size];

        // 2. 탄창 크기만큼 반복해서
        for (int i = 0; i < size; i++)
        {
            // 3. 총알을 생성한다.
            GameObject obj = Instantiate(prefab, transform);

            // 4. 비활성화 한다
            obj.SetActive(false);

            // 5. 생성한 총알을 탄창에 담는다
            pool[i] = obj;
        }
        return pool;
    }

    private void BulletAddList(int magicnumber, List<GameObject> gameObjects, GameObject bulletPrefab)
    {
        // 수신 매개변수만큼 총알을 생성해서 리스트에 추가
        for (int i = 0; i < magicnumber; i++)
        {
            gameObjects.Add(Instantiate(bulletPrefab, transform));
            // 비활성화
            gameObjects[i].SetActive(false);
        }
    }

    public void MakePlayerBullet(Vector3 playerFirePosition)
    {
        for (int i = 0; i < _playerBulletList.Count; i++)
        {
            if (_playerBulletList[i].activeInHierarchy == false)
            {
                _playerBulletList[i].transform.position = playerFirePosition;
                _playerBulletList[i].SetActive(true);
                return; // 하나 활성화 시키고 종료. return 안 하면 모든 비활성화 된 총알이 다 활성화 됨
            }
        }
        BulletAddList(_bonusPoolSize, _playerBulletList, BulletPrefab);
    }

    public void MakeSubBullet(Vector3 playerFirePosition)
    {
        for (int i = 0; i < _subBulletList.Count; i++)
        {
            if (_subBulletList[i].activeInHierarchy == false)
            {
                _subBulletList[i].transform.position = playerFirePosition;
                _subBulletList[i].SetActive(true);
                return; // 하나 활성화 시키고 종료. return 안 하면 모든 비활성화 된 총알이 다 활성화 됨
            }
        }
        BulletAddList(_bonusPoolSize, _subBulletList, SubBulletPrefab);
    }

    // Todo: 펫 총알 풀링 적용하기
    public GameObject MakePetBullet(Vector3 position)
    {
        return Instantiate(PetBulletPrefab, position, Quaternion.identity, transform);
    }

    public GameObject MakeBossBullet(Vector3 position)
    {
        for (int i = 0; i < BossPoolSize; i++)
        {
            GameObject bossBulletObject = _bossBulletObjectPool[i];
            if (bossBulletObject.activeInHierarchy == false)
            {
                bossBulletObject.transform.position = position;
                bossBulletObject.SetActive(true);
                return bossBulletObject;
            }
        }
        Debug.LogWarning("보스 탄창이 부족합니다!");
        return null;
    }

    public GameObject AAMakeBossBullet(Vector3 bossFirePosition)
    {
        for (int i = 0; i < _bossBulletList.Count; i++)
        {
            if (_bossBulletList[i].activeInHierarchy == false)
            {
                _bossBulletList[i].transform.position = bossFirePosition;
                _bossBulletList[i].SetActive(true);
                return _bossBulletList[i];
            }
        }
        BulletAddList(_bonusPoolSize, _bossBulletList, BossBulletPrefab);
        return null;
    }
}
