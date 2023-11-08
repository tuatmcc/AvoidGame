using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AvoidGame.Audio
{
    /// <summary>
    /// シーンにおけるAudioSourceをセットする
    /// </summary>
    public class AudioSourceSetter : MonoBehaviour
    {
        [SerializeField] AudioSource seSource;
        [Inject] IAudioManager audioManager;

        private void Awake()
        {
            audioManager.audioSource = seSource;
        }
    }
}
