using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play.Items
{
    public class SpeedDownItem : ItemBase
    {
        [SerializeField] private float decelerate = 0.1f;

        void Start()
        {
            OnItemCollectorHit += OnOnItemCollectorHit;
        }

        [Inject] SpeedManager _speedManager;

        private void OnOnItemCollectorHit(Collider itemCollector)
        {
            if(itemCollector.TryGetComponent(out IItemCollectable itemCollectable))
            {
                Debug.Log($"Hit! : {-decelerate:0.0}");
                _speedManager.AddPlayerSpeed(-decelerate);
            }
        }
    }
}