using UnityEngine;
using Zenject;

namespace AvoidGame.Play.Audio
{
    public class CountDownSEPlayer : MonoBehaviour
    {
        [SerializeField] AudioClip countClip;
        [SerializeField] AudioClip startClip;

        [Inject] PlaySceneManager playSceneManager;
        [Inject] IAudioManager audioManager;

        private void Start()
        {
            playSceneManager.OnCountChanged += PlayCountSE;
        }

        private void PlayCountSE(int count)
        {
            if(count != 0)
            {
                audioManager.PlaySe(countClip);
            }
            else if(count ==  0)
            {
                audioManager.PlaySe(startClip);
            }
        }

    }
}
