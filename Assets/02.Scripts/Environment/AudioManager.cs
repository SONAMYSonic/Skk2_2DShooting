using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _audioManagerInstance;
    public static AudioManager AudioManagerInstance => _audioManagerInstance;

    public AudioSource AudioSource;
    public AudioClip[] AudioClips;

    public enum EAudioType
    {
        EnemyDeath,
        ItemEat,
        SpecialAttack
    }
    public EAudioType audioType;

    // Singleton Pattern 적용
    private void Awake()
    {
        if (_audioManagerInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        _audioManagerInstance = this;
    }

    public void PlaySound(EAudioType eAudioType)
    {
        if (eAudioType < 0 || (int)eAudioType >= AudioClips.Length)
        {
            Debug.LogWarning("Invalid audio clip index: " + eAudioType);
            return;
        }
        AudioSource.PlayOneShot(AudioClips[(int)eAudioType]);
    }
}
