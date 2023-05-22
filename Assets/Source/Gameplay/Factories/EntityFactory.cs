using System;
using Services.Pooling;
using UnityEngine;

namespace Gameplay.Factories
{
    public class EntityFactory<T> : IFactoryStorage<T> where T : MonoBehaviour
    {
        private readonly Pool<T> _pool;
        

        public EntityFactory(T prefab, int poolSize)
        {
            var container = new GameObject($"[Factory - {typeof(T)}]").transform;            
            _pool = new Pool<T>(prefab.gameObject, container, poolSize);
        }
              

        public T GetItem(Action<T, IFactoryStorage<T>> onCreate) 
        {
            var item = _pool.GetElement();
            onCreate?.Invoke(item, this);
            return item;
        }
        
        
        void IFactoryStorage<T>.ReturnToStorage(T item) => _pool.ReturnToPool(item);
    }
}