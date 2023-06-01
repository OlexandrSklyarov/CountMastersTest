using UnityEngine;
using Source.Data;

namespace Source.Gameplay.Environment
{
    public class FinishStairs : MonoBehaviour
    {
        [SerializeField] private FinishLine _finish;
        [SerializeField] private FinishStairsData _config;

        private int _previousColorIndex;

        private void Awake() 
        {
            _finish.FinishTriggerEvent += Build;
        }


        private FinishStairsData.StairColor GetRandomColor()
        {            
            var index = _previousColorIndex;

            while(index == _previousColorIndex)
            {
                index = UnityEngine.Random.Range(0, _config.BlockColors.Length);
            }

            _previousColorIndex = index;

            return _config.BlockColors[_previousColorIndex];
        }


        private void Build(int rowCount)
        {
            _finish.FinishTriggerEvent -= Build;

            for(int i = 0; i < rowCount + _config.MinBlocksAmount; i++)
            {
                var block = Instantiate(_config.BlockPrefab, transform);

                block.transform.localPosition = Vector3.zero + 
                    transform.up * i * _config.Y_Offset +
                    transform.forward * i * _config.Z_Offset;                

                block.Init((i+1) * _config.PointsStep, GetRandomColor());
            }
        }
    }
}