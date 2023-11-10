using System;
using UnityEngine;
using Zenject;

namespace AvoidGame.Description
{
    public class DescriptionUI : MonoBehaviour
    {
        [Inject] private IDescriptionSceneManager _descriptionSceneManager;
        [SerializeField] private RectTransform[] slides;

        private void Awake()
        {
            if (slides.Length != Enum.GetValues(typeof(DescriptionState)).Length)
            {
                Debug.LogError("The number of slides is not equal to the number of states.");
                return;
            }

            ChangeSlide(_descriptionSceneManager.State);

            // subscribe to the event
            _descriptionSceneManager.OnStateChanged += ChangeSlide;
        }

        private void ChangeSlide(DescriptionState state)
        {
            // inactivate all slides
            foreach (var slide in slides)
            {
                slide.gameObject.SetActive(false);
            }

            // activate the slide of the current state
            slides[(int)state].gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _descriptionSceneManager.OnStateChanged -= ChangeSlide;
        }
    }
}