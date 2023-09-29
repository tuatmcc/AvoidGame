using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpeedDownItem : ItemBase
{
    // Start is called before the first frame update
    void Start()
    {
        OnItemCollectorHit += OnOnItemCollectorHit;
    }

    [Inject] SpeedManager _speedManager;

    // Update is called once per frame
    private void OnOnItemCollectorHit(Collider itemCollector)
    {
        if(itemCollector.TryGetComponent(out IItemCollectable itemCollectable))
        {
            _speedManager.AddPlayerSpeed(0.1f);
        }
    }
}