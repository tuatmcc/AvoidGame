using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play.Items
{
    public class SpeedUpItem : ItemBase
    {
        [SerializeField] private float accelerate = 0.1f;
        private bool used = false;

        void Start()
        {
            OnItemCollectorHit += OnOnItemCollectorHit;
        }

        [Inject] SpeedManager _speedManager;

        private void OnOnItemCollectorHit(Collider itemCollector)
        {
            if(!used)
            {
                used = true;
                Debug.Log($"Hit! : {accelerate:0.0}");
                _speedManager.AddPlayerSpeed(accelerate);
            }
        }
    }
}