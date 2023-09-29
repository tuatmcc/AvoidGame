using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpeedUpItem : ItemBase
{
    void Start()
    {
        OnItemCollectorHit += OnOnItemCollectorHit;
    }

    [Inject] SpeedManager _speedManager;

    private void OnOnItemCollectorHit(Collider itemCollector)
    {
        if(itemCollector.TryGetComponent(out IItemCollectable itemCollectable))
        {
            _speedManager.AddPlayerSpeed(0.1f);
        }
    }
}