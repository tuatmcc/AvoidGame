using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play.Items
{
    public class SpeedDownItem : ItemBase
    {
        [SerializeField] private float decelerate = 0.1f;
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
                _speedManager.AddPlayerSpeed(-decelerate);
            }
        }
    }
}