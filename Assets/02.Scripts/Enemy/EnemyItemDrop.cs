using UnityEngine;

public class EnemyItemDrop : MonoBehaviour
{
    // 적이 죽을 때 아이템 드롭
    // 드롭 확률은 50%
    // 체력/이속/공속 확률 각각 70&, 20%, 10%

    [Header("드롭 아이템 프리팹 배열")]
    public GameObject[] ItemPrefabs; // 드롭할 아이템 프리팹 배열
    [Header("드롭 아이템 가중치 배열")]
    public int[] ItemWeights; // 아이템 드롭 가중치 배열

    [Header("드롭 확률")]
    [Range(0f, 1f)]
    public float DropChance = 0.5f; // 아이템 드롭 확률

    public float DropRateHealth = 0.7f; // 체력 아이템 드롭 확률
    public float DropRateSpeed = 0.2f;  // 이속 아이템 드롭 확률
    public float DropRateAttackSpeed = 0.1f; // 공속 아이템 드롭 확률

    private int weigthtSum;

    public enum EItemType
    {
        Health,
        Speed,
        AttackSpeed
    }

    // 오브젝트 풀링으로 구현 시 OnDisable()에서 처리
    // 하면 처음 풀링 시 아이템이 드롭되는 현상이 발생하므로
    // 하나의 개별 메서드로 구현한 다음 적이 죽을 때 (Enemy.cs) 호출하자
    // 근데 지금보니까 아래 코드는 내가 만든거고 저 아래 코드가 강사님이 제작해주신거네...
    public void ItemSpawn()
    {
        // 아이템 드롭 확률 체크
        float randomValue = Random.Range(0f, 1f);
        if (randomValue <= DropChance)
        {
            // 드롭할 아이템 결정
            float itemRandomValue = Random.Range(0f, 1f);
            GameObject itemToDrop;
            if (itemRandomValue <= DropRateHealth)
            {
                itemToDrop = ItemPrefabs[(int)EItemType.Health];
            }
            else if (itemRandomValue <= DropRateHealth + DropRateSpeed)
            {
                itemToDrop = ItemPrefabs[(int)EItemType.Speed];
            }
            else
            {
                itemToDrop = ItemPrefabs[(int)EItemType.AttackSpeed];
            }
            // 아이템 생성
            Instantiate(itemToDrop, transform.position, Quaternion.identity);
            Debug.Log("Item Dropped: " + itemToDrop.name);
        }
    }


    public void ItemDrop()
    {
        // 50% 확률로 리턴
        if (Random.Range(0, 2) == 0) return;

        // 가중치의 합
        // ItemWeights [70, 20, 10]
        int weightSum = 0;  // 100
        for (int i = 0; i < ItemWeights.Length; ++i)
        {
            weightSum += ItemWeights[i];
        }

        // 0 ~ 100 가중치의 합
        int randomValue = UnityEngine.Random.Range(0, weightSum); // 80

        // 가중치 값을 더해가며 구간을 비교한다.
        // <           70 -> 0번째 아이템 생성되고
        // < (70+20)   90 -> 1번째 아이템 생성되고
        // < (90+10) 105 -> 2번째 아이템이 생성된다.
        int sum = 0;
        for (int i = 0; i < ItemWeights.Length; ++i)
        {
            sum += ItemWeights[i];
            if (randomValue < sum)
            {
                Instantiate(ItemPrefabs[i], transform.position, Quaternion.identity);
            }
        }
    }
}
