using UnityEngine;

public class EffectItem : MonoBehaviour
{
    public GameObject EffectPrefab;

    private void OnDestroy()
    {
        Instantiate(EffectPrefab, transform.position, Quaternion.identity);
    }
}
