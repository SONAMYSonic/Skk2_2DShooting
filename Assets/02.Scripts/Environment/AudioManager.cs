using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    public enum EAudioType
    {
        EnemyDeath,
        ItemEat,
        SpecialAttack
    }
    public EAudioType audioType;

    public void PlaySound(EAudioType eAudioType)
    {
        if (eAudioType < 0 || (int)eAudioType >= audioClips.Length)
        {
            Debug.LogWarning("Invalid audio clip index: " + eAudioType);
            return;
        }
        audioSource.PlayOneShot(audioClips[(int)eAudioType]);
    }
}
