using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AvoidGame.Play
{
    /// <summary>
    /// パス上にアイテムを生成する
    /// </summary>
    public class ItemGenerator : MonoBehaviour
    {
        [SerializeField] CinemachinePath path;
        [SerializeField] List<CinemachineDollyCart> itemPatterns;
        [SerializeField] private int itemCount = 15;


        private void AlignItemPatterns()
        {
            var space = path.PathLength / (itemCount + 1);
            for (var i = 0; i < itemCount; i++)
            {
                var item = Instantiate(itemPatterns[Random.Range(0, itemPatterns.Count)].gameObject);
                var cart = item.GetComponent<CinemachineDollyCart>();
                cart.m_Position = space * (i + 1);
                cart.m_Path = path;
            }
        }

        void Start()
        {
            AlignItemPatterns();
        }
    }
}