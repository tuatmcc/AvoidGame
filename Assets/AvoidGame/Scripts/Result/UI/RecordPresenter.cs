using AvoidGame.Result.Interface;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace AvoidGame.Result.UI
{
    public class RecordPresenter : MonoBehaviour
    {
        private Image image;
        [SerializeField] private int targetRank;
        [Inject] IResultSceneManager _sceneManager;

        private void Awake()
        {
            image = GetComponent<Image>();
            image.enabled = false;
        }

        void Start()
        {
            SetNewRecordImage();
        }

        private void SetNewRecordImage()
        {
            if (targetRank == 0) return;
            if (_sceneManager.PlayerRank == targetRank)
            {
                image.enabled = true;
            }
        }
    }
}