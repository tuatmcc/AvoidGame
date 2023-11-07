using UnityEngine;

namespace AvoidGame.Audio
{
    /// <summary>
    /// 音声の管理(AudioSource)
    /// </summary>
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        public AudioSource audioSource { get; set; }
        
        public void PlaySe(AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }

        public void PlayBGM(AudioClip audioClip, bool loop)
        {
            audioSource.loop = loop;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}