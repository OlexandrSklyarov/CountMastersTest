using System.Collections.Generic;
using Gameplay.Factories;
using Source.Data;
using Source.Gameplay.Characters;

namespace Source.Services
{
    public sealed class StickmanFactory
    {
        private readonly Dictionary<StickmanType, EntityFactory<Stickman>> _factories;

        public StickmanFactory(StickmanData[] poolData)
        {
            _factories = new Dictionary<StickmanType, EntityFactory<Stickman>>();

            for (int i = 0; i < poolData.Length; i++)
            {
                var data = poolData[i];
                var factory = new EntityFactory<Stickman>(data.Prefab, data.PoolSize);

                _factories.Add(data.Type, factory);
            }
        }


        public Stickman Get(StickmanType type)
        {
            var stickman = _factories[type].GetItem((s, storage) =>
            {
                s.Init(storage);
            });            

            return stickman;
        }
    }
}