using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip[] AudioClips;

    public enum EAudioType
    {
        EnemyDeath,
        ItemEat,
        SpecialAttack
    }
    public EAudioType audioType;

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
