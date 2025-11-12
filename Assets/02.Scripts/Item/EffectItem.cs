using UnityEngine;

public class EffectItem : MonoBehaviour
{
    public GameObject EffectPrefab;
    private AudioManager _audioManager;

    private void Start()
    {
        // tag가 AudioManager인 오브젝트를 찾아서 AudioManager 컴포넌트를 가져온다.
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void OnDestroy()
    {
        _audioManager.PlaySound(AudioManager.EAudioType.ItemEat);
        Instantiate(EffectPrefab, transform.position, Quaternion.identity);
    }
}
