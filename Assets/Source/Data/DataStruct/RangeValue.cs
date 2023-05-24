using System;
using UnityEngine;

namespace Data.DataStruct
{
    [Serializable]
    public struct RangeFloatValue
    {
        public float Min => _min;
        public float Max => _max;

        [SerializeField, Min(0f)] private float _min, _max;        


        public RangeFloatValue(float min = 0f, float max = 1f)
        {
            _min = Mathf.Max(0f, min);
            _max = Mathf.Max(0f, max);
        }
    }


    [Serializable]
    public struct RangeIntValue
    {
        public int Min => _min;
        public int Max => _max;

        [SerializeField, Min(0)] private int _min, _max;        


        public RangeIntValue(int min = 0, int max = 1)
        {
            _min = Mathf.Max(0, min);
            _max = Mathf.Max(0, max);
        }
    }
}