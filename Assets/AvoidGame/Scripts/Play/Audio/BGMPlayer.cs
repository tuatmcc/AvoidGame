using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class BGMPlayer : MonoBehaviour
    {
        [SerializeField] AudioSource m_AudioSource;
        [Inject] PlaySceneManager m_SceneManager;

        private void Start()
        {
            m_SceneManager.OnPlayStateChanged += StartBGM;
        }

        private void StartBGM(PlaySceneState playSceneState)
        {
            if(playSceneState == PlaySceneState.Playing)
            {
                m_AudioSource.Play();
            }
        }
    }
}
