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
        [SerializeField] AudioSource source;
        [Inject] AudioManager audioManager;

        private void Awake()
        {
            audioManager.audioSource = source;
        }
    }
}
