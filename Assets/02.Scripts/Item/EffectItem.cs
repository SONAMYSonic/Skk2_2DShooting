using UnityEngine;

public class EffectItem : MonoBehaviour
{
    public GameObject EffectPrefab;

    private void OnDestroy()
    {
        AudioManager.AudioManagerInstance.PlaySound(AudioManager.EAudioType.ItemEat);
        Instantiate(EffectPrefab, transform.position, Quaternion.identity);
    }
}
