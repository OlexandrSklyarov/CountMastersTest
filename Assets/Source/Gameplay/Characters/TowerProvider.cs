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
            CalculateRows(stickmanCount);
            return GetPositions(stickmanCount);
        }


        private void CalculateRows(int count)
        {            
            var rowCount = 1;

            while (count > 0)
            {
                if (count >= rowCount)
                {
                    _towerRowCounts.Add(rowCount);
                    count -= rowCount;
                    rowCount++;

                    if (rowCount > _config.MaxCountInRow) rowCount = _config.MaxCountInRow;

                    continue;
                }

                for(int i = 0; i < count; i++)
                {
                    _towerRowCounts[^(i + 1)] += 1;
                }                
               
                count = 0; 
            }
        }


        private IEnumerable<Vector3> GetPositions(int unitCount)
        {    
            var result = new List<Vector3>();
            var origin = Vector3.zero;

            var sizeCell = _config.X_Offset * 2;
            var offset_Y = 0f;

           _towerRowCounts.Reverse();

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