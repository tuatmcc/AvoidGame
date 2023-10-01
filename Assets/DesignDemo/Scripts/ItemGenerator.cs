using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Random = UnityEngine.Random;

/// <summary>
/// コース上にアイテムを配置する
/// </summary>
public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private List<Pattern> _patterns = new();
    [SerializeField] private GameObject _stageProps;
    [SerializeField] private int _itemCount = 30;
    [SerializeField] private float _speed = 1.0f;
    [Inject] private SpeedManager _speedManager;
    
    private void Start()
    {
        var patterns = RandomPickPattern(_itemCount);
        var pos = new Vector3(0, 1, 0);
        foreach (var pattern in patterns)
        {
            pos.z += 30;
            Instantiate(pattern, pos, Quaternion.identity, this.transform);
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(0, 0, -_speed * _speedManager.Speed));
    }

    private List<Pattern> RandomPickPattern(int count)
    {
        List<Pattern> result = new();
        for(int i = 0; i < count; i++)
        {
            int index = Random.Range(0, _patterns.Count);
            result.Add(_patterns[index]);
        }
        return result;
    }
}
