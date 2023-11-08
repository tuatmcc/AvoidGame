using UnityEngine;

namespace AvoidGame
{
    public interface IAudioManager
    {
        public AudioSource audioSource { get; set; }

        public void PlaySe(AudioClip audioClip) { }

        public void PlaySeNotOverlap(AudioClip audioClip) { }
    }
}
