using System;
using UnityEngine;
using Zenject;

namespace AvoidGame.Play.Items
{
    /// <summary>
    /// アイテム
    /// </summary>
    public abstract class ItemBase : MonoBehaviour
    {
        public virtual bool DestroyOnItemCollectorHit => true;
    
        public event Action<Collider> OnItemCollectorHit;
    
        [Inject] private GameStateManager gameStateManager;

        private void OnTriggerEnter(Collider other)
        {
            if (gameStateManager.GameState != GameState.Playing) return;
            if (!other.TryGetComponent(out IItemCollectable _)) return;
        
            OnItemCollectorHit?.Invoke(other);
        
            if (DestroyOnItemCollectorHit)
            {
                Destroy(gameObject);
            }
        }
    }
}
