using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play.Items
{
    public class SpeedUpItem : ItemBase
    {
        [SerializeField] private float accelerate = 0.1f;

        void Start()
        {
            OnItemCollectorHit += OnOnItemCollectorHit;
        }

        [Inject] SpeedManager _speedManager;

        private void OnOnItemCollectorHit(Collider itemCollector)
        {
            if(itemCollector.TryGetComponent(out IItemCollectable itemCollectable))
            {
                Debug.Log($"Hit! : {accelerate:0.0}");
                _speedManager.AddPlayerSpeed(accelerate);
            }
        }
    }
}