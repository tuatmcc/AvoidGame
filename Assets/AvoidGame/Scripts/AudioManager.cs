using UnityEngine;

namespace AvoidGame
{
    /// <summary>
    /// 音声の管理(AudioSource)
    /// </summary>
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        [SerializeField] AudioSource audioSource;
        
        public void PlaySe(AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }

        public void PlayBGM(AudioClip audioClip)
        {
            audioSource.loop = true;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}