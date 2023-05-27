using System;
using System.Collections.Generic;
using System.Linq;
using Source.Data;
using UnityEngine;

namespace Source.Gameplay.Characters
{
    public class TowerProvider
    {
        private readonly TowerData _config;
        private readonly List<int> _towerRowCounts = new();


        public TowerProvider(TowerData config)
        {
            _config = config;
        }


        public IEnumerable<Vector3> Create(int stickmanCount)
        {
            Debug.Log($"before units {stickmanCount}");

            FillData(stickmanCount);

            int sum = _towerRowCounts.Sum();
            Debug.Log($"after units {sum}");
            foreach (var count in _towerRowCounts)
                Debug.Log($"after units {count}");

            return GetPositions(stickmanCount);
        }


        private void FillData(int count)
        {
            var unitInRow = (count / _config.MaxUnitPerRow >= _config.MaxUnitPerRow) ?
                _config.MaxUnitPerRow : (int)(_config.MaxUnitPerRow * _config.DivScale);

            while (count > 0)
            {
                if (count <= unitInRow)
                {
                    _towerRowCounts.Add(count);
                    break;
                }
                
                for (var i = 0; i < 2; i++)
                {
                    _towerRowCounts.Add(unitInRow);
                    count -= unitInRow;

                }

                unitInRow = Mathf.Max(1, --unitInRow);
            }
        }


        private IEnumerable<Vector3> GetPositions(int unitCount)
        {    
            var result = new List<Vector3>();
            var origin = Vector3.zero;

            var sizeCell = _config.X_Offset * 2;
            var offset_Y = 0f;

            foreach (var countInLine in _towerRowCounts)
            {
                var magnitude = countInLine * sizeCell;
                var offset_X = origin.x - magnitude / 2f;

                for (int i = 0; i < countInLine; i++)
                {
                    result.Add(new Vector3
                    (
                        origin.x + offset_X,
                        origin.x + offset_Y,
                        origin.z
                    ));

                    offset_X += sizeCell;
                }

                offset_Y += _config.Y_Offset;
            }
           
            return (result.Count > unitCount) ? result.Take(unitCount) : result;
        }
    }
}