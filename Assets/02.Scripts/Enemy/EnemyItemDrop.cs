using UnityEngine;

public class EnemyItemDrop : MonoBehaviour
{
    // 적이 죽을 때 아이템 드롭
    // 드롭 확률은 50%
    // 체력/이속/공속 확률 각각 70&, 20%, 10%

    [Header("드롭 아이템 프리팹 배열")]
    public GameObject[] ItemPrefabs; // 드롭할 아이템 프리팹 배열

    [Header("드롭 확률")]
    [Range(0f, 1f)]
    public float DropChance = 0.5f; // 아이템 드롭 확률

    public float DropRateHealth = 0.7f; // 체력 아이템 드롭 확률
    public float DropRateSpeed = 0.2f;  // 이속 아이템 드롭 확률
    public float DropRateAttackSpeed = 0.1f; // 공속 아이템 드롭 확률

    public enum EItemType
    {
        Health,
        Speed,
        AttackSpeed
    }

    // 적 오브젝트가 파괴될 때 호출되는 메서드
    private void OnDestroy()
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



}
